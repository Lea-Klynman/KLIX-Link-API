﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLIX_Link_Core.DTOS
{
    public class PermissionDTO
    {
        public int Id { get; set; }

        public string PermissionName { get; set; }

        public string Description { get; set; }
    }
}
