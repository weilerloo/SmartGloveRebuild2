using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.ViewModels;
using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class ExclusionMultipleDatePage : ContentPage
{
    public ExclusionMultipleDatePage(ExclusionMultipleDateViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}