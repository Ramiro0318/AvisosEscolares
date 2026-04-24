namespace AvisosMAUI.Views;

using AvisosMAUI.Models;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Layouts;
using System.Linq;
using AvisosMAUI.ViewModels;

public partial class GrupoPage : ContentPage
{
    public GrupoPage(AvisosViewModel vm)
    {
        InitializeComponent();
        this.BindingContext = vm;
    }
    private void OnFondoTapped(object sender, EventArgs e)
    {
        if (MenuDesplegable.IsVisible)
        {
            MenuDesplegable.IsVisible = false;
        }
    }
    private void OnToggleMenuClicked(object sender, EventArgs e)
    {
        MenuDesplegable.IsVisible = !MenuDesplegable.IsVisible;
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}