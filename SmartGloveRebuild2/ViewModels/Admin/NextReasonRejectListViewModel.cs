﻿using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class NextReasonRejectListViewModel : BaseViewModel
    {
        #region ObservableCollections
        public ObservableCollection<GroupList> FetchedRejectList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> Items { get; set; } = new ObservableCollection<GroupList>();
        #endregion

        private readonly IScheduleServices _scheduleServices;
        public NextReasonRejectListViewModel(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
            foreach (var groups in ExclusionListViewModel.ReasonRejectList)
            {
                FetchedRejectList.Add(new GroupList
                {
                    EmployeeName = groups.EmployeeName,
                    UserName = groups.UserName,
                    GroupName = groups.GroupName,
                });
            }
            cansee = true;
            Items = new ObservableCollection<GroupList>();
            SelectedItem = new GroupList();
        }

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

        private bool cansee;
        public bool Cansee
        {
            get => cansee;
            set => SetProperty(ref cansee, value);
        }


        [ObservableProperty]
        string reason;

        [ObservableProperty]
        CalendarModel calendarModel;



        [RelayCommand]
        public void NextIsPressed()
        {
            Cansee = false;
        }        
        
        [RelayCommand]
        public void BackIsPressed()
        {
            Cansee = true;
        }


        [RelayCommand]
        public async Task RemoveFromList()
        {
            if (IsBusy) { return; }
            if (cansee == false)
            {
                var action = await Shell.Current.DisplayAlert("Messages", "Are you sure to exclude the Request?", "Yes", "No");
                if (action)
                {
                    IsBusy = true;
                    foreach (var group in FetchedRejectList)
                    {
                        var rejectEmployee = await _scheduleServices.RejectSchedule(new RejectScheduleDTO
                        {
                            DateValue = DateTime.Now,
                            EmployeeNumber = group.UserName,
                            GroupName = group.GroupName,
                            IsRejected = true,
                            RejectedReason = reason,
                        });
                    }
                }
                IsBusy = false;
                await Shell.Current.DisplayAlert("Messages", "Reject Successfull.", "Ok");
                await Shell.Current.GoToAsync(nameof(CheckCalendarPage));
            }
        }



        [RelayCommand]
        public void RevertEmployee(GroupList selectedItem)
        {
            if (selectedItem == null)
            {
                return;
            }

            foreach (var item in FetchedRejectList)
            {
                if (selectedItem.EmployeeName == item.EmployeeName)
                {
                    CheckCalendarViewModel.RejectList.Add(item);
                    ExclusionListViewModel.ReasonRejectList.Add(item);
                    FetchedRejectList.Remove(item);
                    break;
                }
            }
        }
    }

}
