using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.DTOS;
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
        public Task<bool> IsFileNameExist(int id, string name);
        // PUT
        public Task<string> UploadFileAsync(IFormFile file, string fileName, string password, int userId, string type);

        // POST
        public Task<FileContentResult> GetDecryptFileAsync(string encryptedLink, string password);

        public Task<bool> UpdateFileNameAsync(int fileId, string newFileName);

        // DELETE
        public Task<bool> DeleteUserFileAsync(int id);

    }
}
