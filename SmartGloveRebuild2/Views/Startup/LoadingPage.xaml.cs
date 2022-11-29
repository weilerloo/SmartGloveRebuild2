using SmartGloveRebuild2.ViewModels.Startup;

namespace SmartGloveRebuild2.Views.Startup;

public partial class LoadingPage : ContentPage
{
	public LoadingPage(LoadingPageViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}