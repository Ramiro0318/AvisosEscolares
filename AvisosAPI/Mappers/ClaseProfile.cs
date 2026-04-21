using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;

namespace AvisosAPI.Mappers
{
    public class ClaseProfile:Profile
    {
        public ClaseProfile()
        {
            CreateMap<CrearClaseDTO, Clase>();
            CreateMap<EditarClaseDTO, Clase>();
            CreateMap<Clase, ClaseDTO>();
        }
    }
}
