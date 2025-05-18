using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.DTOS;
using KLIX_Link_Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace KLIX_Link_Core.Services
{
    public interface IUserFileService
    {
        // GET
        public Task<IEnumerable<UserFileDto>> GetAllUserFilesAsync();
        public Task<UserFileDto> GetUserFileByIdAsync(int id);
        public Task<IEnumerable<UserFileDto>> GetUserFilesByUserIdAsync(int userId);
        public Task<List<UserFileDto>> GetFileshareByEmail(string email);
        public Task<FileContentResult> GetDecryptFileAsync(SharingFileDTO decryption);

        public Task<FileContentResult> GetEncryptFileAsync(SharingFileDTO decryption);

        public  Task<string> GeneratePresignedUrl(string fileName);


        // PUT
        public Task<string> UploadFileAsync(IFormFile file, string fileName, string password, int userId, string type);

        // POST
        public Task<bool> IsFileNameExist(int id, string name);
        public Task<bool> CheckingIsAllowedViewAsync(string email, SharingFileDTO sharingFile);

        public Task<SharingFileDTO> SharingFileAsync(int id, string email);

        public Task<bool> UpdateFileNameAsync(int fileId, string newFileName);

        // DELETE
        public Task<bool> DeleteUserFileAsync(int id);


    }
}
