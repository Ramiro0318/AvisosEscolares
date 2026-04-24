using AvisosMAUI.ViewModels;

namespace AvisosMAUI.Views;

public partial class EditarAlumnoPage : ContentPage
{
	public EditarAlumnoPage(AvisosViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}