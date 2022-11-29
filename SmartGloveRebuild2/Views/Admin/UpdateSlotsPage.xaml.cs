using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class UpdateSlotsPage : ContentPage
{
	UpdateSlotsViewModel UpdateSlotsViewModel;
	public UpdateSlotsPage()
	{
		InitializeComponent();
		this.BindingContext = UpdateSlotsViewModel = new UpdateSlotsViewModel();
	}
}