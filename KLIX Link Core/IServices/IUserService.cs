using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.DTOS;
using KLIX_Link_Core.Entities;

namespace KLIX_Link_Core.Services
{
    public interface IUserService
    {
        //Get
        public Task<IEnumerable<UserDto>> GetAllUsersAsync();
        public Task<UserDto> GetUserByIdAsync(int id);
        public Task<UserDto> GetUserByEmailAsync(string email);


        //Put
        public Task<UserDto> RegisterAsync(UserDto user, string[] roles);

        //Post
        public Task<UserDto> LoginAsync(string email, string password);
        public Task<bool> UpdatePasswordAsync(int id, string password);
        public Task<bool> UpdateNameAsync(int id, string name);
        public Task<bool> UpdateRoleAsync(int id, Role role);
        public Task<bool> EnableUserAsync(int id);
        public Task<bool> DisableUserAsync(int id);

        //Delete
        public Task<bool> DeleteUserAsync(int id);

    }
}
