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
    public partial class NextReasonRejectListViewModel : BaseViewModel
    {
        #region ObservableCollections
        public ObservableCollection<GroupList> FetchedRejectList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> FetchedApprovedList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> Items { get; set; } = new ObservableCollection<GroupList>();
        #endregion

        private readonly IScheduleServices _scheduleServices;
        public NextReasonRejectListViewModel(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
            if (ExclusionListViewModel.ReasonRejectList.Count > 0 || ExclusionListViewModel.ReasonApprovedList.Count > 0)
            {
                Reasonremark = "Enter your approved reason here...";
                if (ExclusionListViewModel.ReasonRejectList.Count != 0)
                {
                    Cansee = true;
                }

                foreach (var groups in ExclusionListViewModel.ReasonRejectList)
                {
                    FetchedRejectList.Add(new GroupList
                    {
                        DayMonthYear = groups.DayMonthYear,
                        EmployeeName = groups.EmployeeName,
                        UserName = groups.UserName,
                        GroupName = groups.GroupName,
                    });
                }

                foreach (var groups in ExclusionListViewModel.ReasonApprovedList)
                {
                    FetchedApprovedList.Add(new GroupList
                    {
                        DayMonthYear = groups.DayMonthYear,
                        EmployeeName = groups.EmployeeName,
                        UserName = groups.UserName,
                        GroupName = groups.GroupName,
                    });
                }
            }
            else if (ExclusionMultipleDateViewModel.MultipleRejectList.Count > 0)
            {
                Reasonremark = "Reason of exclusion here...";
                foreach (var groups in ExclusionMultipleDateViewModel.MultipleRejectList)
                {
                    FetchedRejectList.Add(new GroupList
                    {
                        DayMonthYear = groups.DayMonthYear,
                        EmployeeName = groups.EmployeeName,
                        UserName = groups.UserName,
                        GroupName = groups.GroupName,
                    });
                }
            }
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
        
        private string reasonremark;
        public string Reasonremark
        {
            get => reasonremark;
            set => SetProperty(ref reasonremark, value);
        }

        [ObservableProperty]
        string rejectreason, approvedreason;

        [ObservableProperty]
        CalendarModel calendarModel;

        [RelayCommand]
        public async Task ApprovedRemoveFromList()
        {
            if (IsBusy) { return; }

            var action = await Shell.Current.DisplayAlert("Messages", "Are you sure to confirm the Request?", "Yes", "No");
            if (action)
            {
                if (approvedreason == null && rejectreason == null)
                {
                    await Shell.Current.DisplayAlert("Messages", "Reason cannot be Empty! Please enter the reasons.", "OK");
                    return;
                }

                IsBusy = true;
                foreach (var group in FetchedRejectList)
                {
                    var getSchedule = await _scheduleServices.GetSchedulebyGroupandDate(new GetSchedulebyGroupandDateDTO
                    {
                        GroupName = group.GroupName,
                        ScheduleDate = group.DayMonthYear,
                    });
                    var Rejectname = await _scheduleServices.GetScheduleLogsByGroupandDate(new GetSchedulebyGroupandDateDTO
                    {
                        GroupName = group.GroupName,
                        ScheduleDate = group.DayMonthYear,
                    });
                    if (group.EmployeeName == null && group.UserName == null) //For Bulk Reject
                    {
                        foreach (var s in getSchedule)
                        {
                            if (s.DayMonthYear == group.DayMonthYear)
                            {
                                var rejectDate = await _scheduleServices.updateScheduleStatusByGroupName(new UpdateScheduleStatusByGroupNameDTO
                                {
                                    GroupName = group.GroupName,
                                    DayMonthYear = group.DayMonthYear,
                                    Hours = s.Hours,
                                    Paxs = s.Paxs,
                                    Status = false,
                                });
                            }
                            break;
                        }
                        foreach (var user in Rejectname)
                        {
                            var BulkrejectEmployee = await _scheduleServices.RejectSchedule(new RejectScheduleDTO
                            {
                                RejectedDate = DateTime.Now,
                                EmployeeNumber = user.EmployeeNumber,
                                GroupName = group.GroupName,
                                IsRejected = true,
                                RejectedBy = App.UserDetails.EmployeeNumber,
                                RejectedReason = approvedreason,
                                DayMonthYear = group.DayMonthYear,
                            });
                        }
                    }
                    else
                    {
                        var rejectEmployee = await _scheduleServices.RejectSchedule(new RejectScheduleDTO
                        {
                            RejectedDate = DateTime.Now,
                            EmployeeNumber = group.UserName,
                            GroupName = group.GroupName,
                            IsRejected = true,
                            RejectedBy = App.UserDetails.EmployeeNumber,
                            RejectedReason = rejectreason,
                            DayMonthYear = group.DayMonthYear,
                        });  //For Single Reject

                        if (rejectEmployee.IsSuccess == false)
                        {
                            await Shell.Current.DisplayAlert("Messages", "Reject Unsuccessfull. Please contact the peers for the further issues.", "Ok");
                            return;
                        }
                    }


                }

                foreach (var group2 in FetchedApprovedList)
                {
                    var getSchedule = await _scheduleServices.GetSchedulebyGroupandDate(new GetSchedulebyGroupandDateDTO
                    {
                        GroupName = group2.GroupName,
                        ScheduleDate = group2.DayMonthYear,
                    });

                    if (getSchedule != null)
                    {
                        foreach (var g in getSchedule)
                        {
                            var response2 = await _scheduleServices.updateScheduleStatusByGroupName(new UpdateScheduleStatusByGroupNameDTO
                            {
                                DayMonthYear = g.DayMonthYear,
                                GroupName = g.GroupName,
                                Status = g.Status,
                                Hours = g.Hours,
                                Remarks = approvedreason,
                            });
                        }
                    }
                }
                if (FetchedApprovedList.Count == 0)
                {

                    foreach (var getfromreject in FetchedRejectList)
                    {
                        var getSchedule = await _scheduleServices.GetSchedulebyGroupandDate(new GetSchedulebyGroupandDateDTO
                        {
                            GroupName = getfromreject.GroupName,
                            ScheduleDate = getfromreject.DayMonthYear,
                        });
                        foreach (var hp in getSchedule)
                        {
                            var response2 = await _scheduleServices.updateScheduleStatusByGroupName(new UpdateScheduleStatusByGroupNameDTO
                            {
                                Hours = hp.Hours,
                                Paxs = hp.Paxs,
                                DayMonthYear = getfromreject.DayMonthYear,
                                GroupName = getfromreject.GroupName,
                                Status = hp.Status,
                                Remarks = approvedreason,
                            });
                        }
                    }
                }
                IsBusy = false;
                await Shell.Current.DisplayAlert("Messages", "Submit Successfull.", "Ok");
                await Shell.Current.GoToAsync("../..");
            }
        }
    }

}
