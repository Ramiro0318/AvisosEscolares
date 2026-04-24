using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AvisosAPI.Services
{
    public class ClasesService
    {
        private readonly Repository<Clase> repository;
        private readonly IMapper mapper;

        public ClasesService(Repository<Clase> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public ClaseDTO GetClaseByMaestro(int idMaestro, int userMaestro)
        {
            var clase = repository.Query().Include(x => x.Alumno).FirstOrDefault(x=>x.IdMaestro == idMaestro);
            if (clase == null)
            {
                throw new KeyNotFoundException();
            }
            //if (clase.IdMaestro != userMaestro)
            //{
            //    throw new AccessViolationException();
            //}
            var claseDTO = mapper.Map<ClaseDTO>(clase);    
            claseDTO.ListaAlumnos = clase.Alumno.Where(x=>x.Eliminado == false).Select(x=>mapper.Map<AlumnoListaDTO>(x)).ToList();
            return claseDTO;
        }

        public void AgregarClase(AgregarClaseDTO dto)
        {
            if (repository.GetAll().Any(x=>x.IdMaestro == dto.IdMaestro))
            {
                throw new InvalidOperationException();
            }
            var clase = mapper.Map<Clase>(dto);  
            repository.Insert(clase);   
        }
        public void EditarClase(EditarClaseDTO dto, int userMaestro)
        {
            var clase = repository.Query().Include(x => x.Alumno).FirstOrDefault(x => x.Id == dto.Id);
            if (clase == null)
            {
                throw new KeyNotFoundException();
            }
            //if (clase.IdMaestro != userMaestro)
            //{
            //    throw new AccessViolationException();
            //}
            mapper.Map(dto, clase);
            repository.Update(clase);
        }
    }
}
