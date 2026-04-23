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
                if (x.FechaLectura == null)
                {
                    dto.Leido = false;
                }
                else
                {
                    dto.Leido = true;
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
            //if (alumno.Id != userAlumno)
            //{
            //    throw new AccessViolationException();
            //}
            var avisos = avisoGeneralRepository.Query().Include(x=>x.IdMaestroNavigation).Where(x => x.Eliminado == false && x.FechaVigencia.Date >= DateTime.Now.Date).ToList();
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
                dto.Maestro = x.IdMaestroNavigation.Nombre;
                return dto;
            }).ToList();
            alumno.UltimaVistaBandeja = DateTime.Now;
            alumnoRepository.Update(alumno);
            return lista;
        }

        public List<AvisoGeneralDTO> VerAvisosGeneralesMaestro(int idMaestro, int userMaestro)
        {
            var maestro = maestroRepository.Query().FirstOrDefault(x => x.Id == idMaestro);
            if (maestro == null)
            {
                throw new KeyNotFoundException();
            }
            //if (maestro.Id != userMaestro)
            //{
            //    throw new AccessViolationException();
            //}
            var avisos = avisoGeneralRepository.Query().Include(x=>x.IdMaestroNavigation).Where(x => x.Eliminado == false && x.FechaVigencia.Date >= DateTime.Now.Date).ToList();
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
                dto.Maestro = x.IdMaestroNavigation.Nombre;
                return dto;
            }).ToList();
            maestro.UltimaVistaBandeja = DateTime.Now;
            maestroRepository.Update(maestro);
            return lista;
        }

        public AvisoPersonalDetallesDTO VerDetallesAvisoPersonalAlumno(int idAviso, int userAlumno)
        {
            var aviso = avisoPersonalRepository.Query().Include(x=>x.IdMaestroNavigation).FirstOrDefault(x => x.Id == idAviso && x.Eliminado == false);
            if (aviso == null)
            {
                throw new KeyNotFoundException();
            }
            //if (aviso.IdAlumno != userAlumno)
            //{
            //    throw new AccessViolationException();
            //}
            if (aviso.FechaLectura == null)
            {
                aviso.FechaLectura = DateTime.Now;
            }
            var avisoDTO = mapper.Map<AvisoPersonalDetallesDTO>(aviso);
            avisoDTO.Maestro = aviso.IdMaestroNavigation.Nombre;
            avisoPersonalRepository.Update(aviso);
            return avisoDTO; 
        }

        public AvisoPersonalDetallesDTO VerDetallesAvisoPersonalMaestro(int idAviso, int userMaestro)
        {
            var aviso = avisoPersonalRepository.Query().Include(x=>x.IdMaestroNavigation).FirstOrDefault(x => x.Id == idAviso && x.Eliminado == false);
            if (aviso == null)
            {
                throw new KeyNotFoundException();
            }
            //if (aviso.IdMaestro != userMaestro)
            //{
            //    throw new AccessViolationException();
            //}
            var avisoDTO = mapper.Map<AvisoPersonalDetallesDTO>(aviso);
            avisoDTO.Maestro = aviso.IdMaestroNavigation.Nombre;
            return avisoDTO;
        }

        public void AgregarAvisoPersonal(AgregarAvisoPersonalDTO dto, int userMaestro)
        {
            var alumno = alumnoRepository.Query().Include(x=>x.IdClaseNavigation).FirstOrDefault(x=>x.Id == dto.IdAlumno &&  x.Eliminado == false);
            if (alumno == null)
            {
                throw new KeyNotFoundException();
            }
            //if(alumno.IdClaseNavigation?.IdMaestro != userMaestro)
            //{
            //    throw new AccessViolationException();
            //}
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
            //if (aviso.IdMaestro != userMaestro)
            //{
            //    throw new AccessViolationException();
            //}


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
            aviso.FechaCreacion = DateTime.Now;
            avisoGeneralRepository.Insert(aviso);
        }
        public void EliminarAvisoGeneral(int idAviso, int idMaestro)
        {
            var aviso = avisoGeneralRepository.Query().FirstOrDefault(x => x.Id == idAviso && x.Eliminado == false);
            if (aviso == null)
            {
                throw new KeyNotFoundException();
            }
            //if (aviso.IdMaestro !=  idMaestro)
            //{
            //    throw new AccessViolationException();
            //}
            aviso.Eliminado= true;
            avisoGeneralRepository.Update(aviso);
        }

        public AvisosNuevosAlumnosDTO VerNotificacionesAlumno(int idAlumno, int userAlumno)
        {
            var alumno = alumnoRepository.Query().Include(x => x.IdClaseNavigation).Include(x => x.Avisopersonal).FirstOrDefault(x => x.Id == idAlumno && x.Eliminado == false);
            if (alumno == null)
            {
                throw new KeyNotFoundException();
            }
            //if (alumno.Id != userAlumno)
            //{
            //    throw new AccessViolationException();
            //}
            AvisosNuevosAlumnosDTO dto = new AvisosNuevosAlumnosDTO();
            int cantidadGenerales = 0;
            int cantidadPersonales = 0;
            foreach (var aviso in avisoGeneralRepository.GetAll())
            {
                if (alumno.UltimaVistaBandeja == null || aviso.FechaCreacion >= alumno.UltimaVistaBandeja)
                {
                    cantidadGenerales++;
                }
            }
            foreach (var aviso in alumno.Avisopersonal)
            {
                if (aviso.Recibido == false)
                {
                    cantidadPersonales++;
                }
            }
            dto.AvisosGeneralesNuevos = cantidadGenerales;
            dto.AvisosPersonalesNuevos = cantidadPersonales;
            return dto;
        }

        public AvisosNuevosMaestroDTO VerNotificacionesMaestro(int idMaestro, int userMaestro)
        {
            var maestro = maestroRepository.Query().FirstOrDefault(x => x.Id == idMaestro);
            if (maestro == null)
            {
                throw new KeyNotFoundException();
            }
            //if (maestro.Id != userMaestro)
            //{
            //    throw new AccessViolationException();
            //}
            AvisosNuevosMaestroDTO dto = new AvisosNuevosMaestroDTO();
            int cantidadGenerales = 0;
            foreach (var aviso in avisoGeneralRepository.GetAll())
            {
                if (maestro.UltimaVistaBandeja == null || aviso.FechaCreacion >= maestro.UltimaVistaBandeja)
                {
                    cantidadGenerales++;
                }
            }
            dto.AvisosGeneralesNuevos = cantidadGenerales;
            return dto;
        }
    }
}
