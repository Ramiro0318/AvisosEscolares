using AvisosMAUI.ViewModels;

namespace AvisosMAUI.Views;

public partial class MainPage : ContentPage
{

    public MainPage(AvisosViewModel vm)
    {
        InitializeComponent();
        this.BindingContext = vm;
    }

}
