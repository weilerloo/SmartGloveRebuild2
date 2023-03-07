using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Models.ScheduleResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Services
{
    public interface ILoginService
    {
        Task<LoginResponse> Authenticate(LoginRequest loginRequest);
        Task<string> CheckRefreshToken(LoginRequest loginRequest);
        Task<UserBasicInfo> GetUserBasicInfo(LoginRequest loginRequest);
    }
}