using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class UpdateSlotsPage : ContentPage
{
	IScheduleServices scheduleServices;
	public UpdateSlotsPage()
	{
		InitializeComponent();
		this.BindingContext = new UpdateSlotsViewModel(scheduleServices);
	}
}