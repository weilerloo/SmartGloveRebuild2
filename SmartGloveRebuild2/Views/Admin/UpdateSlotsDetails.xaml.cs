#if ANDROID
using SmartGloveRebuild2.Platforms.Android;
#endif
using SmartGloveRebuild2.Models.ClerkDTO;
using SmartGloveRebuild2.ViewModels.Admin;
using System.Collections.ObjectModel;

namespace SmartGloveRebuild2.Views.Admin;

public partial class UpdateSlotsDetails : ContentPage
{
    public UpdateSlotsDetails(UpdateSlotsDetailViewModel updateSlotsDetailViewModel)
    {
        InitializeComponent();
        this.BindingContext = updateSlotsDetailViewModel;
    }

    private void Paxs_Completed(object sender, EventArgs e)
    {
#if ANDROID
            KeyboardHelper.HideKeyboard();
#endif
    }
}