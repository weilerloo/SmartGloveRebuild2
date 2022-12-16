using SmartGloveRebuild2.Models.ClerkDTO;
using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class UpdateSlotsDetails : ContentPage
{
    UpdateSlotsDetailViewModel updateSlotsDetailViewModel;

    public UpdateSlotsDetails(UpdateSlotsDetailViewModel updateSlotsDetailViewModel)
	{
		InitializeComponent();
		this.BindingContext = updateSlotsDetailViewModel;
    }
}