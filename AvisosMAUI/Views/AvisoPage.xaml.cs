using AvisosMAUI.ViewModels;

namespace AvisosMAUI.Views;

public partial class AvisoPage : ContentPage
{
	public AvisoPage(AvisosViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}