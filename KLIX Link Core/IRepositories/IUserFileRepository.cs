﻿using System;
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
        public Task<List<UserFile>> GetAllFilesAsync();
        public Task<UserFile> GetFileByIdAsync(int id);
        public Task<UserFile> GetFileByNameAsync(string name);
        public Task<UserFile[]> GetUserFilesByUserIdAsync(int userId);
        public Task<bool> IsFileNameExistsAsync(int ownerId, string fileName);
        public Task<UserFile> GetFileByUrlAsync(string fileUrl);

        public Task<List<UserFile>> GetFileshareByEmail(string email);
        public Task<bool> CheckingIsAllowedEmailAsync(int id, string email);

        //POST
        public Task<UserFile> AddFileAsync(UserFile file);

        //PUT
        public Task<bool> UpdateFileNameAsync(UserFile userFile);
        public Task<bool> UpdateEmailListAsync(int id ,string email);


        //DELETE
        public Task<bool> DeleteFileAsync(int id);


    }
}
