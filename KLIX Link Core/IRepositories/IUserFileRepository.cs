using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.Entities;

namespace KLIX_Link_Core.IRepositories
{
    public interface IUserFileRepository
    {
        //GET
        Task<List<UserFile>> GetAllFilesAsync();
        Task<UserFile> GetFileByIdAsync(int id);
        Task<UserFile> GetFileByNameAsync(string name);
        Task<UserFile[]> GetUserFilesByUserIdAsync(int userId);
        public Task<bool> IsFileNameExistsAsync(int ownerId, string fileName);

        //PUT
        Task<UserFile> AddFileAsync(UserFile file);
        Task<bool> updateFileNameAsync(UserFile userFile);


        //DELETE
        Task<bool> DeleteFileAsync(int id);


    }
}
