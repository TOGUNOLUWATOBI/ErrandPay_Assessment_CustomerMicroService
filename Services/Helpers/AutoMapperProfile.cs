using AutoMapper;
using Models.DTOs;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDTO, User>();
        }
    }
}
