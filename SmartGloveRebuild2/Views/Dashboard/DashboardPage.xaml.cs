
using SmartGloveRebuild2.ViewModels.Dashboard;

namespace SmartGloveRebuild2.Views.Dashboard;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}