using CommunityToolkit.Mvvm.ComponentModel;
using SmartGloveRebuild2.Models;

namespace SmartGloveRebuild2.Controls;

public partial class FlyoutHeaderControl : StackLayout
{
    string imagesourceflyoutcontrol;
    public FlyoutHeaderControl()
    {
        InitializeComponent();

        if (App.UserDetails != null)
        {
            lblUserName.Text = App.UserDetails.EmployeeName.ToUpper();
            lblUserEmail.Text = App.UserDetails.EmployeeNumber;
            lblUserRole.Text = App.UserDetails.Role;
        }



        if (App.UserDetails.RoleID == (int)RoleDetails.Employee)
        {
            imagesourceflyoutcontrol = "employeeflyout.png";
        }
        else
        {
            imagesourceflyoutcontrol = "adminflyout.png";
        }

        LoadImage.Source = imagesourceflyoutcontrol;
    }
}