using SmartGloveRebuild2.Views.Admin;
using SmartGloveRebuild2.Views.Employee;

namespace SmartGloveRebuild2.Views.Dashboard;

public partial class HODDashboardPage : ContentPage
{
    public HODDashboardPage()
    {
        InitializeComponent();

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
        await Shell.Current.GoToAsync(nameof(ScheduleOT));
    }
    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(GroupPage));
    }
    private async void Button_Clicked_3(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CheckCalendarPage));
    }
}