namespace AvisosMAUI.Views;

public partial class AvisosPage : ContentPage
{
	public AvisosPage()
	{
		InitializeComponent();
	}

    private void OnTabClicked(object sender, EventArgs e)
    {
        var layout = sender as Grid;

        if (layout == BtnPersonales)
        {
            BtnPersonales.BackgroundColor = Color.FromArgb("#26A69A");
            ViewPersonales.IsVisible = true;
            LblPersonales.TextColor = Colors.White;

            BtnGenerales.BackgroundColor = Colors.Transparent;
            ViewGenerales.IsVisible = false;
            LblGenerales.TextColor = Colors.Gray;

        }
        else
        {
            BtnGenerales.BackgroundColor = Color.FromArgb("#FFA69A");
            LblGenerales.TextColor = Colors.White;
            ViewGenerales.IsVisible = true;

            BtnPersonales.BackgroundColor = Colors.Transparent;
            LblPersonales.TextColor = Colors.Gray;
            ViewPersonales.IsVisible = false;

        }
    }
    private void OnToggleMenuClicked(object sender, EventArgs e)
    {
        MenuDesplegable.IsVisible = !MenuDesplegable.IsVisible;
    }
}