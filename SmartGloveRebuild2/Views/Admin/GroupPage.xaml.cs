using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class GroupPage : ContentPage
{
    public GroupPage(GroupViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(DisplayGroupPage));
    }
    //private void Button_Clicked_2(object sender, EventArgs e)
    //{
    //    Shell.Current.GoToAsync(nameof(AssignGroupPage));
    //}
    private void Button_Clicked_3(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(CreateGroupPage));
    }
}