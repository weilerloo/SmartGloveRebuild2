using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.Views;
using System.Collections.ObjectModel;
using SmartGloveRebuild2.Models.Group;
using SmartGloveRebuild2.Views.Admin;
using SmartGloveRebuild2.Models.ClerkDTO;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SmartGloveRebuild2.ViewModels.Admin
{
    public partial class DisplayGroupViewModel : BaseViewModel
    {
        public ObservableCollection<GroupList> GroupNameList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> GroupTitleList { get; set; } = new ObservableCollection<GroupList>();
        public static ObservableCollection<GroupList> DisplayGroupList { get; set; } = new ObservableCollection<GroupList>();

        [ObservableProperty]
        string entrygroupname;

        [ObservableProperty]
        bool isRefreshing;

        private int totalworker;
        public int TotalWorker
        {
            get { return totalworker; }
            set
            {
                totalworker = value;
                OnPropertyChanged();
            }
        }


        private GroupList selectedgroupname;
        public GroupList SelectedGroupname
        {
            get => selectedgroupname;
            set
            {
                selectedgroupname = value;
                OnPropertyChanged();
                DisplayGroupMember();
            }
        }

        private readonly IGroupServices _groupServices;
        public DisplayGroupViewModel(IGroupServices groupServices)
        {
            _groupServices = groupServices;
            DisplayGroupMember();
        }


        [RelayCommand]
        public async void DisplayGroupMember()
        {
            if (IsBusy) { return; }

            IsBusy = true;
            var response = await _groupServices.DisplayGroupFromUsers();

            if (response.Count > 0)
            {
                foreach (var grp in response)
                {
                    var res = GroupTitleList.Where(f => f.GroupName.Equals(grp.GroupName)).FirstOrDefault();
                    if (res != null)
                    {
                        continue;
                    }
                    else
                    {
                        GroupTitleList.Add(new GroupList
                        {
                            GroupName = grp.GroupName,
                        });
                    }
                }
            }

            if (SelectedGroupname != null)
            {
                if (GroupNameList.Count > 0)
                {
                    GroupNameList.Clear();
                    TotalWorker = 0;
                }

                foreach (var grp in response)
                {
                    var res = GroupTitleList.Where(f => f.GroupName.Contains(SelectedGroupname.GroupName));
                    if (res != null)
                    {
                        if (grp.GroupName == SelectedGroupname.GroupName)
                        {
                            GroupNameList.Add(new GroupList
                            {
                                GroupName = grp.GroupName,
                                EmployeeName = grp.EmployeeName,
                                TotalHour = grp.TotalHour,
                                UserName = grp.UserName,

                            });

                            TotalWorker++;
                        }
                    }
                }
            }
            IsRefreshing = false;
            IsBusy = false;
        }

        [RelayCommand]
        public async void EditGroup()
        {

            if (selectedgroupname == null)
            {
                return;
            }
            IsBusy = true;

            if (DisplayGroupList.Count > 0)
            {
                DisplayGroupList.Clear();
            }
            var response = await _groupServices.DisplayGroupbyGroupName(selectedgroupname.GroupName);
            if (response != null)
            {
                foreach (var employee in response)
                {
                    DisplayGroupList.Add(new GroupList
                    {
                        GroupName = employee.GroupName,
                        EmployeeName = employee.EmployeeName,
                        TotalHour = employee.TotalHour,
                        UserName = employee.UserName,
                    });
                }
            }
            IsRefreshing = false;
            IsBusy = false;

            await Shell.Current.GoToAsync($"{nameof(AssignGroupPage)}?", true, new Dictionary<string, object>
            {
                [nameof(GroupList)] = selectedgroupname
            });
        }
    }
}