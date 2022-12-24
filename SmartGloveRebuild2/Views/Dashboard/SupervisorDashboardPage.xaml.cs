using SmartGloveRebuild2.Views.Admin;

namespace SmartGloveRebuild2.Views.Dashboard;

public partial class SupervisorDashboardPage : ContentPage
{
    public SupervisorDashboardPage()
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
        }

    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(GroupPage));
    }
    private void Button_Clicked_2(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(CheckCalendarPage));
    }
}