using AvisosMAUI.ViewModels;

namespace AvisosMAUI.Views;

public partial class VerAvisosGeneralesPage : ContentPage
{
	public VerAvisosGeneralesPage(AvisosViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}