using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KLIX_Link_Core.Entities;

namespace KLIX_Link_Core.Entities
{
    [Table("Permissions")]
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PermissionName { get; set; }

        public string Description { get; set; }

        public ICollection<Role>? Roles { get; set; } = new List<Role>();
    }
}