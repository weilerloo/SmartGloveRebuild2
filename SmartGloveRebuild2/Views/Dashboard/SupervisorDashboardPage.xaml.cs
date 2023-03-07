using SmartGloveOvertime.Handlers;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using SmartGloveRebuild2.ViewModels.Dashboard;
using SmartGloveRebuild2.Views.Admin;
using SmartGloveRebuild2.Views.Employee;

namespace SmartGloveRebuild2.Views.Dashboard;

public partial class SupervisorDashboardPage : ContentPage
{
    public SupervisorDashboardPage(DashboardPageViewModel dashboardPageViewModel)
    {
        InitializeComponent();
        this.BindingContext = dashboardPageViewModel;

    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        if (IsBusy)
            return;
        IsBusy = true;
        PopupPages p = new PopupPages();
        Application.Current.MainPage.ShowPopup(p);
        await Task.Delay(100);
        await Shell.Current.GoToAsync(nameof(ScheduleOT));
        p.Close();
        IsBusy = false;
    }
    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        if (IsBusy)
            return;
        IsBusy = true;
        PopupPages p = new PopupPages();
        Application.Current.MainPage.ShowPopup(p);
        await Task.Delay(100);
        await Shell.Current.GoToAsync(nameof(GroupPage));
        p.Close();
        IsBusy = false;
    }
    private async void Button_Clicked_3(object sender, EventArgs e)
    {
        if (IsBusy)  
            return;
        IsBusy = true;
        PopupPages p = new PopupPages();
        Application.Current.MainPage.ShowPopup(p);
        await Task.Delay(100);
        await Shell.Current.GoToAsync(nameof(CheckCalendarPage));
        p.Close();
        IsBusy = false;
    }
}