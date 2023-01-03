using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.Models;
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
    [QueryProperty(nameof(CalendarModel), "CalendarModel")]
    public partial class ExclusionListViewModel : BaseViewModel
    {
        #region ObservableCollections
        public ObservableCollection<GroupScheduleModel> addedGroupSchedule { get; set; } = new ObservableCollection<GroupScheduleModel>();
        public ObservableCollection<GroupScheduleModel> Items { get; set; } = new ObservableCollection<GroupScheduleModel>();
        #endregion


        private CalendarModel selectedItem;
        public CalendarModel SelectedItem
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
        public ExclusionListViewModel(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
            //foreach (var groups in UpdateSlotsViewModel.GroupSchedule)
            //{
            //    if (groups.GroupName == "Unassigned")
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        Color color;
            //        string statonoff;
            //        if (groups.Status == true)
            //        {
            //            color = Color.FromArgb("#7CFC00");
            //            statonoff = "ON";
            //        }
            //        else
            //        {
            //            color = Color.FromArgb("#FF0000");
            //            statonoff = "OFF";
            //        }

            //        addedGroupSchedule.Add(new GroupScheduleModel
            //        {
            //            GroupName = groups.GroupName,
            //            Hours = groups.Hours,
            //            Paxs = groups.Paxs,
            //            Status = groups.Status,
            //            DayMonthYear = groups.DayMonthYear,
            //            Color = color,
            //            OnOff = statonoff,
            //        });
            //        daymonthyear = groups.DayMonthYear;
            //    }
            //    Items = new ObservableCollection<GroupScheduleModel>();
            //    SelectedItem = new GroupScheduleModel();

            //}

        }

        [ObservableProperty]
        CalendarModel calendarModel;

        [ObservableProperty]
        bool isRefreshing;


    }
}