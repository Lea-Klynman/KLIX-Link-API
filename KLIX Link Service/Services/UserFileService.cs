using Amazon.S3;
using Amazon.S3.Model;
using KLIX_Link_Core.Entities;
using KLIX_Link_Core.IRepositories;
using KLIX_Link_Core.IServices;
using KLIX_Link_Core.Services;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using KLIX_Link_Core.DTOS;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace KLIX_Link_Service.Services
{
    public class UserFileService : IUserFileService
    {

        private readonly IUserFileRepository _userFileRepository;
        private readonly S3Service _fileStorageService;
        private readonly IUserService _userService;
        private readonly string _encryptionKey;
        readonly IMapper _mapper;

        public UserFileService(IUserFileRepository fileRepository, S3Service fileStorageService, IConfiguration configuration, IMapper mapper,IUserService userService)
        {
            _userFileRepository = fileRepository;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
            _userService = userService;
            _encryptionKey = configuration["ENCRYPTION_KEY"];

        }


        //GET
        public async Task<IEnumerable<UserFileDto>> GetAllUserFilesAsync()
        {
            var res = await _userFileRepository.GetAllFilesAsync();
            return _mapper.Map<UserFileDto[]>(res);
        }


        public async Task<UserFileDto> GetUserFileByIdAsync(int id)
        {
            var res = await _userFileRepository.GetFileByIdAsync(id);
            return _mapper.Map<UserFileDto>(res);
        }


        public async Task<IEnumerable<UserFileDto>> GetUserFilesByUserIdAsync(int userId)
        {
            var res = await _userFileRepository.GetUserFilesByUserIdAsync(userId);
            return _mapper.Map<UserFileDto[]>(res);
        }


        public async Task<FileContentResult> GetDecryptFileAsync(SharingFileDTO decryption)
        {
            // פענוח הקישור כדי לקבל את הנתיב לקובץ ב-S3
            string fileUrl = DecryptLinkOrPassword(decryption.EncryptedLink, _encryptionKey);

            // חיפוש הקובץ במסד הנתונים
            var userFile = await _userFileRepository.GetFileByUrlAsync(fileUrl);
            if (userFile == null || userFile.FilePassword != decryption.Password)
            {
                return null; // סיסמה לא נכונה או קובץ לא נמצא
            }

            // הורדת הקובץ המוצפן מ-S3
            var encryptedFileBytes = await _fileStorageService.DownloadFileAsync(fileUrl);
            if (encryptedFileBytes == null)
            {
                return null; // הקובץ לא נמצא ב-S3
            }

            // פענוח הקובץ
            byte[] decryptedFile = DecryptFile(encryptedFileBytes, _encryptionKey);

            return new FileContentResult(decryptedFile, userFile.FileType)
            {
                FileDownloadName = userFile.Name + "." + userFile.FileType // שם ברירת מחדל לקובץ, אפשר להחליף בשם שמגיע ממסד הנתונים
            };

        }


        public async Task<List<UserFileDto>> GetFileshareByEmail(string email)
        {
            var res = await _userFileRepository.GetFileshareByEmail(email);
            var filteredFiles = res.Where(x => x.EmailAloowed.Any(e => email == e)).ToList();
            return _mapper.Map<List<UserFileDto>>(filteredFiles);
        }

        //PUT
        public async Task<bool> UpdateFileNameAsync(int fileId, string newFileName)
        {
            var userFile = await _userFileRepository.GetFileByIdAsync(fileId);
            if (userFile == null)
            {
                return false;
            }
            string oldFilePath = userFile.Name;
            string newFilePath = $"{newFileName}";
            try
            {
                var newLink = await _fileStorageService.UpdateFileNameAsync(oldFilePath, newFilePath);
                if (newLink == null)
                {
                    return false;
                }
                userFile.FileLink = newLink;
                userFile.EncryptedLink = EncryptLinkOrPassword(userFile.FileLink, _encryptionKey);
                userFile.Name = newFileName;
                return await _userFileRepository.UpdateFileNameAsync(userFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating file name in S3: {ex.Message}");
                return false;
            }
        }


        //POST
        public async Task<bool> IsFileNameExist(int id, string name)
        {
            return await _userFileRepository.IsFileNameExistsAsync(id, name);
        }

        public async Task<bool> CheckingIsAllowedViewAsync(string email, SharingFileDTO sharingFile)
        {
            string decryptionpassword= DecryptLinkOrPassword(sharingFile.Password, _encryptionKey);
            if(decryptionpassword != email)
            {
                return false;
            }
            string fileUrl = DecryptLinkOrPassword(sharingFile.EncryptedLink, _encryptionKey);
            var userFile = await _userFileRepository.GetFileByUrlAsync(fileUrl);
            return await _userFileRepository.CheckingIsAllowedEmailAsync(userFile.Id,email);
        }

        public async Task<SharingFileDTO> SharingFileAsync(int id, string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            var userFile = await _userFileRepository.GetFileByIdAsync(id);
            if (userFile == null) { return null; }
            await _userFileRepository.UpdateEmailListAsync(id, email);
            string password=EncryptLinkOrPassword(userFile.FilePassword, _encryptionKey);
            return new SharingFileDTO
            {
                EncryptedLink = userFile.EncryptedLink,
                Password = password
            };
        }
        
        
        public async Task<string> UploadFileAsync(IFormFile file, string fileName, string password, int userId, string type)
        {
            string fileType = type;
            // הצפנת הקובץ
            byte[] encryptedData = EncryptFile(file, _encryptionKey, userId, fileName);

            // העלאה ל-S3
            // יצירת קישור ציבורי ל-S3
            string fileUrl = await _fileStorageService.UploadFileAsync(file, fileName, encryptedData);
            if (fileUrl == null)
            {
                return null;
            }
            // הצפנת הקישור
            string encryptedLink = EncryptLinkOrPassword(fileUrl, _encryptionKey);

            // שמירה במסד הנתונים
            await _userFileRepository.AddFileAsync(new UserFile
            {
                OwnerId = userId,
                Name = fileName,
                FileLink = fileUrl,
                EncryptedLink = encryptedLink,
                FilePassword = password,
                FileType = fileType
            });

            return encryptedLink;
        }

        //DELETE
        public async Task<bool> DeleteUserFileAsync(int id)
        {
            try
            {
                var userFile = await _userFileRepository.GetFileByIdAsync(id);
                if (userFile == null)
                {
                    return false;
                }
                //הוצאת המפתח של הקובץ מהקישור
                var fileKey = userFile.FileLink.Contains("s3.amazonaws.com") ?
                 userFile.FileLink.Split(new[] { ".s3.amazonaws.com/" }, StringSplitOptions.None).Last() :
                 userFile.FileLink;

                if (!await _fileStorageService.DeleteFileAsync(fileKey))
                {
                    return false;
                }

                return await _userFileRepository.DeleteFileAsync(id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //Encrypt and Decrypt methods
        private byte[] EncryptFile(IFormFile file, string key, int userId, string fileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                using (var aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key.PadRight(24).Substring(0, 24)); // Ensures 32-byte key
                    aes.IV = new byte[16];

                    using (var encryptor = aes.CreateEncryptor())
                    {
                        return encryptor.TransformFinalBlock(fileBytes, 0, fileBytes.Length);
                    }
                }
            }
        }


        private string EncryptLinkOrPassword(string data, string key)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(24).Substring(0, 24)); // Ensures 32-byte key
                aes.IV = new byte[16];

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }


        private string DecryptLinkOrPassword(string encryptedLink, string key)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedLink);

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(24).Substring(0, 24)); // Ensures 32-byte key
                aes.IV = new byte[16]; // אותו IV ששימש בהצפנה

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }


        private byte[] DecryptFile(byte[] encryptedData, string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(24).Substring(0, 24)); // Ensures 32-byte key
                aes.IV = new byte[16]; // אותו IV ששימש בהצפנה

                using (var decryptor = aes.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                }
            }
        }

       
    }
}
