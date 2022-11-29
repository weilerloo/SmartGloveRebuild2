namespace SmartGloveRebuild2.Controls;

public partial class FlyoutHeaderControl : StackLayout
{
	public FlyoutHeaderControl()
	{
		InitializeComponent();

        if (App.UserDetails != null)
        {
			lblUserName.Text = App.UserDetails.EmployeeName;
            lblUserEmail.Text = App.UserDetails.EmployeeNumber;
            lblUserRole.Text = App.UserDetails.Role;
        }
    }
}