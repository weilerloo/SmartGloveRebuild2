using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class CheckCalendarPage : ContentPage
{
	public CheckCalendarPage(CheckCalendarViewModel viewmodel)
	{
		InitializeComponent();
		this.BindingContext = viewmodel;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync(nameof(ExclusionMultipleDatePage));
    }
}