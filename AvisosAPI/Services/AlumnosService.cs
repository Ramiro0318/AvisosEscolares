using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using AvisosAPI.Validators;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace AvisosAPI.Services
{
    public class AlumnosService
    {
        private readonly Repository<Alumno> repository;
        private readonly Repository<Clase> claseRepository;
        private readonly IMapper mapper;

        public AlumnosService(Repository<Alumno> repository, Repository<Clase> claseRepository, IMapper mapper)
        {
            this.repository = repository;
            this.claseRepository = claseRepository;
            this.mapper = mapper;
        }

        public AlumnoDetallesDTO VerDetallesAlumno(int idUsuario, int userMaestro)
        {
            var alumno = repository.Query().Include(x => x.IdClaseNavigation).Include(x=>x.Avisopersonal).FirstOrDefault(x => x.Id == idUsuario && x.Eliminado == false);
            if (alumno == null)
            {
                throw new KeyNotFoundException();
            }
            if (alumno.IdClaseNavigation?.IdMaestro != userMaestro)
            {
                throw new AccessViolationException();
            }
            var alumnoDTO = mapper.Map<AlumnoDetallesDTO>(alumno);
            alumnoDTO.ListaAvisosAlumno = alumno.Avisopersonal.Where(x => x.Eliminado == false).Select(x => mapper.Map<AvisoPersonalMaestroDTO>(x)).ToList();
            return alumnoDTO;
        }

        public void AgregarAlumno(AgregarAlumnoDTO dto, int userMaestro)
        {
            var clase = claseRepository.Get(dto.IdClase);
            if (clase == null)
            {
                throw new KeyNotFoundException();
            }
            if (clase.IdMaestro != userMaestro)
            {
                throw new AccessViolationException();
            }
            var alumno = mapper.Map<Alumno>(dto);
            repository.Insert(alumno);
        }
        public void EditarAlumno(EditarAlumnoDTO dto, int userMaestro)
        {
            var alumno = repository.Query().Include(x => x.IdClaseNavigation).FirstOrDefault(x => x.Id == dto.Id && x.Eliminado == false);
            if (alumno == null)
            {
                throw new KeyNotFoundException();
            }
            if (alumno.IdClaseNavigation?.IdMaestro != userMaestro)
            {
                throw new AccessViolationException();
            }

            mapper.Map(dto, alumno);
            repository.Update(alumno);
        }

        public void EliminarAlumno(int idAlumno, int userMaestro)
        {
            var alumno = repository.Query().Include(x => x.IdClaseNavigation).FirstOrDefault(x => x.Id == idAlumno && x.Eliminado == false);
            if (alumno == null)
            {
                throw new KeyNotFoundException();
            }
            if (alumno.IdClaseNavigation?.IdMaestro != userMaestro)
            {
                throw new AccessViolationException();
            }
            alumno.Eliminado = true;
            repository.Update(alumno);  
        }

    }
}
