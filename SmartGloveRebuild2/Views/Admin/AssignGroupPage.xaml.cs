using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class AssignGroupPage : ContentPage
{
    public AssignGroupPage(AssignGroupViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }

}