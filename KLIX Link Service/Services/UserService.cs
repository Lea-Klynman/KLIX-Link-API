using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KLIX_Link_Core.DTOS;
using KLIX_Link_Core.Entities;
using KLIX_Link_Core.Repositories;
using KLIX_Link_Core.Services;

namespace KLIX_Link_Service.Services
{
    public class UserService : IUserService
    {
        readonly IUserRepository _userRepository;
        readonly IMapper _mapper;
        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //Get

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var res = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<UserDto[]>(res);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var res = await _userRepository.GetUserByEmailAsync(email);
            return _mapper.Map<UserDto>(res);
        }


        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var res = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserDto>(res);
        }

        public async Task<UserDto> LoginAsync(string email, string password)
        {
            var res = await _userRepository.LoginAsync(email, password);
            return _mapper.Map<UserDto>(res);
        }


        //put

        public async Task<UserDto> RegisterAsync(UserDto user)
        {
            var res = await _userRepository.AddUserAsync(_mapper.Map<User>(user));
            return _mapper.Map<UserDto>(res);
        }


        //Post
        public async Task<bool> UpdateNameAsync(int id, string name)
        {
            return await _userRepository.UpdateNameAsync(id, name);
        }


        public async Task<bool> UpdatePasswordAsync(int id, string password)
        {
            return await _userRepository.UpdatePasswordAsync(id, password);
        }

        public async Task<bool> UpdateRoleAsync(int id, Role role)
        {
            return await _userRepository.UpdateRoleAsync(id, role);
        }

        //Delete

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

        
    }
}
