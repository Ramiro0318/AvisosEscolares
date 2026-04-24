using AvisosMAUI.ViewModels;

namespace AvisosMAUI.Views;

public partial class CrearAvisoGeneralPage : ContentPage
{
	public CrearAvisoGeneralPage(AvisosViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}