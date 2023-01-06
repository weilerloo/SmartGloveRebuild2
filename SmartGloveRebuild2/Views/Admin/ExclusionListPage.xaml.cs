using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class ExclusionListPage : ContentPage
{
    public ExclusionListPage(ExclusionListViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }

}