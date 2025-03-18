using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.Entities;

namespace KLIX_Link_Core.DTOS
{
    public class UserFileDto
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        public string Name { get; set; }

        public string FileLink { get; set; }

        public string EncryptedLink { get; set; }

        public string FilePassword { get; set; }
        public DateOnly CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public string FileType { get; set; }



    }
}
