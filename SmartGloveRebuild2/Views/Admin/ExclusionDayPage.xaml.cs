using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class ExclusionDayPage : ContentPage
{
	public ExclusionDayPage(ExclusionDayViewModel viewModel)
	{
		InitializeComponent();
        this.BindingContext = viewModel;
    }
}