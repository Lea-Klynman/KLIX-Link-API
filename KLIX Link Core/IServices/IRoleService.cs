using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.DTOS;
using KLIX_Link_Core.Entities;

namespace KLIX_Link_Core.IServices
{
    public interface IRoleService
    {
        public Task<bool> IsRoleHasPermissinAsync(string roleName, string permission);
        public Task<RoleDto> GetRoleByNameAsync(string roleName);
        public Task<IEnumerable<RoleDto>> GetRolesAsync();
        public Task<bool> AddPermissinForRoleAsync(string roleName, string permission);
        public Task<bool> AddRoleAsync(RoleDto role);
        public Task<bool> UpdateRoleAsync(int id, RoleDto role);
        public Task<bool> DeleteRoleAsync(int id);

    }
}
