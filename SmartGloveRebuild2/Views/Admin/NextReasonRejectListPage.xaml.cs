#if ANDROID
using SmartGloveRebuild2.Platforms.Android;
# endif
using SmartGloveRebuild2.ViewModels.Admin;

namespace SmartGloveRebuild2.Views.Admin;

public partial class NextReasonRejectListPage : ContentPage
{
	public NextReasonRejectListPage(NextReasonRejectListViewModel viewModel)
	{
		InitializeComponent();
        this.BindingContext = viewModel;
    }

    private void EntryCompleted(object sender, EventArgs e)
    {
#if ANDROID
            KeyboardHelper.HideKeyboard();
#endif
    }
}