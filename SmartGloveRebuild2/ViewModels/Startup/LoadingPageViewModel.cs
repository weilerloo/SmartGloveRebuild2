using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SmartGloveRebuild2.Controls;
using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Views.Dashboard;
using SmartGloveRebuild2.Views.Startup;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.ViewModels.Startup
{

    public partial class LoadingPageViewModel
    {
        private readonly IConnectivity _connectivity;

        public LoadingPageViewModel(IConnectivity connectivity)
        {
            _connectivity = connectivity;
            CheckInternetAccess();
            CheckUserLoginDetails();
        }

        private async void CheckUserLoginDetails()
        {
            string userDetailsStr = Preferences.Get(nameof(App.UserDetails), "");

            if (string.IsNullOrWhiteSpace(userDetailsStr))
            {
                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    AppShell.Current.Dispatcher.Dispatch(async () =>
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LoadingPage)}");
                    });
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{nameof(LoadingPage)}");
                }
                // navigate to Login Page
            }
            else
            {
                var tokenDetails = await SecureStorage.GetAsync(nameof(App.Token));

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(tokenDetails) as JwtSecurityToken;

                if (jsonToken.ValidTo < DateTime.UtcNow)
                {
                    await AppShell.Current.DisplayAlert("User session expired", "Login Again To conitnue", "OK");
                    await Shell.Current.GoToAsync($"//{nameof(LoadingPage)}");
                }
                else
                {
                    var userInfo = JsonConvert.DeserializeObject<UserBasicInfo>(userDetailsStr);
                    App.UserDetails = userInfo;
                    App.Token = tokenDetails;
                    await AppConstant.AddFlyoutMenusDetails();
                }
            }
        }

        private async void CheckInternetAccess()
        {
            try
            {
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    if (Preferences.ContainsKey(nameof(App.UserDetails)))
                    {
                        Preferences.Remove(nameof(App.UserDetails));
                    }
                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        AppShell.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Shell.Current.GoToAsync($"//{nameof(LoadingPage)}");
                            await Shell.Current.DisplayAlert("No connectivity!",
                            $"Please check internet and try again.", "OK");
                            return;
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LoadingPage)}");
                        await Shell.Current.DisplayAlert("No connectivity!",
                            $"Please check internet and try again.", "OK");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Internet Error", "Failed to Conenct to Server. ", "OK");
            }
        }
    }
}