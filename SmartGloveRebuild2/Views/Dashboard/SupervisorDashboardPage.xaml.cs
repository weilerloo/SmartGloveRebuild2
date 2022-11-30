namespace SmartGloveRebuild2.Views.Dashboard;

public partial class SupervisorDashboardPage : ContentPage
{
	public SupervisorDashboardPage()
	{
		InitializeComponent();

        if (App.UserDetails != null)
        {
            EmployeeNumber.Text = App.UserDetails.EmployeeNumber;
            //Plant.Text = App.UserDetails.Plant;
            DepartmentCode.Text = App.UserDetails.DepartmentCode;
            //Payroll.Text = App.UserDetails.Payroll;
            //TotalOTDay.Text = App.UserDetails.TotalDayOT;
            //TotalHour.Text = App.UserDetails.TotalHour;
        }
    }
}