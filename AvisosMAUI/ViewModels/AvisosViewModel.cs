using AvisosAPI.Models.DTOs;
using AvisosMAUI.Services;
using AvisosMAUI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvisosMAUI.ViewModels
{
    public class AvisosViewModel : INotifyPropertyChanged
    {
        private readonly AvisosService service;

        public AvisosViewModel(AvisosService service)
        {
            this.service = service;
            CargarClaseCommand = new Command(CargarClase);
            FiltrarCommand = new Command(FiltrarAlumnos);
            VerRegistrarAlumnoCommand = new Command(VerRegistrarAlumno);
            RegistrarAlumnoCommand = new Command(RegistrarAlumno);
            VerDetallesAlumnoCommand = new Command<AlumnoListaDTO>(VerDetallesAlumno);
            VerEditarAlumnoCommand = new Command<AlumnoListaDTO>(VerEditarAlumno);
            EditarAlumnoCommand = new Command(EditarAlumno);
            VerEliminarAlumnoCommand = new Command<AlumnoListaDTO>(VerEliminarAlumno);
            EliminarAlumnoCommand = new Command(EliminarAlumno);
            VerCrearAvisoPersonalCommand = new Command<AlumnoListaDTO>(VerCrearAvisoPersonal);
            CrearAvisoPersonalCommand = new Command(CrearAvisoPersonal);
            VerAvisosGeneralesMaestroCommand = new Command(VerAvisosGeneralesMaestro);
            VerCrearAvisoGeneralCommand = new Command(VerCrearAvisoGeneral);
            CrearAvisoGeneralCommand = new Command(CrearAvisoGeneral);
            VerAvisosPersonalesAlumnoCommand = new Command(VerAvisosPersonalesAlumno);
            VerAvisosGeneralesAlumnoCommand = new Command(VerAvisosGeneralesAlumno);
            VerDetallesAvisoPersonalMaestroCommand = new Command<AvisoPersonalMaestroDTO>(VerDetallesAvisoPersonalMaestro);
            VerDetallesAvisoPersonalAlumnoCommand = new Command<AvisoPersonalAlumnoDTO>(VerDetallesAvisoPersonalAlumno);
            EliminarAvisoPersonalCommand = new Command(EliminarAvisoPersonal);
            EliminarAvisoGeneralCommand = new Command(EliminarAvisoGeneral);
            VerEliminarAvisoGeneralCommand = new Command<AvisoGeneralDTO>(VerEliminarAvisoGeneral);
            VerEliminarAvisoPersonalCommand = new Command<AvisoPersonalMaestroDTO>(VerEliminarAvisoPersonal);
            RegresarCommand = new Command(Regresar);
            CancelarEliminarCommand = new Command(CancelarEliminar);
            LoginCommand = new Command(Login);
            LogoutCommand = new Command(Logout);
        }

        private async void Logout()
        {
            service.Logout();
            LoginModel = new();
            PropertyChanged?.Invoke(this, new(nameof(LoginModel)));
            await Shell.Current.GoToAsync("LoginPage");
        }

        private async void Login()
        {
            Errores = "";
            PropertyChanged?.Invoke(this, new(nameof(Errores)));
            if (LoginModel != null)
            {
                Cargando = true;
                PropertyChanged?.Invoke(this, new(nameof(Cargando)));
                var res = await service.Login(LoginModel);
                if (res.res != null)
                {
                    if (res.res.Rol == "Maestro")
                    {
                        idMaestro = res.res.Id;
                        idClase = res.res.IdClase;
                        NumControl = res.res.NumControl;
                        Correo = res.res.Correo ?? "";
                        await Shell.Current.GoToAsync("GrupoPage");
                        CargarClase();
                    } 
                    if (res.res.Rol == "Alumno")
                    {
                        idAlumno = res.res.Id;
                        NombreAlumno = res.res.Nombre;
                        NumControl = res.res.NumControl;
                        Correo = res.res.Correo ?? "";
                        await Shell.Current.GoToAsync("AvisosAlumnoPage");
                        VerAvisosPersonalesAlumno();
                    }
                }
                else
                {
                    Errores = string.Join(Environment.NewLine, res.errores ?? Enumerable.Empty<string>());
                    PropertyChanged?.Invoke(this, new(nameof(Errores)));
                }

                Cargando = false;
                PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            }
        }

        private void CancelarEliminar()
        {
            Eliminando = false;
            idEliminar = 0;
            NombreEliminacion = "";
            PropertyChanged?.Invoke(this, new(nameof(Eliminando)));
            PropertyChanged?.Invoke(this, new(nameof(NombreEliminacion)));
        }

        private void VerEliminarAvisoPersonal(AvisoPersonalMaestroDTO dTO)
        {
            Eliminando = true;
            idEliminar = dTO.Id;
            NombreEliminacion = dTO.Titulo;
            PropertyChanged?.Invoke(this, new(nameof(Eliminando)));
            PropertyChanged?.Invoke(this, new(nameof(NombreEliminacion)));
        }

        private void VerEliminarAvisoGeneral(AvisoGeneralDTO dTO)
        {
            Eliminando = true;
            idEliminar = dTO.Id;
            NombreEliminacion = dTO.Titulo;
            PropertyChanged?.Invoke(this, new(nameof(Eliminando)));
            PropertyChanged?.Invoke(this, new(nameof(NombreEliminacion)));
        }

        private async void EliminarAvisoGeneral()
        {
            Cargando = true;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            var res = await service.EliminarAvisoGeneral(idEliminar);
            if (res.res)
            {
                var res2 = await service.VerAvisosGeneralesMaestro(idMaestro);
                if (res2.avisos != null)
                {
                    ListaAvisosGenerales.Clear();
                    foreach (var aviso in res2.avisos.OrderByDescending(x => x.FechaCreacion))
                    {
                        ListaAvisosGenerales.Add(aviso);
                    }
                }
                else
                {
                    Errores = res.error ?? "";
                    PropertyChanged?.Invoke(this, new(nameof(Errores)));
                }
            }
            else
            {
                Errores = res.error ?? "";
                PropertyChanged?.Invoke(this, new(nameof(Errores)));
            }
            Cargando = false;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            CancelarEliminar();
            //await Shell.Current.GoToAsync("AvisosGeneralesMaestroPage");
        }

        private async void EliminarAvisoPersonal()
        {
            Cargando = true;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            var res = await service.EliminarAvisoPersonal(idEliminar);
            if (res.res)
            {
                var res2 = await service.VerDetallesAlumno(AlumnoDetallesModel.Id);
                if (res2.alumno != null)
                {
                    AlumnoDetallesModel = res2.alumno;
                    AlumnoDetallesModel.ListaAvisosAlumno = AlumnoDetallesModel.ListaAvisosAlumno.OrderByDescending(x => x.FechaCreacion).ToList();
                    PropertyChanged?.Invoke(this, new(nameof(AlumnoDetallesModel)));
                }
                else
                {
                    Errores = res.error ?? "";
                    PropertyChanged?.Invoke(this, new(nameof(Errores)));
                }
            }
            else
            {
                Errores = res.error ?? "";
                PropertyChanged?.Invoke(this, new(nameof(Errores)));
            }
            Cargando = false;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            CancelarEliminar();
            //await Shell.Current.GoToAsync("AvisosGeneralesMaestroPage");
        }

        private async void Regresar()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void VerDetallesAvisoPersonalAlumno(AvisoPersonalAlumnoDTO dto)
        {
            Cargando = true;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            var res = await service.VerDetallesAvisoAlumno(dto.Id);
            if (res.aviso != null)
            {
                AvisoPersonalDetallesModel = res.aviso;
                PropertyChanged?.Invoke(this, new(nameof(AvisoPersonalDetallesModel)));
            }
            else
            {
                Errores = res.error ?? "";
                PropertyChanged?.Invoke(this, new(nameof(Errores)));
            }
            Cargando = false;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            await Shell.Current.GoToAsync("AvisoDetallesPage");
        }

        private async void VerDetallesAvisoPersonalMaestro(AvisoPersonalMaestroDTO dto)
        {
            Cargando = true;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            var res = await service.VerDetallesAvisoMaestro(dto.Id);
            if (res.aviso != null)
            {
                AvisoPersonalDetallesModel = res.aviso;
                PropertyChanged?.Invoke(this, new(nameof(AvisoPersonalDetallesModel)));
            }
            else
            {
                Errores = res.error ?? "";
                PropertyChanged?.Invoke(this, new(nameof(Errores)));
            }
            Cargando = false;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            await Shell.Current.GoToAsync("AvisoDetallesPage");
        }

        private async void VerAvisosGeneralesAlumno()
        {
            Cargando = true;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            var res = await service.VerAvisosGeneralesAlumno(idAlumno);
            if (res.avisos != null)
            {
                ListaAvisosGenerales.Clear();
                foreach (var aviso in res.avisos.OrderByDescending(x => x.FechaCreacion))
                {
                    ListaAvisosGenerales.Add(aviso);
                }
            }
            else
            {
                Errores = res.error ?? "";
                PropertyChanged?.Invoke(this, new(nameof(Errores)));
            }
            Cargando = false;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            //await Shell.Current.GoToAsync("AvisosGeneralesMaestroPage");
        }

        private async void VerAvisosPersonalesAlumno()
        {
            Cargando = true;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            var res = await service.VerAvisosPersonalesAlumno(idAlumno);
            if (res.avisos != null)
            {
                ListaAvisosPersonalesAlumno.Clear();
                foreach (var aviso in res.avisos.OrderByDescending(x => x.FechaCreacion))
                {
                    ListaAvisosPersonalesAlumno.Add(aviso);
                }
            }
            else
            {
                Errores = res.error ?? "";
                PropertyChanged?.Invoke(this, new(nameof(Errores)));
            }
            Cargando = false;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            //await Shell.Current.GoToAsync("AvisosGeneralesMaestroPage");
        }

        private async void CrearAvisoGeneral()
        {
            if (AgregarAvisoGeneralModel != null)
            {
                Cargando = true;
                PropertyChanged?.Invoke(this, new(nameof(Cargando)));
                var res = await service.AgregarAvisoGeneral(AgregarAvisoGeneralModel);
                if (res.res)
                {
                    CargarClase();
                    await Shell.Current.GoToAsync("GrupoPage");
                }
                else
                {
                    Errores = string.Join(Environment.NewLine, res.errores ?? Enumerable.Empty<string>());
                    PropertyChanged?.Invoke(this, new(nameof(Errores)));
                }

                Cargando = false;
                PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            }
        }

        private async void VerCrearAvisoGeneral()
        {
            AgregarAvisoGeneralModel = new();
            AgregarAvisoGeneralModel.IdMaestro = idMaestro;
            AgregarAvisoGeneralModel.FechaVigencia = DateTime.Now.Date;
            Errores = "";
            PropertyChanged?.Invoke(this, new(nameof(Errores)));
            PropertyChanged?.Invoke(this, new(nameof(AgregarAvisoGeneralModel)));
            await Shell.Current.GoToAsync("AgregarAvisoGeneralPage");
        }

        private async void VerAvisosGeneralesMaestro()
        {
            Cargando = true;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            var res = await service.VerAvisosGeneralesMaestro(idMaestro);
            if (res.avisos != null)
            {
                ListaAvisosGenerales.Clear();
                foreach (var aviso in res.avisos.OrderByDescending(x=>x.FechaCreacion))
                {
                    ListaAvisosGenerales.Add(aviso);
                }
            }
            else
            {
                Errores = res.error ?? "";
                PropertyChanged?.Invoke(this, new(nameof(Errores)));
            }
            Cargando = false;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            await Shell.Current.GoToAsync("AvisosGeneralesMaestroPage");
        }

        private async void EliminarAlumno()
        {
            Cargando = true;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            var res = await service.EliminarAlumno(idEliminar);
            if (res.res)
            {
                CargarClase();
            }
            else
            {
                Errores = res.error ?? "";
                PropertyChanged?.Invoke(this, new(nameof(Errores)));
            }
            Cargando = false;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            CancelarEliminar();
            //await Shell.Current.GoToAsync("AvisosGeneralesMaestroPage");
        }

        private async void VerEliminarAlumno(AlumnoListaDTO dTO)
        {
            Eliminando = true;
            idEliminar = dTO.Id;
            NombreEliminacion = dTO.Nombre;
            PropertyChanged?.Invoke(this, new(nameof(Eliminando)));
            PropertyChanged?.Invoke(this, new(nameof(NombreEliminacion)));
        }

        private async void CrearAvisoPersonal()
        {
            if (AgregarAvisoPersonalModel != null)
            {
                Cargando = true;
                PropertyChanged?.Invoke(this, new(nameof(Cargando)));
                var res = await service.AgregarAvisoPersonal(AgregarAvisoPersonalModel);
                if (res.res)
                {
                    CargarClase();
                    await Shell.Current.GoToAsync("GrupoPage");
                }
                else
                {
                    Errores = string.Join(Environment.NewLine, res.errores ?? Enumerable.Empty<string>());
                    PropertyChanged?.Invoke(this, new(nameof(Errores)));
                }

                Cargando = false;
                PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            }
        }

        private async void VerCrearAvisoPersonal(AlumnoListaDTO dto)
        {
            //Cargando = true;
            //PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            Errores = "";
            NombreDestinatario = dto.Nombre;
            PropertyChanged?.Invoke(this, new(nameof(Errores)));
            PropertyChanged?.Invoke(this, new(nameof(NombreDestinatario)));
            AgregarAvisoPersonalDTO aviso = new AgregarAvisoPersonalDTO
            {
                IdAlumno = dto.Id,
                IdMaestro = idMaestro
            };
            AgregarAvisoPersonalModel = aviso;
            PropertyChanged?.Invoke(this, new(nameof(AgregarAvisoPersonalModel)));
            //Cargando = false;
            //PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            await Shell.Current.GoToAsync("CrearAvisoPersonalPage");
        }

        private async void EditarAlumno()
        {
            if (EditarAlumnoModel != null)
            {
                Cargando = true;
                PropertyChanged?.Invoke(this, new(nameof(Cargando)));
                var res = await service.EditarAlumno(EditarAlumnoModel);
                if (res.res)
                {
                    CargarClase();
                    await Shell.Current.GoToAsync("GrupoPage");
                }
                else
                {
                    Errores = string.Join(Environment.NewLine, res.errores ?? Enumerable.Empty<string>());
                    PropertyChanged?.Invoke(this, new(nameof(Errores)));
                }

                Cargando = false;
                PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            }
        }

        private async void VerEditarAlumno(AlumnoListaDTO dto)
        {
            //Cargando = true;
            //PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            Errores = "";
            PropertyChanged?.Invoke(this, new(nameof(Errores)));
            EditarAlumnoDTO alumno = new EditarAlumnoDTO
            {
                Nombre = dto.Nombre,
                NumControl = dto.NumControl,
                Correo = dto.Correo,
                Contraseña = dto.Contraseña,
                Id = dto.Id,
                IdClase = idClase
            };
            EditarAlumnoModel = alumno;
            PropertyChanged?.Invoke(this, new(nameof(EditarAlumnoModel)));
            //Cargando = false;
            //PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            await Shell.Current.GoToAsync("EditarAlumnoPage");
        }

        private async void VerDetallesAlumno(AlumnoListaDTO dto)
        {
            Cargando = true;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            var res = await service.VerDetallesAlumno(dto.Id);
            if (res.alumno != null)
            {
                AlumnoDetallesModel = res.alumno;
                AlumnoDetallesModel.ListaAvisosAlumno = AlumnoDetallesModel.ListaAvisosAlumno.OrderByDescending(x => x.FechaCreacion).ToList();
                PropertyChanged?.Invoke(this, new(nameof(AlumnoDetallesModel)));
                Cargando = false;
            }
            else
            {
                Errores = res.error ?? "";
                PropertyChanged?.Invoke(this, new(nameof(Errores)));
            }
            Cargando = false;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            await Shell.Current.GoToAsync("AlumnoPage");
        }

        private async void RegistrarAlumno()
        {
            if (AgregarAlumnoModel != null)
            {
                Cargando = true;
                PropertyChanged?.Invoke(this, new(nameof(Cargando)));
                var res = await service.AgregarAlumno(AgregarAlumnoModel);
                if (res.res)
                {
                    CargarClase();
                    await Shell.Current.GoToAsync("GrupoPage");
                }
                else
                {
                    Errores = string.Join(Environment.NewLine, res.errores ?? Enumerable.Empty<string>());
                    PropertyChanged?.Invoke(this, new(nameof(Errores)));
                }

                Cargando = false;
                PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            }
        }

        private async void VerRegistrarAlumno()
        {
            AgregarAlumnoModel = new();
            AgregarAlumnoModel.IdClase = idClase;
            Errores = "";
            PropertyChanged?.Invoke(this, new(nameof(Errores)));
            PropertyChanged?.Invoke(this, new(nameof(AgregarAlumnoModel)));
            await Shell.Current.GoToAsync("RegistroPage");
        }

        //PROPIEDADES GENERALES APP

        public bool Cargando { get; set; } = false;
        public int idClase { get; set; }
        public int idMaestro { get; set; }
        public string NombreClase { get; set; } = "";
        public string Errores { get; set; } = "";
        //string busqueda = "";
        //public string Busqueda
        //{
        //    get => busqueda;
        //    set
        //    {
        //        if (busqueda != value)
        //        {
        //            busqueda = value;
        //            PropertyChanged?.Invoke(this, new(nameof(Busqueda)));
        //            FiltrarAlumnos();
        //        }
        //    }
        //}
        public string Busqueda { get; set; } = "";
        public Command FiltrarCommand { get; set; }
        //public ObservableCollection<AlumnoListaDTO> ListaAlumnos { get; set; } = new();
        public ObservableCollection<AlumnoListaDTO> ListaAlumnos { get; set; } = new();
        public List<AlumnoListaDTO> alumnos { get; set; } = new();
        public ObservableCollection<AvisoGeneralDTO> ListaAvisosGenerales { get; set; } = new();
        public bool Eliminando { get; set; }
        public AvisoPersonalDetallesDTO AvisoPersonalDetallesModel { get; set; } = new();
        public ICommand RegresarCommand { get; set; }
        //public ICommand VerEliminarCommand { get; set; }
        public ICommand CancelarEliminarCommand { get; set; }

        //LOGIN COMANDOS
        public ICommand LoginCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        //LOGIN PROPIEDADES
        public LoginDTO LoginModel { get; set; } = new();


        //COMANDOS MAESTRO

        public ICommand CargarClaseCommand { get; set; }
        public ICommand VerRegistrarAlumnoCommand { get; set; }
        public ICommand RegistrarAlumnoCommand { get; set; }
        public ICommand VerDetallesAlumnoCommand { get; set; }
        public ICommand VerEditarAlumnoCommand { get; set; }
        public ICommand EditarAlumnoCommand { get; set; }
        public ICommand VerEliminarAlumnoCommand { get; set; }
        public ICommand EliminarAlumnoCommand { get; set; }
        public ICommand VerCrearAvisoPersonalCommand { get; set; }
        public ICommand CrearAvisoPersonalCommand { get; set; }
        public ICommand VerAvisosGeneralesMaestroCommand { get; set; }
        public ICommand VerCrearAvisoGeneralCommand { get; set; }
        public ICommand CrearAvisoGeneralCommand { get; set; }
        public ICommand VerDetallesAvisoPersonalMaestroCommand { get; set; }
        public ICommand VerEliminarAvisoPersonalCommand { get; set;}
        public ICommand VerEliminarAvisoGeneralCommand { get; set; }
        public ICommand EliminarAvisoGeneralCommand { get; set; }
        public ICommand EliminarAvisoPersonalCommand { get; set; }


        //PROPIEDADES MAESTRO

        public AgregarAlumnoDTO AgregarAlumnoModel { get; set; } = new();
        public EditarAlumnoDTO EditarAlumnoModel { get; set; } = new();
        public AlumnoDetallesDTO AlumnoDetallesModel { get; set; } = new();
        public AgregarAvisoPersonalDTO AgregarAvisoPersonalModel { get; set; } = new();
        public AgregarAvisoGeneralDTO AgregarAvisoGeneralModel { get; set; } = new();
        int idEliminar;
        public string NombreDestinatario { get; set; } = "";
        public string NombreEliminacion { get; set; } = "";


        // COMANDOS ALUMNO

        public ICommand VerAvisosPersonalesAlumnoCommand { get; set; }
        public ICommand VerAvisosGeneralesAlumnoCommand { get; set; }
        public ICommand VerDetallesAvisoPersonalAlumnoCommand { get; set; }


        // PROPIEDADES ALUMNO
        private int idAlumno;
        public ObservableCollection<AvisoPersonalAlumnoDTO> ListaAvisosPersonalesAlumno { get; set; } = new();
        public string NombreAlumno { get; set; } = "";
        public string NumControl { get; set; } = "";
        public string Correo { get; set; } = "";

        private void FiltrarAlumnos()
        {
            var texto = Busqueda?.ToLower() ?? "";

            ListaAlumnos.Clear();

            var filtrados = string.IsNullOrWhiteSpace(texto)
                ? alumnos
                : alumnos.Where(a =>
                    (a.Nombre?.ToLower().Contains(texto) ?? false) ||
                    (a.NumControl?.ToLower().Contains(texto) ?? false));

            foreach (var alumno in filtrados)
            {
                ListaAlumnos.Add(alumno);
            }
        }
        public async void CargarClase()
        {
            Cargando = true;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
            var res = await service.VerClase(idMaestro);
            if(res.clase != null)
            {
                NombreClase = res.clase.Nombre;
                idClase = res.clase.Id;
                ListaAlumnos.Clear();
                foreach (var alumno in res.clase.ListaAlumnos)
                {
                    ListaAlumnos.Add(alumno);
                }
                alumnos = res.clase.ListaAlumnos;
                PropertyChanged?.Invoke(this, new(nameof(NombreClase)));
                Cargando = false;
            }
            else
            {
                AgregarClaseDTO nuevaClase = new()
                {
                    IdMaestro = idMaestro,
                    Nombre = "Clase"
                };
                var result = await service.AgregarClase(nuevaClase);   
                if (result.res)
                {
                    var res2 = await service.VerClase(idMaestro);
                    if (res2.clase != null)
                    {
                        NombreClase = res2.clase.Nombre;
                        idClase = res2.clase.Id;
                        ListaAlumnos.Clear();
                        foreach (var alumno in res2.clase.ListaAlumnos)
                        {
                            ListaAlumnos.Add(alumno);
                        }
                        alumnos = res2.clase.ListaAlumnos;
                        PropertyChanged?.Invoke(this, new(nameof(NombreClase)));
                    }
                }
                else
                {
                    Errores = string.Join(Environment.NewLine, result.errores ?? Enumerable.Empty<string>());
                    PropertyChanged?.Invoke(this, new(nameof(Errores)));
                }
            }
            Cargando = false;
            PropertyChanged?.Invoke(this, new(nameof(Cargando)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
