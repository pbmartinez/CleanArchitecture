﻿using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public partial class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CategoriaDtoForCreate>().ReverseMap();
            CreateMap<Categoria, CategoriaDtoForUpdate>().ReverseMap();
        }
    }
}
