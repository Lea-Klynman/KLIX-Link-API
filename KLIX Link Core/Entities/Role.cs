﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KLIX_Link_Core.Entities;
using System.Security;

namespace KLIX_Link_Core.Entities
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        public string Description { get; set; }

        [Required]
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public DateOnly UpdatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public ICollection<User>? Users { get; set; } = new List<User>();
        public ICollection<Permission>? Permissions { get; set; }= new List<Permission>();

    }
}