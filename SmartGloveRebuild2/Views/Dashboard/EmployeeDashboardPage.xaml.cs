using CommunityToolkit.Maui.Views;
using SmartGloveOvertime.Handlers;
using SmartGloveRebuild2.ViewModels.Dashboard;
using SmartGloveRebuild2.Views.Employee;

namespace SmartGloveRebuild2.Views.Dashboard;

public partial class EmployeeDashboardPage : ContentPage
{
	public EmployeeDashboardPage(DashboardPageViewModel dashboardPageViewModel)
	{
		InitializeComponent();
        this.BindingContext = dashboardPageViewModel;

    }

    private async void ScheduleButton_Clicked(object sender, EventArgs e)
    {
        IsBusy = true;
        PopupPages p = new PopupPages();
        Application.Current.MainPage.ShowPopup(p);
        await Task.Delay(100);
        await Shell.Current.GoToAsync(nameof(ScheduleOT));
        p.Close();
        IsBusy = false;
    }
}