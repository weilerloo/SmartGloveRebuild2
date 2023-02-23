using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class GenerateReportPage : ContentPage
{
	public GenerateReportPage(HRGenerateReportViewModel hRGenerateReportViewModel)
	{
		InitializeComponent();
        this.BindingContext = hRGenerateReportViewModel;

        //if (App.UserDetails.Role == "EXECUTIVE")
        //{
        //    EmployeePicker.IsVisible = false;
        //}
    }
}