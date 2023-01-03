using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class AdjustSlotsPage : ContentPage
{
    public AdjustSlotsPage()
    {
        InitializeComponent();
    }
    private void Button_Clicked_1(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(ExclusionDayPage));
    }
    private void Button_Clicked_2(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(ExclusionMultipleDayPage));
    }
}