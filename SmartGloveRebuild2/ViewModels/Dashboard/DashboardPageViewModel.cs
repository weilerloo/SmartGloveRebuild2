using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
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
using System.Text.RegularExpressions;
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
        private readonly INotificationService _notificationServices;
        private readonly IScheduleServices _scheduleServices;

#if ANDROID
        public DashboardPageViewModel(ILoginService loginServices, INotificationService notificationServices, IScheduleServices scheduleServices)
        {
            _loginService = loginServices;
            _notificationServices = notificationServices;
            _scheduleServices = scheduleServices;
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
            GetUserBasicInfo();
            CheckUserActivity();
            LoopCheckActivity();
            getAppVersion();
            getNotification();
            if (App.UserDetails.Role == "ADMIN1" || App.UserDetails.Role == "ADMIN2" )
            {
                Title = "Admin Dashboard";
            }
            else if(App.UserDetails.Role == "CLERK")
            {
                Title = "Clerk Dashboard";
            }
            else if(App.UserDetails.Role == "SUPERVISOR")
            {
                Title = "Supervisor Dashboard";
            }
            else if(App.UserDetails.Role == "HOD")
            {
                Title = "HOD Dashboard";
            }
            else if(App.UserDetails.Role == "BUH")
            {
                Title = "Business Unit Head";
            }
            else if(App.UserDetails.Role == "EXECUTIVE")
            {
                Title = "Executive Dashboard";
            }
            else if(App.UserDetails.Role == "HR")
            {
                Title = "HR Dashboard";
            }
            else
            {
                Title = "Employee Dashboard";
            }
        }
#else
        public DashboardPageViewModel(ILoginService loginServices)
        {
            _loginService = loginServices;
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
            GetUserBasicInfo();
            CheckUserActivity();
            LoopCheckActivity();
            if (App.UserDetails.Role == "ADMIN1" || App.UserDetails.Role == "ADMIN2")
            {
                Title = "Admin Dashboard";
            }
            else if (App.UserDetails.Role == "CLERK")
            {
                Title = "Clerk Dashboard";
            }
            else if (App.UserDetails.Role == "SUPERVISOR")
            {
                Title = "Supervisor Dashboard";
            }
            else if (App.UserDetails.Role == "HOD")
            {
                Title = "HOD Dashboard";
            }
            else if (App.UserDetails.Role == "BUH")
            {
                Title = "Business Unit Head";
            }
            else if (App.UserDetails.Role == "EXECUTIVE")
            {
                Title = "Executive Dashboard";
            }
            else if (App.UserDetails.Role == "HR")
            {
                Title = "HR Dashboard";
            }
            else
            {
                Title = "Employee Dashboard";
            }
        }
#endif

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

        public async void getNotification()
        {
            var checkaccess = await _notificationServices.AreNotificationsEnabled();
            if (checkaccess != true)
            {
                await _notificationServices.RequestNotificationPermission();
            }
            else
            {
                DateTime now = DateTime.Now;
                var currentdaynimonth = DateTime.DaysInMonth(now.Year, now.Month);
                _notificationServices.CancelAll();
                for (int day = 1; day < currentdaynimonth; day++)
                {
                    int counter = 0;
                    DateTime dateTime = new DateTime(now.Year, now.Month, day);
                    string currentday = dateTime.ToString("d/M/yyyy");
                    var getNotifications = await _scheduleServices.GetScheduleByEmployeeNumberandDate(new Models.ClerkDTO.GetScheduleByEmployeeNumberandDateDTO
                    {
                        DayMonthYear = currentday,
                        EmployeeNumber = App.UserDetails.EmployeeNumber,
                    });

                    if (getNotifications != null && getNotifications.IsRejected == false)
                    {
                        var hm = new DateTime(dateTime.Year, dateTime.Month, dateTime.AddDays(-1).Day, 20, 0, 0);
                        counter++;
                        DateTime converted = DateTime.ParseExact(getNotifications.DayMonthYear, "d/M/yyyy hh:mm:ss tt", null);
                        getNotifications.DayMonthYear = converted.ToString("d/M/yyyy");
                        var request = new NotificationRequest()
                        {
                            Android = new AndroidOptions
                            {
                                Priority = AndroidPriority.Max,
                                VisibilityType = AndroidVisibilityType.Public,
                                ChannelId = "0829",
                            },
                            CategoryType = NotificationCategoryType.Reminder,
                            Group = "Schedule OT",
                            NotificationId = day + 1000,
                            Title = $"Your next overtime on {getNotifications.DayMonthYear}",
                            Subtitle = $"Smart Glove",
                            Description = $"At : {getNotifications.GroupName}, Hours : {getNotifications.Hours}",
                            BadgeNumber = counter,
                            Schedule = new NotificationRequestSchedule
                            {
                                NotifyTime = hm,
                                RepeatType = NotificationRepeat.No,
                            }
                        };
                        await LocalNotificationCenter.Current.Show(request);
                    }
                }
            }
        }

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
