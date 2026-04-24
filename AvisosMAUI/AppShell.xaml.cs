using AvisosMAUI.Views;

namespace AvisosMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("GrupoPage", typeof(GrupoPage));
            Routing.RegisterRoute("RegistroPage", typeof(RegistroPage));
            Routing.RegisterRoute("AlumnoPage", typeof(AlumnoPage));
            Routing.RegisterRoute("EditarAlumnoPage", typeof(EditarAlumnoPage));
            Routing.RegisterRoute("CrearAvisoPersonalPage", typeof(CrearAvisoPage));
            Routing.RegisterRoute("AvisosGeneralesMaestroPage", typeof(VerAvisosGeneralesPage));
            Routing.RegisterRoute("AgregarAvisoGeneralPage", typeof(CrearAvisoGeneralPage));
            Routing.RegisterRoute("AvisosAlumnoPage", typeof(AvisosPage));
            Routing.RegisterRoute("AvisoDetallesPage", typeof(AvisoPage));
        }
    }
}
