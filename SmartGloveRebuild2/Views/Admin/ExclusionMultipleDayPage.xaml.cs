using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class ExclusionMultipleDayPage : ContentPage
{
    public ExclusionMultipleDayPage(ExclusionMultipleDayViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}