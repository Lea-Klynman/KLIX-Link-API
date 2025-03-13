using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.Entities;

namespace KLIX_Link_Service.Post_Modle
{
    public class UserFilePostmodel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string FilePassword { get; set; }
        public bool IsActive { get; set; }
    }
}
