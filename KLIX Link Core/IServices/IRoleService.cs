using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.Entities;

namespace KLIX_Link_Core.IServices
{
    public interface IRoleService
    {
        public Task<bool> IsRoleHasPermissinAsync(string roleName, string permission);
        public Task<Role> GetRoleByNameAsync(string roleName);

        public Task<bool> AddPermissinForRoleAsync(string roleName, Permission permission);

    }
}
