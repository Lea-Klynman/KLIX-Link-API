using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KLIX_Link_Core.DTOS;
using KLIX_Link_Core.Entities;

namespace KLIX_Link_Core
{
    public class ProfileMapping: Profile
    {
        public ProfileMapping() {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserFile, UserFileDto>().ReverseMap();

        }
    }
}
