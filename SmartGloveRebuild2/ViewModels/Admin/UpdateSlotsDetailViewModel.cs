using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Network;
using SmartGloveRebuild2.Models.ClerkDTO;
using SmartGloveRebuild2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.ViewModels.Admin
{
    [QueryProperty("UpdateSlotsModel", "UpdateSlotsModel")]
    public partial class UpdateSlotsDetailViewModel : BaseViewModel
    {

        [ObservableProperty]
        UpdateSlotsModel updateSlotsModel;

        private readonly IScheduleServices _scheduleServices;
        public UpdateSlotsDetailViewModel(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
        }

        [RelayCommand]
        async Task GetSchdule()
        {
            try
            {
                var response = await _scheduleServices.GetSchedule();

                if (response != null)
                {
                    App.UserDetails = response.UserDetail;
                    App.Token = response.Token;
                }
                else
                {
                    await AppShell.Current.DisplayAlert("Error!", "No Group or Schedule Found!", "OK");

                }
            }
            catch (Exception ex)
            {
                return ex;

            }
        }


    }
}
