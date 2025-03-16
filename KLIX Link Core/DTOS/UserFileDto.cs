using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.Entities;

namespace KLIX_Link_Core.DTOS
{
    public class UserFileDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int OwnerId { get; set; }
        public string FileLink { get; set; }
        public string EncryptedFileLink { get; set; }
        public string FilePassword { get; set; }
        public DateOnly CreateedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
