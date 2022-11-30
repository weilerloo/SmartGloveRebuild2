using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.Views.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.ViewModels.Startup
{
    public partial class SignInViewModel : BaseViewModel
    {
        public SignInViewModel()
        {
            GoToSignInPage();
        }

        [RelayCommand]
        private async void GoToSignInPage()
        {
            await Shell.Current.GoToAsync(nameof(SignInPage));
        }
    }
}
