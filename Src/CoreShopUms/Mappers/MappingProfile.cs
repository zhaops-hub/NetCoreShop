using AutoMapper;
using CoreShopUms.Infrastructure.Entity;
using CoreShopUms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShopUms.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserModel>().ReverseMap(); // 双向映射
        }
    }
}
