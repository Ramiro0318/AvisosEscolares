namespace AvisosMAUI.Views;
using Microsoft.Maui.Platform;

public partial class GrupoPage : ContentPage
{
	public GrupoPage()
	{
		InitializeComponent();
    }

    private void OnShowCustomMenu(object sender, EventArgs e)
    {
        var view = sender as View;
        if (view == null) return;

        double totalX = 0;
        double totalY = 0;

        var current = view;

        // Mientras el elemento actual tenga un padre, seguimos sumando
        while (current != null)
        {
            totalX += current.X;
            totalY += current.Y;

            // Intentamos subir al siguiente nivel
            var parent = current.Parent as View;

            // Si el padre ya no es una View (es la página o nulo), salimos
            if (parent == null) break;

            current = parent;
        }

        // Obtenemos cuánto ha bajado el scroll actualmente
        double desplazamiento = MiScroll.ScrollY;

        // Aplicamos la posición
        MiMenuFlotante.TranslationX = totalX - 120;
        MiMenuFlotante.TranslationY = totalY - 85 - desplazamiento;

        MiMenuFlotante.IsVisible = true;
        Escudo.IsVisible = true;
    }

    private void OnCloseCustomMenu(object sender, EventArgs e)
    {
        MiMenuFlotante.IsVisible = false;
        Escudo.IsVisible = false;
    }


}