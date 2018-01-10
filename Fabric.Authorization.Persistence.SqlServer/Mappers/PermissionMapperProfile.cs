﻿using AutoMapper;

namespace Fabric.Authorization.Persistence.SqlServer.Mappers
{
    public class PermissionMapperProfile : Profile
    {
        public PermissionMapperProfile()
        {
            CreateMap<EntityModels.Permission, Domain.Models.Permission>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.PermissionId))
                .ReverseMap()
                .ForPath(x => x.SecurableItem.Name, opt => opt.MapFrom(src => src.SecurableItem));

            CreateMap<EntityModels.Permission, Domain.Models.Permission>()
                .ForMember(x => x.SecurableItem, opt => opt.MapFrom(src => src.SecurableItem.Name));
        }
    }
}