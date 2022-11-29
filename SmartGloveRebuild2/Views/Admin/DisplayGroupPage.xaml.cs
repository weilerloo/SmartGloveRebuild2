using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class DisplayGroupPage : ContentPage
{
	DisplayGroupViewModel DisplayGroupViewModel;
	public DisplayGroupPage()
	{
		InitializeComponent();
		this.BindingContext = DisplayGroupViewModel = new DisplayGroupViewModel();
	}
}