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

namespace SmartGloveRebuild2.ViewModels.Admin
{
    public partial class DisplayGroupViewModel : BaseViewModel
    {
        public ObservableCollection<GroupList> GroupNameList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> GroupTitleList { get; set; } = new ObservableCollection<GroupList>();

        [ObservableProperty]
        string entrygroupname;

        [ObservableProperty]
        bool isBusy, isRefreshing;

        private GroupList selectedgroupname;
        public GroupList SelectedGroupname
        {
            get => selectedgroupname;
            set
            {
                selectedgroupname = value;
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
            isBusy = true;
            var response = await _groupServices.DisplayGroupFromUsers();

            if (response.Count > 0)
            {
                foreach (var grp in response)
                {
                    var res = GroupTitleList.Where(f => f.GroupName.Contains(grp.GroupName)).FirstOrDefault();
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
                if(GroupNameList.Count > 0)
                {
                    GroupNameList.Clear();
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
                        }
                    }
                }
            }
            isRefreshing = false;
            isBusy = false;
        }
    }
}