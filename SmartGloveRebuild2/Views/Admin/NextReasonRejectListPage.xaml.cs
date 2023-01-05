using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class NextReasonRejectListPage : ContentPage
{
	public NextReasonRejectListPage(NextReasonRejectListViewModel viewModel)
	{
		InitializeComponent();
        this.BindingContext = viewModel;
    }
}