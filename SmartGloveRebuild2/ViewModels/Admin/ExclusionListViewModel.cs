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
        public ObservableCollection<GroupList> FetchedRejectList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> Items { get; set; } = new ObservableCollection<GroupList>();
        public static ObservableCollection<GroupList> ReasonRejectList { get; set; } = new ObservableCollection<GroupList>();
        #endregion


        private GroupList selectedItem;
        public GroupList SelectedItem
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
            foreach (var groups in CheckCalendarViewModel.RejectList)
            {
                FetchedRejectList.Add(new GroupList
                {
                    EmployeeName = groups.EmployeeName,
                    UserName = groups.UserName,
                    GroupName = groups.GroupName,
                });
            }

            Items = new ObservableCollection<GroupList>();
            SelectedItem = new GroupList();
        }


        [ObservableProperty]
        CalendarModel calendarModel;

        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        public void RejectEmployee(GroupList selectedItem)
        {
            if (selectedItem == null)
            {
                return;
            }

            foreach (var item in FetchedRejectList)
            {
                if (selectedItem.EmployeeName == item.EmployeeName)
                {
                    CheckCalendarViewModel.RejectList.Remove(item);
                    FetchedRejectList.Remove(item);
                    ReasonRejectList.Add(item);                
                    break;
                }
            }
        }

        [RelayCommand]
        public async Task NextReasonRejectList()
        {
            await Shell.Current.GoToAsync(nameof(NextReasonRejectListPage));
        }

    }

}
