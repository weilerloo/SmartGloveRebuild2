using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveOvertime.Handlers;
using SmartGloveRebuild2.ViewModels.Admin;
using SmartGloveRebuild2.ViewModels.Dashboard;
using SmartGloveRebuild2.Views.Admin;

namespace SmartGloveRebuild2.Views.Dashboard;

public partial class BUHeadDashboardPage : ContentPage
{
    public BUHeadDashboardPage(DashboardPageViewModel dashboardPageViewModel)
    {
        InitializeComponent();
        this.BindingContext = dashboardPageViewModel;

        if (App.UserDetails != null)
        {
            EmployeeName.Text = App.UserDetails.EmployeeName;
            EmployeeNumber.Text = App.UserDetails.EmployeeNumber;
            Plant.Text = App.UserDetails.Plant;
            Department.Text = App.UserDetails.Department;
            Group.Text = App.UserDetails.GroupName;
        }
    }

    //private async void Button_Clicked(object sender, EventArgs e)
    //{
    //    if (IsBusy)
    //        return;
    //    IsBusy = true;
    //    PopupPages p = new PopupPages();
    //    Application.Current.MainPage.ShowPopup(p);
    //    await Shell.Current.GoToAsync(nameof(GenerateReportPage));
    //    p.Close();
    //    IsBusy = false;
    //}

    //private async void Button_Clicked_3(object sender, EventArgs e)
    //{
    //    if (IsBusy)
    //        return;
    //    IsBusy = true;
    //    PopupPages p = new PopupPages();
    //    Application.Current.MainPage.ShowPopup(p);
    //    await Shell.Current.GoToAsync(nameof(CheckCalendarPage));
    //    p.Close();
    //    IsBusy = false;
    //}
}