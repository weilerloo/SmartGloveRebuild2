using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.ViewModels.Employee;

namespace SmartGloveRebuild2.Views.Employee;

public partial class RejectedOT : ContentPage
{
	public RejectedOT(RejectViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
    }
}