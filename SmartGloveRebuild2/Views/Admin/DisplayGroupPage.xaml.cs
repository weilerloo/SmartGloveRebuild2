using SmartGloveRebuild2.ViewModels.Admin;
using SmartGloveRebuild2.Views.Dashboard;

namespace SmartGloveRebuild2.Views.Admin;

public partial class DisplayGroupPage : ContentPage
{
    public DisplayGroupPage(DisplayGroupViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}