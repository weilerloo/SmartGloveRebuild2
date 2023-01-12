using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.ViewModels.Admin;
using SmartGloveRebuild2.ViewModels.Dashboard;
using SmartGloveRebuild2.Views.Admin;

namespace SmartGloveRebuild2.Views.Dashboard;

public partial class HRDashboardPage : ContentPage
{
    public HRDashboardPage(DashboardPageViewModel dashboardPageViewModel)
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
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(GenerateReportPage));
    }
}