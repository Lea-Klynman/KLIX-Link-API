﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.Entities;

namespace KLIX_Link_Core.IRepositories
{
    public interface IPermissionRepository
    {
        public Task<Permission> AddPermissionAsync(Permission permission);
        public Task<List<Permission>> GetPermissionsAsync();
        public Task<Permission> GetPermissionByIdAsync(int id);
        public Task<Permission> GetPermissionByNameAsync(string name);
        public Task<bool> RemovePermissionAsync(int id);
        public Task<Permission> UpdatePermissionAsync(int id, Permission permission);
    }
}
