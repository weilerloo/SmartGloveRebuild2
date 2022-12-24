using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class DisplayGroupPage : ContentPage
{
    public DisplayGroupPage(DisplayGroupViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}