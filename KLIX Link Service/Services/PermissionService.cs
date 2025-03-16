using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KLIX_Link_Core.DTOS;
using KLIX_Link_Core.Entities;
using KLIX_Link_Core.IRepositories;
using KLIX_Link_Core.IServices;

namespace KLIX_Link_Service.Services
{

    public class PermissionService : IPermissionService
    {
        readonly IPermissionRepository _permissionRepository;
        readonly IMapper _mapper;

        public PermissionService(IPermissionRepository permissionRepository,IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<PermissionDTO> AddPermissionAsync(PermissionDTO permission)
        {
            var permissionEntity = _mapper.Map<Permission>(permission);
            var addedPermission = await _permissionRepository.AddPermissionAsync(permissionEntity);
            return _mapper.Map<PermissionDTO>(addedPermission);
        }

        public async Task<PermissionDTO> GetPermissionByIdAsync(int id)
        {
            return _mapper.Map< PermissionDTO>(await _permissionRepository.GetPermissionByIdAsync(id));
        }

        public async Task<PermissionDTO> GetPermissionByNameAsync(string name)
        {
            return _mapper.Map< PermissionDTO>( await _permissionRepository.GetPermissionByNameAsync(name));
        }

        public async Task<List<PermissionDTO>> GetPermissionsAsync()
        {
            var permissions = await _permissionRepository.GetPermissionsAsync();
            return _mapper.Map<List<PermissionDTO>>(permissions);
        }

        public async Task<bool> RemovePermissionAsync(int id)
        {
            return await _permissionRepository.RemovePermissionAsync(id);
        }

        public async Task<PermissionDTO> UpdatePermissionAsync(int id, PermissionDTO permission)
        {
            var permissionEntity = _mapper.Map<Permission>(permission);
            var updatedPermission = await _permissionRepository.UpdatePermissionAsync(id, permissionEntity);
            return _mapper.Map<PermissionDTO>(updatedPermission);
        }
    }
}
