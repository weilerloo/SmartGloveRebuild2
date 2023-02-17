using CommunityToolkit.Maui.Views;
using SmartGloveOvertime.Handlers;
using SmartGloveRebuild2.ViewModels.Dashboard;
using SmartGloveRebuild2.Views.Admin;
using SmartGloveRebuild2.Views.Employee;

namespace SmartGloveRebuild2.Views.Dashboard;

public partial class ClerkDashboardPage : ContentPage
{

    public ClerkDashboardPage(DashboardPageViewModel dashboardPageViewModel)
    {
        InitializeComponent();
        this.BindingContext = dashboardPageViewModel;

        if (App.UserDetails != null)
        {
            EmployeeName.Text = App.UserDetails.EmployeeName;
            EmployeeNumber.Text = App.UserDetails.EmployeeNumber;
            Plant.Text = App.UserDetails.Plant;
            Department.Text = App.UserDetails.Department;
            Payroll.Text = App.UserDetails.Payroll;
            TotalDayOT.Text = App.UserDetails.TotalOTDay.ToString();
            TotalHour.Text = App.UserDetails.TotalHour.ToString();
            Group.Text = App.UserDetails.GroupName;
        }
    }
    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        IsBusy = true;
        PopupPages p = new PopupPages();
        Application.Current.MainPage.ShowPopup(p);
        await Task.Delay(100);
        await Shell.Current.GoToAsync(nameof(UpdateSlotsPage)); 
        p.Close();
        IsBusy = false;
    }
    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        IsBusy = true;
        PopupPages p = new PopupPages();
        Application.Current.MainPage.ShowPopup(p);
        await Task.Delay(100);
        await Shell.Current.GoToAsync(nameof(DisplayGroupPage)); 
        p.Close();
        IsBusy = false;
    }
    private async void Button_Clicked_3(object sender, EventArgs e)
    {
        IsBusy = true;
        PopupPages p = new PopupPages();
        Application.Current.MainPage.ShowPopup(p);
        await Task.Delay(100);
        await Shell.Current.GoToAsync(nameof(ScheduleOT));
        p.Close();
        IsBusy = false;
    }    
    
    private async void Button_Clicked_4(object sender, EventArgs e)
    {
        PopupPages p = new PopupPages();
        Application.Current.MainPage.ShowPopup(p);
        await Task.Delay(100);

    }
}