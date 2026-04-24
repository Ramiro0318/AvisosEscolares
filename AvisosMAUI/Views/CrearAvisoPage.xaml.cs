using AvisosMAUI.ViewModels;

namespace AvisosMAUI.Views;

public partial class CrearAvisoPage : ContentPage
{
	public CrearAvisoPage(AvisosViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}