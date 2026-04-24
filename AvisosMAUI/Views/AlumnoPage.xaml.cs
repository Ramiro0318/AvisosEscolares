using AvisosMAUI.ViewModels;

namespace AvisosMAUI.Views;

public partial class AlumnoPage : ContentPage
{
	public AlumnoPage(AvisosViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}