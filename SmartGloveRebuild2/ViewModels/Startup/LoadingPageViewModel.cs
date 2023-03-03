using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SmartGloveOvertime.Handlers;
using SmartGloveRebuild2.Controls;
using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Views.Dashboard;
using SmartGloveRebuild2.Views.Startup;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.ViewModels.Startup
{

    public partial class LoadingPageViewModel : BaseViewModel
    {
        private readonly IConnectivity _connectivity;

        public LoadingPageViewModel(IConnectivity connectivity)
        {
            _connectivity = connectivity;
            CheckInternetAccess();
            CheckUserLoginDetails();
#if ANDROID
            getAppVersion();
#endif
        }

        [RelayCommand]
        public async Task GoToLoginPage()
        {
            if (IsBusy)
            {
                await Shell.Current.DisplayAlert("Messages", "Please check your internet access, or update your version to latest, or check it from Google Play.", "OK");
                return;
            }
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }


        private async void CheckUserLoginDetails()
        {
            if (IsBusy) { return; }
            IsBusy = true;
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
            IsBusy = false;
        }

        private async void CheckInternetAccess()
        {
            if (IsBusy) { return; }
            IsBusy = true;
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
            catch
            {
                await Shell.Current.DisplayAlert("Internet Error", "Failed to Conenct to Server. ", "OK");
            }
            IsBusy = false;
        }
#if ANDROID
        public async Task<string> getPlayStoreVersion()
        {
            var version = await Task.Run(async () =>
            {
                var uri = new Uri($"https://play.google.com/store/apps/details?id={AppInfo.Current.PackageName}&hl=en");
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "text/html");
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                    request.Headers.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
                    using (var response = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        try
                        {
                            response.EnsureSuccessStatusCode();
                            var responseHTML = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var rx = new Regex(@"\[\[\[""\d{1,3}\.\d{1,3}\.{0,1}\d{0,3}""]]", RegexOptions.Compiled);
                            MatchCollection matches = rx.Matches(responseHTML);
                            return matches.Count > 0 ? matches[0].Value : "Unknown";
                        }
                        catch
                        {
                            return "Error";
                        }
                    }
                }
            }
            );
            char[] charsToTrim = { '[', ']' };
            string trimmedversion = version.Trim(charsToTrim);
            string correctversion = trimmedversion.Substring(1, trimmedversion.Length - 2);
            return correctversion;
        }

        public async void getAppVersion()
        {
            if (IsBusy) { return; }
            IsBusy = true;
            var currentPlaystoreversion = await getPlayStoreVersion();
            if (currentPlaystoreversion != null)
            {
                if (AppInfo.Current.Version.ToString() == currentPlaystoreversion)
                {
                    IsBusy = false;
                    return;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Alert", "New Version Detected, Please update to the latest version from Play Store.", "OK");
                    var uri = new Uri($"https://play.google.com/store/apps/details?id={AppInfo.Current.PackageName}&hl=en");
                    await Launcher.OpenAsync(uri);
                }
            }
            IsBusy = false;
        }
#endif
    }
}