using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using System.Runtime;

namespace AvisosAPI.Mappers
{
    public class AlumnoProfile:Profile
    {
        public AlumnoProfile()
        {
            CreateMap<AgregarAlumnoDTO, Alumno>();
            CreateMap<Alumno, AlumnoListaDTO>();
            CreateMap<Alumno, AlumnoDetallesDTO>();
            CreateMap<EditarAlumnoDTO, Alumno>();
        }
    }
}
