namespace AvisosMAUI.Views;

using AvisosMAUI.Models;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Layouts;
using System.Linq;

public partial class GrupoPage : ContentPage
{
    public GrupoPage()
    {
        InitializeComponent();

        // Creamos una lista vacía
        var listaDePrueba = new List<Alumno>();

        // Llenamos con 20 nombres genéricos para probar el scroll
        for (int i = 1; i <= 20; i++)
        {
            listaDePrueba.Add(new Alumno
            {
                Nombre = $"Alumno de Prueba #{i}",
                Matricula = $"221G{i:D4}" // Esto genera 0001, 0002, etc.
            });
        }

        // Se los asignamos al CollectionView
        ListaAlumnos.ItemsSource = listaDePrueba;
    }

    private void OnToggleMenuClicked(object sender, EventArgs e)
    {
        MenuDesplegable.IsVisible = !MenuDesplegable.IsVisible;
    }
}