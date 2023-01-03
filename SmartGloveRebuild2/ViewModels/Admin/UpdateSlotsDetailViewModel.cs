using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.Models.ClerkDTO;
using SmartGloveRebuild2.Models.Group;
using SmartGloveRebuild2.Models.Schedule;
using SmartGloveRebuild2.Models.ScheduleResponse;
using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.Views.Admin;
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
        public ObservableCollection<GroupScheduleModel> addedGroupSchedule { get; set; } = new ObservableCollection<GroupScheduleModel>();
        public ObservableCollection<GroupScheduleModel> Items { get; set; } = new ObservableCollection<GroupScheduleModel>();
        #endregion


        public GroupScheduleModel selectedItem;
        public GroupScheduleModel SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                }
            }
        }

        private readonly IScheduleServices _scheduleServices;
        public UpdateSlotsDetailViewModel(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
            foreach (var groups in UpdateSlotsViewModel.GroupSchedule)
            {
                if (groups.GroupName == "Unassigned")
                {
                    continue;
                }
                else
                {
                    Color color;
                    string statonoff;
                    if (groups.Status == true)
                    {
                        color = Color.FromArgb("#7CFC00");
                        statonoff = "ON";
                    }
                    else
                    {
                        color = Color.FromArgb("#FF0000");
                        statonoff = "OFF";
                    }

                    addedGroupSchedule.Add(new GroupScheduleModel
                    {
                        GroupName = groups.GroupName,
                        Hours = groups.Hours,
                        Paxs = groups.Paxs,
                        Status = groups.Status,
                        DayMonthYear = groups.DayMonthYear,
                        Color = color,
                        OnOff = statonoff,
                    });
                    daymonthyear = groups.DayMonthYear;
                }
                Items = new ObservableCollection<GroupScheduleModel>();
                SelectedItem = new GroupScheduleModel();

            }

        }

        [ObservableProperty]
        UpdateSlotsModel updateslotsmodel;

        [ObservableProperty]
        string daymonthyear, groupname;

        [ObservableProperty]
        double hours;

        [ObservableProperty]
        int paxs;

        [ObservableProperty]
        bool isRefreshing;


        [RelayCommand]
        public async Task UpdateStatus()
        {
            IsBusy = true;
            foreach (var content in addedGroupSchedule)
            {
                if ((content.Paxs == 0 || content.Hours == 0) && content.Status == true)
                {
                    content.Status = false;
                    content.OnOff = "OFF";
                }
                var response = await _scheduleServices.updateScheduleStatusByGroupName(new UpdateScheduleStatusByGroupNameDTO
                {
                    DayMonthYear = content.DayMonthYear,
                    GroupName = content.GroupName,
                    Status = content.Status,
                    Hours = content.Hours,
                    Paxs = content.Paxs,
                });
            }

            IsRefreshing = false;
            IsBusy = false;
            await Shell.Current.DisplayAlert("Messages", "Schedule Updated.", "OK");
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public void UpdateButton(GroupScheduleModel selectedItem)
        {
            if (selectedItem != null)
            {
                foreach (var ccm in addedGroupSchedule)
                {
                    if (ccm.GroupName == selectedItem.GroupName)
                    {
                        if (ccm.Status == true)
                        {
                            ccm.Status = false;
                            ccm.Color = Color.FromArgb("#FF0000");
                            ccm.OnOff = "OFF";
                        }
                        else if (ccm.Status == false)
                        {
                            ccm.Status = true;
                            ccm.Color = Color.FromArgb("#7CFC00");
                            ccm.OnOff = "ON";
                        }
                    }
                }
            }

        }

        [RelayCommand]
        public void RefreshButton()
        {
            addedGroupSchedule.Clear();
            foreach (var groups in UpdateSlotsViewModel.GroupSchedule)
            {
                if (groups.GroupName == "Unassigned")
                {
                    continue;
                }
                Color color;
                string statonoff;
                if (groups.Status == true)
                {
                    color = Color.FromArgb("#7CFC00");
                    statonoff = "ON";
                }
                else
                {
                    color = Color.FromArgb("#FF0000");
                    statonoff = "OFF";
                }

                addedGroupSchedule.Add(new GroupScheduleModel
                {
                    GroupName = groups.GroupName,
                    Hours = groups.Hours,
                    Paxs = groups.Paxs,
                    Status = groups.Status,
                    DayMonthYear = groups.DayMonthYear,
                    Color = color,
                    OnOff = statonoff,
                });
                daymonthyear = groups.DayMonthYear;
            }
        }
    }
}