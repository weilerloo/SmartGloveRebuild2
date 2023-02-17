using Newtonsoft.Json;
using SmartGloveRebuild2.Controls;
using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Views.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.Views.Startup;
using SmartGloveRebuild2.Services;
using Microsoft.Maui.Networking;
using SmartGloveOvertime.Handlers;
using CommunityToolkit.Maui.Views;

namespace SmartGloveRebuild2.ViewModels.Startup
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _employeeNumber;

        [ObservableProperty]
        private string _password;


        private readonly ILoginService _loginService;
        private readonly IConnectivity _connectivity;

        public LoginPageViewModel(ILoginService loginServices, IConnectivity connectivity)
        {
            _loginService = loginServices;
            _connectivity = connectivity;
        }

        #region Commands

        [RelayCommand]
        async void Login()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            PopupPages p = new PopupPages();
            Application.Current.MainPage.ShowPopup(p);
            await Task.Delay(100);
            try
            {
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("No connectivity!",
                        $"Please check internet and try again.", "OK");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(EmployeeNumber) && !string.IsNullOrWhiteSpace(Password))
                {

                    var response = await _loginService.Authenticate(new LoginRequest
                    {
                        UserName = EmployeeNumber,
                        Password = Password
                    });

                    if (response != null)
                    {

                        if (response.UserDetail.Role == null)
                        {
                            await AppShell.Current.DisplayAlert("No Role Assigned",
                                "No role assigned to this user.", "OK");
                            return;
                        }
                        response.UserDetail.EmployeeNumber = EmployeeNumber;

                        if (Preferences.ContainsKey(nameof(App.UserDetails)))
                        {
                            Preferences.Remove(nameof(App.UserDetails));
                        }

                        await SecureStorage.SetAsync(nameof(App.Token), response.Token);

                        string userDetailStr = JsonConvert.SerializeObject(response.UserDetail);
                        Preferences.Set(nameof(App.UserDetails), userDetailStr);
                        App.UserDetails = response.UserDetail;
                        App.Token = response.Token;
                        await AppConstant.AddFlyoutMenusDetails();

                    }
                    else
                    {
                        await AppShell.Current.DisplayAlert("Invalid User Name Or Password", "Invalid UserName or Password", "OK");
                    }
                }
                else
                {
                    await AppShell.Current.DisplayAlert("No User Name or Password", "Please Enter Users Name or Passwword", "OK");
                }
            }
            catch 
            {
                await Shell.Current.DisplayAlert("Internet Error", "Failed to Conenct to Server. ", "OK");
            }
            p.Close();
            IsBusy = false;
            #endregion
        }

    }
}
