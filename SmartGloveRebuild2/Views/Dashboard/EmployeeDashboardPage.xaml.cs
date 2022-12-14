namespace SmartGloveRebuild2.Views.Dashboard;

public partial class EmployeeDashboardPage : ContentPage
{
	public EmployeeDashboardPage()
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
}