using SmartGloveRebuild2.ViewModels;
using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Employee;

public partial class ScheduleOT : ContentPage
{
	ScheduleViewModel ScheduleViewModel;
	public ScheduleOT(ScheduleViewModel viewModel)
	{
		InitializeComponent();
		//BindingContext = viewModel;
		this.BindingContext = ScheduleViewModel = new ScheduleViewModel();

	}

}