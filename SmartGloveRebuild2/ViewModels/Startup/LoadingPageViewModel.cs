﻿using Newtonsoft.Json;
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

    public class LoadingPageViewModel
    {
        public LoadingPageViewModel()
        {
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
                        await Shell.Current.GoToAsync($"//{nameof(SignInPage)}");
                    });
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{nameof(SignInPage)}");
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
                    await Shell.Current.GoToAsync($"/{nameof(SignInPage)}");
                    await Shell.Current.DisplayAlert("User session expired", "Login Again To conitnue", "OK");
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

    }
}