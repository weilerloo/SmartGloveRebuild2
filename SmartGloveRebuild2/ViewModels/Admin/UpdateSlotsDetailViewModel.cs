using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.Models.ClerkDTO;
using SmartGloveRebuild2.Models.Group;
using SmartGloveRebuild2.Models.Schedule;
using SmartGloveRebuild2.Models.ScheduleResponse;
using SmartGloveRebuild2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.ViewModels.Admin
{
    [QueryProperty(nameof(UpdateSlotsModel), "UpdateSlotsModel")]
    public partial class UpdateSlotsDetailViewModel : BaseViewModel
    {
        #region ObservableCollections
        public ObservableCollection<GroupScheduleModel> GroupSchedule { get; set; } = new ObservableCollection<GroupScheduleModel>();

        #endregion

        private readonly IScheduleServices _scheduleServices;
        public UpdateSlotsDetailViewModel(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
        }

        [ObservableProperty]
        UpdateSlotsModel updateSlotsModel;

        [ObservableProperty]
        string daymonthyear, groupname, paxs, status;

        [ObservableProperty]
        double hours;

        public void ConcatenateLabelDate()
        {
            if (UpdateSlotsModel.Day.ToString().Length > 1)
            {
                string day = UpdateSlotsModel.Day.ToString();
                string month = UpdateSlotsModel.Currentmonth.ToString();
                string year = UpdateSlotsModel.Currentyear.ToString();
                daymonthyear = day + "%2F" + month + "%2F" + year;
            }
            else
            {
                string day = UpdateSlotsModel.Day.ToString();
                string month = UpdateSlotsModel.Currentmonth.ToString();
                string year = UpdateSlotsModel.Currentyear.ToString();
                daymonthyear = day + "%2F" + month + "%2F" + year;
            }

        }

        [RelayCommand]
        public async void GetGroupSchdule()
        {
            try
            {
                ConcatenateLabelDate();
                if (App.UserDetails.Role == "CLERK")
                {

                    var response = await _scheduleServices.GetSchedulebyDate(new GetSchedulebyDateDTO
                    {
                        ScheduleDate = daymonthyear,
                    });

                    if (response != null)
                    {
                        GroupSchedule.Add(new GroupScheduleModel
                        {
                            GroupName = response.GroupName,
                            Hours = response.Hours,
                            Paxs = response.Paxs,
                            Status = response.Status,
                        });
                    }
                    else
                    {
                        await AppShell.Current.DisplayAlert("Hello", "Takpaye kerja", "OK");
                    }
                }
                else
                {
                    await AppShell.Current.DisplayAlert("Error!", "No Group or Schedule Found!", "OK");

                }
            }
            catch (Exception ex)
            {


            }
        }


    }
}
