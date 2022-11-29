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

namespace SmartGloveRebuild2.ViewModels.Startup
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _employeeNumber;

        [ObservableProperty]
        private string _password;


        private readonly ILoginService _loginService;

        public LoginPageViewModel(ILoginService loginServices)
        {
            _loginService = loginServices;
        }

        #region Commands
        [RelayCommand]
        async void Login()
        {
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


        }
        #endregion
    }




}
