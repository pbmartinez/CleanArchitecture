﻿using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public partial class EscuelaProfile : Profile
    {
        public EscuelaProfile()
        {
            CreateMap<Escuela, EscuelaDto>().ReverseMap();
            CreateMap<Escuela, EscuelaDtoForCreate>().ReverseMap();
            CreateMap<Escuela, EscuelaDtoForUpdate>().ReverseMap();
        }
    }
}
