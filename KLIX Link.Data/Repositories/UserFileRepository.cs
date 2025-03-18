using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.Entities;
using KLIX_Link_Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace KLIX_Link.Data.Repositories
{
    public class UserFileRepository : IUserFileRepository
    {
        private readonly IDataContext _dataContext;

        public UserFileRepository(IDataContext context)
        {
            _dataContext = context;
        }
        public async Task<UserFile> AddFileAsync(UserFile file)
        {
            try
            {
                await _dataContext._Files.AddAsync(file);
                await _dataContext.SaveChangesAsync();
                return file;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteFileAsync(int id)
        {
            try
            {
                var res = await _dataContext._Files.FirstOrDefaultAsync(file => file.Id == id);
                if (res == null) return false;
                _dataContext._Files.Remove(res);
                await _dataContext.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<UserFile>> GetAllFilesAsync()
        {
            return await _dataContext._Files.ToListAsync();

        }

        public async Task<UserFile> GetFileByIdAsync(int id)
        {
            return await _dataContext._Files.FirstOrDefaultAsync(file => file.Id == id);
        }

        public async Task<UserFile> GetFileByNameAsync(string name)
        {
            return await _dataContext._Files.FirstOrDefaultAsync(file => file.Name == name);
        }

        public async Task<UserFile[]> GetUserFilesByUserIdAsync(int userId)
        {
            var userFiles = await _dataContext._Files.Where(file => file.OwnerId == userId).ToArrayAsync();
            return userFiles;
        }

        public async Task<bool> updateFileNameAsync(UserFile userFile)
        {

            var userFileToUpdate = await _dataContext._Files.FirstOrDefaultAsync(file => file.Id == userFile.Id);
            if (userFileToUpdate == null) return false;
            if (userFileToUpdate.Name == userFile.Name) return true;
            userFileToUpdate.Name = userFile.Name;
            try
            {
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> IsFileNameExistsAsync(int ownerId, string fileName)
        {
            var res= await _dataContext._Files.Where(f => f.OwnerId == ownerId && f.Name == fileName).FirstOrDefaultAsync();
            return res != null;
        }
    }
    }


