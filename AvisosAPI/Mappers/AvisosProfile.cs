using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;

namespace AvisosAPI.Mappers
{
    public class AvisosProfile:Profile
    {
        public AvisosProfile()
        {
            CreateMap<AgregarAvisoPersonalDTO, Avisopersonal>();
            CreateMap<AgregarAvisoGeneralDTO, Avisogeneral>();
            CreateMap<Avisopersonal, AvisoPersonalAlumnoDTO>();
            CreateMap<Avisopersonal, AvisoPersonalMaestroDTO>();
            CreateMap<Avisopersonal, AvisoPersonalDetallesDTO>();
            CreateMap<Avisogeneral, AvisoGeneralDTO>();
        }
    }
}
