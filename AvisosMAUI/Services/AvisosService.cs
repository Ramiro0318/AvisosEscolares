using AvisosAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace AvisosMAUI.Services
{
    public class AvisosService
    {
        string baseUrl = "https://localhost:7184/";
        HttpClient cliente;

        public AvisosService()
        {
            cliente = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public async Task<(bool res, List<string>? errores)> AgregarAlumno(AgregarAlumnoDTO dto)
        {
            try
            {
                var resultado = await cliente.PostAsJsonAsync("api/alumnos", dto);
                if (resultado.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var errores = await resultado.Content.ReadFromJsonAsync<List<string>>();
                    return (false, errores);
                }
            }
            catch (Exception ex)
            {
                return (false, new List<string> { ex.Message });
            }
        }

        public async Task<(bool res, List<string>? errores)> EditarAlumno(EditarAlumnoDTO dto)
        {
            try
            {
                var resultado = await cliente.PutAsJsonAsync("api/alumnos", dto);
                if (resultado.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var errores = await resultado.Content.ReadFromJsonAsync<List<string>>();
                    return (false, errores);
                }
            }
            catch (Exception ex)
            {
                return (false, new List<string> { ex.Message });
            }
        }

        public async Task<(bool, string?)> EliminarAlumno(int idAlumno)
        {
            try
            {
                var resultado = await cliente.DeleteAsync("api/alumnos/" + idAlumno);
                if (resultado.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var error = await resultado.Content.ReadAsStringAsync();
                    return (false, error);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(AlumnoDetallesDTO? alumno, string? error)> VerDetallesAlumno(int idAlumno)
        {
            try
            {
                var resultado = await cliente.GetAsync("api/alumnos/" + idAlumno);
                if (resultado.IsSuccessStatusCode)
                {
                    var alumno = await resultado.Content.ReadFromJsonAsync<AlumnoDetallesDTO>();
                    return (alumno, null);
                }
                else
                {
                    var error = await resultado.Content.ReadAsStringAsync();
                    return (null, error);
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(ClaseDTO? clase, string? errores)> VerClase(int idMaestro)
        {
            try
            {
                var resultado = await cliente.GetAsync("api/clases/" + idMaestro);
                if (resultado.IsSuccessStatusCode)
                {
                    var clase = await resultado.Content.ReadFromJsonAsync<ClaseDTO>();
                    return (clase, null);
                }
                else
                {
                    var error = await resultado.Content.ReadAsStringAsync();
                    return (null, error);
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(bool res, List<string>? errores)> AgregarClase(AgregarClaseDTO dto)
        {
            try
            {
                var resultado = await cliente.PostAsJsonAsync("api/clases", dto);
                if (resultado.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var errores = await resultado.Content.ReadFromJsonAsync<List<string>>();
                    return (false, errores);
                }
            }
            catch (Exception ex)
            {
                return (false, new List<string> { ex.Message });
            }
        }

        public async Task<(bool, List<string>?)> EditarClase(EditarClaseDTO dto)
        {
            try
            {
                var resultado = await cliente.PutAsJsonAsync("api/clases", dto);
                if (resultado.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var errores = await resultado.Content.ReadFromJsonAsync<List<string>>();
                    return (false, errores);
                }
            }
            catch (Exception ex)
            {
                return (false, new List<string> { ex.Message });
            }
        }
        public async Task<(List<AvisoPersonalAlumnoDTO>? avisos, string? error)> VerAvisosPersonalesAlumno(int idAlumno)
        {
            try
            {
                var resultado = await cliente.GetAsync("api/avisos/alumnos/" + idAlumno);
                if (resultado.IsSuccessStatusCode)
                {
                    var avisos = await resultado.Content.ReadFromJsonAsync<List<AvisoPersonalAlumnoDTO>>();
                    return (avisos, null);
                }
                else
                {
                    var error = await resultado.Content.ReadAsStringAsync();
                    return (null, error);
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(AvisoPersonalDetallesDTO? aviso, string? error)> VerDetallesAvisoAlumno(int idAviso)
        {
            try
            {
                var resultado = await cliente.GetAsync($"api/avisos/{idAviso}/alumno");
                if (resultado.IsSuccessStatusCode)
                {
                    var aviso = await resultado.Content.ReadFromJsonAsync<AvisoPersonalDetallesDTO>();
                    return (aviso, null);
                }
                else
                {
                    var error = await resultado.Content.ReadAsStringAsync();
                    return (null, error);
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(AvisoPersonalDetallesDTO? aviso, string? error)> VerDetallesAvisoMaestro(int idAviso)
        {
            try
            {
                var resultado = await cliente.GetAsync($"api/avisos/{idAviso}/maestro");
                if (resultado.IsSuccessStatusCode)
                {
                    var aviso = await resultado.Content.ReadFromJsonAsync<AvisoPersonalDetallesDTO>();
                    return (aviso, null);
                }
                else
                {
                    var error = await resultado.Content.ReadAsStringAsync();
                    return (null, error);
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public async Task<(List<AvisoGeneralDTO>? avisos, string? error)> VerAvisosGeneralesAlumno(int idAlumno)
        {
            try
            {
                var resultado = await cliente.GetAsync($"api/avisos/generales/alumno/" + idAlumno);
                if (resultado.IsSuccessStatusCode)
                {
                    var avisos = await resultado.Content.ReadFromJsonAsync<List<AvisoGeneralDTO>>();
                    return (avisos, null);
                }
                else
                {
                    var error = await resultado.Content.ReadAsStringAsync();
                    return (null, error);
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(List<AvisoGeneralDTO>? avisos, string? error)> VerAvisosGeneralesMaestro(int idMaestro)
        {
            try
            {
                var resultado = await cliente.GetAsync($"api/avisos/generales/maestro/" + idMaestro);
                if (resultado.IsSuccessStatusCode)
                {
                    var avisos = await resultado.Content.ReadFromJsonAsync<List<AvisoGeneralDTO>>();
                    return (avisos, null);
                }
                else
                {
                    var error = await resultado.Content.ReadAsStringAsync();
                    return (null, error);
                }
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(bool res, List<string>? errores)> AgregarAvisoGeneral(AgregarAvisoGeneralDTO dto)
        {
            try
            {
                var resultado = await cliente.PostAsJsonAsync("api/avisos/generales", dto);
                if (resultado.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var errores = await resultado.Content.ReadFromJsonAsync<List<string>>();
                    return (false, errores);
                }
            }
            catch (Exception ex)
            {
                return (false, new List<string> { ex.Message });
            }
        }

        public async Task<(bool, string?)> EliminarAvisoGeneral(int idAviso)
        {
            try
            {
                var resultado = await cliente.DeleteAsync($"api/avisos/generales/" + idAviso);
                if (resultado.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var error = await resultado.Content.ReadAsStringAsync();
                    return (false, error);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool res, List<string>? errores)> AgregarAvisoPersonal(AgregarAvisoPersonalDTO dto)
        {
            try
            {
                var resultado = await cliente.PostAsJsonAsync("api/avisos/alumno", dto);
                if (resultado.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var errores = await resultado.Content.ReadFromJsonAsync<List<string>>();
                    return (false, errores);
                }
            }
            catch (Exception ex)
            {
                return (false, new List<string> { ex.Message });
            }
        }

        public async Task<(bool, string?)> EliminarAvisoPersonal(int idAviso)
        {
            try
            {
                var resultado = await cliente.DeleteAsync($"api/avisos/" + idAviso);
                if (resultado.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var error = await resultado.Content.ReadAsStringAsync();
                    return (false, error);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool, string?)> RecibirNotificacionesAlumno(int idAlumno)
        {
            try
            {
                var resultado = await cliente.GetAsync($"api/avisos/notificaciones/alumno/" + idAlumno);
                if (resultado.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var error = await resultado.Content.ReadAsStringAsync();
                    return (false, error);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool, string?)> RecibirNotificacionesMaestro(int idMaestro)
        {
            try
            {
                var resultado = await cliente.GetAsync($"api/avisos/notificaciones/maestro/" + idMaestro);
                if (resultado.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var error = await resultado.Content.ReadAsStringAsync();
                    return (false, error);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
