using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SmartGloveRebuild2.Controls;
using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.Views.Admin;
using SmartGloveRebuild2.Views.Employee;
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
        [ObservableProperty]
        string empnum, empnam, grpname, plt, depart, payr;

        [ObservableProperty]
        int ttdayofot;

        [ObservableProperty]
        double tthoursofot;

        private readonly ILoginService _loginService;
        public DashboardPageViewModel(ILoginService loginServices)
        {
            _loginService = loginServices;
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
            GetUserBasicInfo();
            CheckUserActivity();
            LoopCheckActivity();
        }

        public async void GetUserBasicInfo()
        {
            IsBusy = true;
            if (App.UserDetails.Passport == "")
            {
                var checktoken = await _loginService.GetUserBasicInfo(new LoginRequest
                {
                    UserName = App.UserDetails.EmployeeNumber,
                    Password = App.UserDetails.NRIC,
                });

                if (checktoken != null)
                {
                    string userDetailStr = JsonConvert.SerializeObject(checktoken);
                    Preferences.Set(nameof(App.UserDetails), userDetailStr);
                    App.UserDetails = checktoken;
                    Empnum = App.UserDetails.EmployeeNumber;
                    Empnam = App.UserDetails.EmployeeName;
                    Grpname = App.UserDetails.GroupName;
                    Plt = App.UserDetails.Plant;
                    Depart = App.UserDetails.Department;
                    Payr = App.UserDetails.Payroll;
                    Ttdayofot = App.UserDetails.TotalOTDay;
                    Tthoursofot = App.UserDetails.TotalHour;
                }
            }
            else
            {
                var checktoken = await _loginService.GetUserBasicInfo(new LoginRequest
                {
                    UserName = App.UserDetails.EmployeeNumber,
                    Password = App.UserDetails.Passport,
                });
                if (checktoken != null)
                {
                    string userDetailStr = JsonConvert.SerializeObject(checktoken);
                    Preferences.Set(nameof(App.UserDetails), userDetailStr);
                    App.UserDetails = checktoken;
                    Empnum = App.UserDetails.EmployeeNumber;
                    Empnam = App.UserDetails.EmployeeName;
                    Grpname = App.UserDetails.GroupName;
                    Plt = App.UserDetails.Plant;
                    Depart = App.UserDetails.Department;
                    Payr = App.UserDetails.Payroll;
                    Ttdayofot = App.UserDetails.TotalOTDay;
                    Tthoursofot = App.UserDetails.TotalHour;
                }
            }
            IsBusy = false;
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
                    GetUserBasicInfo();
                }
            }
        }
    }
}
