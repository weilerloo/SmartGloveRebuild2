using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class UpdateSlotsPage : ContentPage
{
	public UpdateSlotsPage(UpdateSlotsViewModel updateSlotsViewModel)
	{
		InitializeComponent();
		this.BindingContext = updateSlotsViewModel;
    }
}