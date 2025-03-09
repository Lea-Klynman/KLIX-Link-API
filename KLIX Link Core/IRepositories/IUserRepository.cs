using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.Entities;

namespace KLIX_Link_Core.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<User> GetUserByIdAsync(int id);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<User> AddUserAsync(User user);

    }
}
