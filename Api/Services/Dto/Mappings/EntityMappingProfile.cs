using App.Data.Entities;
using App.Service.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Service.Dto
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateForwardMap();
        }
        public void CreateForwardMap()
        {
            CreateMap<RegistrationViewModel, AppIdentityUser>();
            CreateMap<AppIdentityUser, UserModel>().ReverseMap();
        }
    }
}
