using CommunityToolkit.Mvvm.ComponentModel;
using SmartGloveRebuild2.Controls;
using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.Views.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SmartGloveRebuild2.ViewModels.Dashboard
{
    public partial class DashboardPageViewModel : BaseViewModel
    {
        private readonly ILoginService _loginService;

        public DashboardPageViewModel(ILoginService loginServices)
        {
            _loginService = loginServices;
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
            CheckUserActivity();
            LoopCheckActivity();
        }

        public async void CheckUserActivity()
        {
            IsBusy = true;
            if (App.UserDetails.Passport == "")
            {
                var checktoken = await _loginService.CheckRefreshToken(new LoginRequest
                {
                    UserName = App.UserDetails.EmployeeNumber,
                    Password = App.UserDetails.NRIC,
                });

                if (checktoken != null)
                {
                    if (checktoken != App.UserDetails.RefreshToken)
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
                            });
                        }
                        else
                        {
                            await Shell.Current.GoToAsync($"//{nameof(LoadingPage)}");
                        }
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Internet Error", "You have been logout, please login again.", "OK");
                    if (Preferences.ContainsKey(nameof(App.UserDetails)))
                    {
                        Preferences.Remove(nameof(App.UserDetails));
                    }
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
                    return;
                }

            }
            else
            {
                var checktoken = await _loginService.CheckRefreshToken(new LoginRequest
                {
                    UserName = App.UserDetails.EmployeeNumber,
                    Password = App.UserDetails.Passport,
                });
                if (checktoken != null)
                {

                    if (checktoken != App.UserDetails.RefreshToken)
                    {
                        await Shell.Current.DisplayAlert("Messages", "Session Expired, Login from other devices Detected", "OK");
                        if (Preferences.ContainsKey(nameof(App.UserDetails)))
                        {
                            Preferences.Remove(nameof(App.UserDetails));
                        }
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
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Internet Error", "Failed to connect to the server, you will be logout.", "OK");
                    if (Preferences.ContainsKey(nameof(App.UserDetails)))
                    {
                        Preferences.Remove(nameof(App.UserDetails));
                    }
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
                }
            }
            IsBusy = false;
        }

        public async void LoopCheckActivity()
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));

            while (await timer.WaitForNextTickAsync())
            {
                if (Preferences.ContainsKey(nameof(App.UserDetails)))
                {
                    CheckUserActivity();
                }
            }
        }
    }
}
