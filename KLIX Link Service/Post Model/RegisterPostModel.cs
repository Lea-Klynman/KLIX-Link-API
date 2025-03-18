using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLIX_Link_Core.DTOS;

namespace KLIX_Link_Service.Post_Model
{
    public class RegisterPostModel
    {
        public UserDto User { get; set; }
        public string[] Roles { get; set; }
    }
}
