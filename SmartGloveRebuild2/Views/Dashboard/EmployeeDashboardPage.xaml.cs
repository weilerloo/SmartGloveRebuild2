using SmartGloveRebuild2.ViewModels.Dashboard;
using SmartGloveRebuild2.Views.Employee;

namespace SmartGloveRebuild2.Views.Dashboard;

public partial class EmployeeDashboardPage : ContentPage
{
	public EmployeeDashboardPage(DashboardPageViewModel dashboardPageViewModel)
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

    private void ScheduleButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(ScheduleOT));
    }
}