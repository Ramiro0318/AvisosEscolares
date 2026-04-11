namespace AvisosMAUI.Views;

public partial class AvisosPage : ContentPage
{
	public AvisosPage()
	{
		InitializeComponent();
	}

    private void OnTabClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button == BtnGenerales)
        {
            // Activar visualmente Generales
            BtnGenerales.BackgroundColor = Color.FromArgb("#26A69A");
            BtnGenerales.TextColor = Colors.White;
            ViewGenerales.IsVisible = true;

            // Desactivar Personales
            BtnPersonales.BackgroundColor = Colors.Transparent;
            BtnPersonales.TextColor = Colors.Gray;
            ViewPersonales.IsVisible = false;
        }
        else
        {
            // Activar visualmente Personales
            BtnPersonales.BackgroundColor = Color.FromArgb("#26A69A");
            BtnPersonales.TextColor = Colors.White;
            ViewPersonales.IsVisible = true;

            // Desactivar Generales
            BtnGenerales.BackgroundColor = Colors.Transparent;
            BtnGenerales.TextColor = Colors.Gray;
            ViewGenerales.IsVisible = false;
        }
    }
}