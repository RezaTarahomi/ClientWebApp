using AutoMapper;
using ClientWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientWebApp.Configurations
{
    public class MapperInitialize :Profile
    {
        public MapperInitialize()
        {
            CreateMap<ApiUser,RegisterViewModel>().ReverseMap().ForMember(dto => dto.Image, opt => opt.Ignore());
            CreateMap<RegisterViewModel, ApiUser>().ReverseMap().ForMember(dto => dto.Image, opt => opt.Ignore());
            CreateMap<ApiUser, EditViewModel>().ReverseMap().ForMember(dto => dto.Image, opt => opt.Ignore());
            CreateMap<EditViewModel, ApiUser>().ReverseMap().ForMember(dto => dto.Image, opt => opt.Ignore());
        }
    }
}
