﻿using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public partial class MatriculaProfile : Profile
    {
        public MatriculaProfile()
        {
            CreateMap<Matricula, MatriculaDto>().ReverseMap();
            CreateMap<Matricula, MatriculaDtoForCreate>().ReverseMap();
            CreateMap<Matricula, MatriculaDtoForUpdate>().ReverseMap();
        }
    }
}
