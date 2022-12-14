using SmartGloveRebuild2.ViewModels.Startup;

namespace SmartGloveRebuild2.Views.Startup;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;

    }

    protected override bool OnBackButtonPressed()
    {
        return base.OnBackButtonPressed();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new AppShell();
    }
}