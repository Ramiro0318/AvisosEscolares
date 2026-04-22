using AutoMapper;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AvisosAPI.Services
{
    public class AvisosService
    {
        private readonly Repository<Avisopersonal> avisoPersonalRepository;
        private readonly Repository<Avisogeneral> avisoGeneralRepository;
        private readonly Repository<Alumno> alumnoRepository;
        private readonly Repository<Maestro> maestroRepository;
        private readonly IMapper mapper;

        public AvisosService(Repository<Avisopersonal> avisoPersonalRepository, Repository<Avisogeneral> avisoGeneralRepository, Repository<Alumno> alumnoRepository, Repository<Maestro> maestroRepository, IMapper mapper)
        {
            this.avisoPersonalRepository = avisoPersonalRepository;
            this.avisoGeneralRepository = avisoGeneralRepository;
            this.alumnoRepository = alumnoRepository;
            this.maestroRepository = maestroRepository;
            this.mapper = mapper;
        }

        public List<AvisoPersonalAlumnoDTO> VerAvisosPersonalesAlumno(int idAlumno)
        {
            var avisos = avisoPersonalRepository.Query().Where(x => x.IdAlumno == idAlumno && x.Eliminado == false).ToList();
            var lista = avisos.Select(x =>
            {
                var dto = mapper.Map<AvisoPersonalAlumnoDTO>(x);

                if (x.Recibido == false)
                {
                    dto.Nuevo = true;
                }
                else
                {
                    dto.Nuevo = false;
                }
                return dto;
            }).ToList();

            foreach (var aviso in avisos)
            {
                if (aviso.Recibido == false)
                {
                    aviso.Recibido = true;
                    avisoPersonalRepository.Update(aviso);
                }
            }

            return lista;
        }

        public List<AvisoGeneralDTO> VerAvisosGeneralesAlumno(int idAlumno, int userAlumno)
        {
            var alumno = alumnoRepository.Query().FirstOrDefault(x => x.Id == idAlumno && x.Eliminado == false);
            if (alumno == null)
            {
                throw new KeyNotFoundException();
            }
            if (alumno.Id != userAlumno)
            {
                throw new AccessViolationException();
            }
            var avisos = avisoGeneralRepository.Query().Where(x => x.Eliminado == false && x.FechaVigencia >= DateTime.Now).ToList();
            var lista = avisos.Select(x => 
            {
                var dto = mapper.Map<AvisoGeneralDTO>(x);
                if (alumno.UltimaVistaBandeja == null || x.FechaCreacion >= alumno.UltimaVistaBandeja)
                {
                    dto.Nuevo = true;
                }
                else
                {
                    dto.Nuevo = false;
                }
                return dto;
            }).ToList();
            alumno.UltimaVistaBandeja = DateTime.Now;
            return lista;
        }

        public List<AvisoGeneralDTO> VerAvisosGeneralesMaestro(int idMaestro, int userMaestro)
        {
            var maestro = maestroRepository.Query().FirstOrDefault(x => x.Id == idMaestro);
            if (maestro == null)
            {
                throw new KeyNotFoundException();
            }
            if (maestro.Id != userMaestro)
            {
                throw new AccessViolationException();
            }
            var avisos = avisoGeneralRepository.Query().Where(x => x.Eliminado == false && x.FechaVigencia >= DateTime.Now).ToList();
            var lista = avisos.Select(x =>
            {
                var dto = mapper.Map<AvisoGeneralDTO>(x);
                if (maestro.UltimaVistaBandeja == null || x.FechaCreacion >= maestro.UltimaVistaBandeja)
                {
                    dto.Nuevo = true;
                }
                else
                {
                    dto.Nuevo = false;
                }
                return dto;
            }).ToList();
            maestro.UltimaVistaBandeja = DateTime.Now;
            return lista;
        }

        public AvisoPersonalDetallesDTO VerDetallesAvisoPersonalAlumno(int idAviso, int userAlumno)
        {
            var aviso = avisoPersonalRepository.Query().FirstOrDefault(x => x.Id == idAviso && x.Eliminado == false);
            if (aviso == null)
            {
                throw new KeyNotFoundException();
            }
            if (aviso.IdAlumno != userAlumno)
            {
                throw new AccessViolationException();
            }
            var avisoDTO = mapper.Map<AvisoPersonalDetallesDTO>(aviso);
            aviso.FechaLectura = DateTime.Now;
            avisoPersonalRepository.Update(aviso);
            return avisoDTO; 
        }

        public AvisoPersonalDetallesDTO VerDetallesAvisoPersonalMaestro(int idAviso, int userMaestro)
        {
            var aviso = avisoPersonalRepository.Query().FirstOrDefault(x => x.Id == idAviso && x.Eliminado == false);
            if (aviso == null)
            {
                throw new KeyNotFoundException();
            }
            if (aviso.IdMaestro != userMaestro)
            {
                throw new AccessViolationException();
            }
            var avisoDTO = mapper.Map<AvisoPersonalDetallesDTO>(aviso);
            return avisoDTO;
        }

        public void AgregarAvisoPersonal(AgregarAvisoPersonalDTO dto, int userMaestro)
        {
            var alumno = alumnoRepository.Query().Include(x=>x.IdClaseNavigation).FirstOrDefault(x=>x.Id == dto.IdAlumno &&  x.Eliminado == false);
            if (alumno == null)
            {
                throw new KeyNotFoundException();
            }
            if(alumno.IdClaseNavigation?.IdMaestro != userMaestro)
            {
                throw new AccessViolationException();
            }
            var aviso = mapper.Map<Avisopersonal>(dto);
            aviso.FechaCreacion = DateTime.Now;
            avisoPersonalRepository.Insert(aviso);
        }

        public void EliminarAvisoPersonal(int idAviso, int userMaestro)
        {
            var aviso = avisoPersonalRepository.Query().FirstOrDefault(x => x.Id == idAviso && x.Eliminado == false);
            if (aviso == null)
            {
                throw new KeyNotFoundException();
            }
            if (aviso.IdMaestro != userMaestro)
            {
                throw new AccessViolationException();
            }
            //if (DateTime.Now <= aviso.FechaCreacion.AddSeconds(30)) //REVISAR SI ES NECESARIO USAR DATETIME.UTCNOW
            //{
            //    aviso.Eliminado = true;
            //    avisoPersonalRepository.Update(aviso);
            //}
            //else
            //{
            //    throw new InvalidOperationException();
            //}
            aviso.Eliminado = true;
            avisoPersonalRepository.Update(aviso);
        }

        public void AgregarAvisoGeneral(AgregarAvisoGeneralDTO dto)
        {
            var maestro = maestroRepository.Get(dto.IdMaestro);
            if (maestro == null)
            {
                throw new KeyNotFoundException();
            }
            var aviso = mapper.Map<Avisogeneral>(dto);
            avisoGeneralRepository.Insert(aviso);
        }
        public void EliminarAvisoGeneral(int idAviso, int idMaestro)
        {
            var aviso = avisoGeneralRepository.Query().FirstOrDefault(x => x.Id == idAviso && x.Eliminado == false);
            if (aviso == null)
            {
                throw new KeyNotFoundException();
            }
            if (aviso.IdMaestro !=  idMaestro)
            {
                throw new AccessViolationException();
            }
            aviso.Eliminado= true;
            avisoGeneralRepository.Update(aviso);
        }
    }
}
