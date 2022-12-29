﻿using Newtonsoft.Json;
using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Views.Startup;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Services
{
    public class LoginServices : ILoginService
    {
        public async Task<LoginResponse> Authenticate(LoginRequest loginRequest)
        {
            using (var client = new HttpClient())
            {
                string loginRequestStr = JsonConvert.SerializeObject(loginRequest);

                var response = await client.PostAsync("http://172.16.12.151:7006/api/Users/AuthenticateUser",
                      new StringContent(loginRequestStr, Encoding.UTF8,
                      "application/json"));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LoginResponse>(json);
                }
                else
                {
                    return null;
                }
            }
        }

    }
}