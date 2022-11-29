using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class CheckCalendarPage : ContentPage
{
	CheckCalendarViewModel CheckCalendarViewModel;
	public CheckCalendarPage()
	{
		InitializeComponent();
		this.BindingContext = CheckCalendarViewModel = new CheckCalendarViewModel();
	}
}