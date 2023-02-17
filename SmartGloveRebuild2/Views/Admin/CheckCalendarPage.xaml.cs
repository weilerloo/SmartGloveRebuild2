using CommunityToolkit.Maui.Views;
using SmartGloveOvertime.Handlers;
using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class CheckCalendarPage : ContentPage
{
	public CheckCalendarPage(CheckCalendarViewModel viewmodel)
	{
		InitializeComponent();
		this.BindingContext = viewmodel;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        IsBusy = true;
        PopupPages p = new PopupPages();
        Application.Current.MainPage.ShowPopup(p);
        await Shell.Current.GoToAsync(nameof(ExclusionMultipleDatePage));
        p.Close();
        IsBusy = false;
    }
}