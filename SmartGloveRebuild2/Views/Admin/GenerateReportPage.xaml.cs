using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class GenerateReportPage : ContentPage
{
    HRGenerateReportViewModel HRGenerateReportViewModel;
	public GenerateReportPage()
	{
		InitializeComponent();
        this.BindingContext = HRGenerateReportViewModel = new HRGenerateReportViewModel();
    }
}