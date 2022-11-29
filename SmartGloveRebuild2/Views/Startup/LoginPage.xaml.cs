using SmartGloveRebuild2.ViewModels.Startup;

namespace SmartGloveRebuild2.Views.Startup;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}