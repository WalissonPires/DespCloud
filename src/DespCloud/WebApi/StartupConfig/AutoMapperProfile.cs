using AutoMapper;
using Base.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Database.Entities;

namespace WebApi.StartupConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<OrderServiceEty, OrderService>()
            .ReverseMap()
                .ForMember(x => x.Client, y => y.Ignore());
            CreateMap<OrderDetailEty, OrderDetail>()                
                .ForMember(x => x.Service, x => x.Ignore())
                .ForPath(x => x.Service.Honorary, x => x.MapFrom(a => a.Honorary))
                .ForPath(x => x.Service.Rate, x => x.MapFrom(a => a.Rate))
                .ForPath(x => x.Service.PlateCard, x => x.MapFrom(a => a.PlateCard))
                .ForPath(x => x.Service.Other, x => x.MapFrom(a => a.Other))
                .ForPath(x => x.Service.Name, x => x.MapFrom(a => a.Service.Name))
            .ReverseMap()
                .ForMember(x => x.Honorary, x => x.MapFrom(a => a.Service.Honorary))
                .ForMember(x => x.Rate, x => x.MapFrom(a => a.Service.Rate))
                .ForMember(x => x.PlateCard, x => x.MapFrom(a => a.Service.PlateCard))
                .ForMember(x => x.Other, x => x.MapFrom(a => a.Service.Other))
                .ForMember(x => x.ServiceId, x => x.MapFrom(a => a.ServiceId))
                .ForMember(x => x.CompanyId, x => x.Ignore())
                .ForMember(x => x.Total, x => x.Ignore())
                ;
            CreateMap<CompanyEty, Company>().ReverseMap();
            CreateMap<ClientEty, Client>().ReverseMap();
            CreateMap<ServiceEty, Service>().ReverseMap();
            CreateMap<VehicleEty, Vehicle>().ReverseMap();
            CreateMap<VehicleTypeEty, VehicleType>().ReverseMap();
            CreateMap<OrderStatusEty, OrderStatus>().ReverseMap();
            CreateMap<AddressEty, Address>().ReverseMap();

            CreateMap<AddressCountyEty, County>();
            CreateMap<AddressCityEty, City>();
            CreateMap<AddressDistrictEty, District>();
        }
    }
}
