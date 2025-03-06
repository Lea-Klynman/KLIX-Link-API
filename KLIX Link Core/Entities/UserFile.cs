using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLIX_Link_Core.Entities
{
    public class UserFile
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int OwnerId { get; set; }

        public string Name { get; set; }
        public string FileLink { get; set; }
        public string EncryptedLink { get; set; }
        public string FilePassword { get; set; }
        public DateOnly CreateedAt { get; set; }

    }
}
