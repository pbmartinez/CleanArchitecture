﻿using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public partial class EstudianteProfile : Profile
    {
        public EstudianteProfile()
        {
            CreateMap<Estudiante, EstudianteDto>().ReverseMap();
            CreateMap<Estudiante, EstudianteDtoForCreate>().ReverseMap();
            CreateMap<Estudiante, EstudianteDtoForUpdate>().ReverseMap();
        }
    }
}
