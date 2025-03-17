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


namespace KLIX_Link_Service.Services
{
    public class UserFileService : IUserFileService
    {
        private readonly IUserFileRepository _userFileRepository;
        private readonly IAmazonS3 _s3Client;
        private readonly string _encryptionKey;
        private readonly string _bucketName;
        readonly IMapper _mapper;

        public UserFileService(IUserFileRepository fileRepository, IConfiguration configuration,IMapper mapper)
        {
            _userFileRepository = fileRepository;
            _mapper = mapper;
            Console.WriteLine($"AWS_REGION from config: {configuration["AWS_REGION"]}");
            _s3Client = new AmazonS3Client(
                configuration["AWS_ACCESS_KEY_ID"],
                configuration["AWS_SECRET_ACCESS_KEY"],
                Amazon.RegionEndpoint.GetBySystemName(configuration["AWS_REGION"])
            );

            _encryptionKey = configuration["ENCRYPTION_KEY"];
            _bucketName = configuration["AWS_BUCKET_NAME"];
        }
       
        public async Task<bool> DeleteUserFileAsync(int id)
        {
            return await _userFileRepository.DeleteFileAsync(id);

        }

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
            return (IEnumerable<UserFileDto>)_mapper.Map<UserFileDto>(res);
        }

        public async  Task<bool> IsFileNameExist(int id,string name)
        {
            return await _userFileRepository.IsFileNameExistsAsync(id,name);
        }

      

        public async Task<bool> UpdateFileNameAsync(int fileId, string newFileName)
        {
            try
            {
                var userFile = await _userFileRepository.GetFileByIdAsync(fileId);
                if (userFile == null)
                {
                    return false;
                }
                userFile.Name = newFileName;
                var updateResult = await _userFileRepository.updateFileNameAsync(userFile);
                return updateResult;

            }
            catch (Exception)
            {
                return false;
            }
        }

       

        public async Task<string> UploadFileAsync(IFormFile file, string fileName, string password, int userId)
        {
            // הצפנת הקובץ
            byte[] encryptedData = EncryptFile(file, _encryptionKey, userId, fileName);

            // העלאה ל-S3
            var uploadRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = new MemoryStream(encryptedData),
                ContentType = file.ContentType
            };  

            await _s3Client.PutObjectAsync(uploadRequest);

            // יצירת קישור ציבורי ל-S3
            string fileUrl = $"https://{_bucketName}.s3.amazonaws.com/{fileName}";

            // הצפנת הקישור
            string encryptedLink = EncryptLink(fileUrl, _encryptionKey);

            // שמירה במסד הנתונים
            await _userFileRepository.AddFileAsync(new UserFile
            {
                OwnerId = userId,
                Name = fileName,
                FileLink = fileUrl,
                EncryptedLink = encryptedLink,
                FilePassword = password
            });

            return encryptedLink;
        }

        private byte[] EncryptFile(IFormFile file, string key, int userId, string fileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                using (var aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                    aes.IV = new byte[16]; // ניתן לשפר עם IV דינמי

                    using (var encryptor = aes.CreateEncryptor())
                    {
                        return encryptor.TransformFinalBlock(fileBytes, 0, fileBytes.Length);
                    }
                }
            }
        }

        private string EncryptLink(string data, string key)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16];

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }
    }
}
