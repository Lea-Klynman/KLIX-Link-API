using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KLIX_Link_Core.DTOS;
using KLIX_Link_Service.Post_Modle;

namespace KLIX_Link_Service
{
    public class PostModelProfileMapping:Profile
    {
        public PostModelProfileMapping()
        {
            CreateMap<UserPostmodel,UserDto>().ReverseMap();
            CreateMap<UserFilePostmodel,UserFileDto>().ReverseMap();
        }
    }
}
