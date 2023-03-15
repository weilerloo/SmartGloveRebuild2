using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using SmartGloveRebuild2.ViewModels.Startup;
using Application = Microsoft.Maui.Controls.Application;

namespace SmartGloveRebuild2.Views.Startup;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        App.Current.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
        Console.WriteLine("OnAppearing");
    }    
    
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        App.Current.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
        Console.WriteLine("OnDisAppearing");
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