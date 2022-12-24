using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class CreateGroupPage : ContentPage
{
	public CreateGroupPage(CreateGroupViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}