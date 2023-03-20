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
        public ObservableCollection<GroupList> SearchedGroupList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> BeforeSearchedGroupList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> BeforeReasonRejectList { get; set; } = new ObservableCollection<GroupList>();
        public static ObservableCollection<GroupList> ReasonRejectList { get; set; } = new ObservableCollection<GroupList>();
        public static ObservableCollection<GroupList> ReasonApprovedList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> Items { get; set; } = new ObservableCollection<GroupList>();
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

        private bool cansee;
        public bool Cansee
        {
            get => cansee;
            set => SetProperty(ref cansee, value);
        }

        private string accepttextsearch;
        public string AcceptTxtSearch
        {
            get => accepttextsearch;
            set
            {
                accepttextsearch = value;
                OnPropertyChanged();
                OnAcceptSearchContactCommand();
            }
        }

        private string rejecttextsearch;
        public string RejectTxtSearch
        {
            get => rejecttextsearch;
            set
            {
                rejecttextsearch = value;
                OnPropertyChanged();
                OnRejectSearchContactCommand();
            }
        }

        [ObservableProperty]
        CalendarModel calendarModel;

        [ObservableProperty]
        bool isRefreshing;

        private readonly IScheduleServices _scheduleServices;
        public ExclusionListViewModel(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
            if (ReasonRejectList != null)
            {
                ReasonRejectList.Clear();
            }
            foreach (var groups in CheckCalendarViewModel.RejectList)
            {
                if (groups.IsRejected == true)
                {
                    continue;
                }
                else
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
            var list = FetchedRejectList.OrderBy(f => f.EmployeeName).ToList();
            FetchedRejectList.Clear();
            foreach (var contact in list)
            {
                FetchedRejectList.Add(contact); //12
            }
            List<GroupList> originalEnityList = FetchedRejectList.ToList();  //53, 12
            ObservableCollection<GroupList> bRef = new ObservableCollection<GroupList>(originalEnityList);
            SearchedGroupList = bRef;
            Items = new ObservableCollection<GroupList>();
            SelectedItem = new GroupList();
        }


        private void OnRejectSearchContactCommand()
        {

            var founContacts = SearchedGroupList.Where(found =>
             found.UserName.Contains(RejectTxtSearch.ToUpper()) ||
             found.EmployeeName.Contains(RejectTxtSearch.ToUpper())
             ).ToList();  //12

            if (founContacts.Count > 0)
            {

                FetchedRejectList.Clear();
                foreach (var contact in founContacts)
                {
                    FetchedRejectList.Add(contact); //12
                }
            }
            else
            {
                FetchedRejectList.Clear();
            }
        }

        private void OnAcceptSearchContactCommand()
        {
            var founContacts = BeforeSearchedGroupList.Where(found =>
             found.UserName.Contains(AcceptTxtSearch.ToUpper()) ||
             found.EmployeeName.Contains(AcceptTxtSearch.ToUpper())
             ).ToList();  //12

            if (founContacts.Count > 0)
            {

                BeforeReasonRejectList.Clear();
                foreach (var contact in founContacts)
                {
                    BeforeReasonRejectList.Add(contact); //12
                }
            }
            else
            {
                BeforeReasonRejectList.Clear();
            }
        }


        [RelayCommand]
        public void AddIntoRejectEmployee(GroupList selectedItem)
        {
            if (selectedItem == null)
            {
                return;
            }

            foreach (var item in FetchedRejectList)
            {
                if (selectedItem.UserName == item.UserName)
                {
                    FetchedRejectList.Remove(item);
                    SearchedGroupList.Remove(item);
                    ReasonRejectList.Add(item);
                    BeforeReasonRejectList.Add(item);
                    break;
                }
            }
        }

        [RelayCommand]
        public void DeleteFromRejectEmployee(GroupList selectedItem)
        {
            if (selectedItem == null)
            {
                return;
            }

            foreach (var item in BeforeReasonRejectList)
            {
                if (selectedItem.UserName == item.UserName)
                {
                    FetchedRejectList.Add(item); //Original
                    SearchedGroupList.Add(item); //Original copied
                    ReasonRejectList.Remove(item); //Delete from Reject List
                    BeforeReasonRejectList.Remove(item);
                    BeforeSearchedGroupList.Remove(item);
                    break;
                }
            }
        }

        [RelayCommand]
        public void RejectAllEmployee(GroupList selectedItem)
        {
            if (FetchedRejectList.Count() > 0)
            {
                foreach (var item in FetchedRejectList.ToList())
                {
                    FetchedRejectList.Remove(item);
                    ReasonRejectList.Add(item);
                    BeforeReasonRejectList.Add(item);
                    BeforeSearchedGroupList.Add(item);
                }
            }
            else
            {
                return;
            }
        }


        [RelayCommand]
        public async Task NextReasonRejectList()
        {
            if (BeforeReasonRejectList.Count <= 0 && ReasonApprovedList.Count <= 0 && FetchedRejectList.Count <= 0)
            {
                await Shell.Current.DisplayAlert("Messages", "No Employee are found.", "OK");
                await Shell.Current.GoToAsync("../../..");
            }
            else
            {
                if (ReasonApprovedList.Count() != 0)
                {
                    ReasonApprovedList.Clear();
                }

                foreach (var item in FetchedRejectList.ToList())
                {
                    ReasonApprovedList.Add(item);
                }
                await Shell.Current.GoToAsync(nameof(NextReasonRejectListPage));
            }

        }


        [RelayCommand]
        public void NextIsPressed()
        {
            if (BeforeReasonRejectList.Count != 0)
            {
                List<GroupList> originalEnityList = BeforeReasonRejectList.ToList();  //53, 12
                ObservableCollection<GroupList> bRef = new ObservableCollection<GroupList>(originalEnityList);
                BeforeSearchedGroupList = bRef;
            }
            Cansee = false;
        }

        [RelayCommand]
        public void BackIsPressed()
        {
            if (BeforeReasonRejectList.Count != 0)
            {
                List<GroupList> originalEnityList = BeforeReasonRejectList.ToList();  //53, 12
                ObservableCollection<GroupList> bRef = new ObservableCollection<GroupList>(originalEnityList);
                BeforeSearchedGroupList = bRef;
            }
            Cansee = true;
        }
    }

}
