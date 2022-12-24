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
        public ObservableCollection<UserBasicInfo> displaygrp { get; set; } = new ObservableCollection<UserBasicInfo>();
        public ObservableCollection<GroupList> GroupNameList { get; set; } = new ObservableCollection<GroupList>();

        [ObservableProperty]
        string entrygroupname;        
        
        [ObservableProperty]
        bool isBusy, isRefreshing;

        private readonly IGroupServices _groupServices;
        public DisplayGroupViewModel(IGroupServices groupServices)
        {
            _groupServices = groupServices;
            //FetchGroupName();
        }


        //    [RelayCommand]
        //    public async void FetchGroupName()
        //    {
        //        isBusy = true;
        //        var response = await _groupServices.DisplayGroup();
        //        if (response.Count > 0)
        //        {
        //            foreach (var grp in response)
        //            {
        //                GroupNameList.Add(response);
        //            }
        //        }
        //        isRefreshing= false;
        //        isBusy = false;
        //    }        

        //    [RelayCommand]
        //    public async void DisplayGroupMember(CreateGroupDTO createGroupDTO)
        //    {
        //        isBusy = true;
        //        var response = await _groupServices.DisplayGroupbyGroupName(new CreateGroupDTO
        //        {
        //            GroupName = entrygroupname,
        //        });

        //        if (response != null)
        //        {
        //            foreach (var grp in response)
        //            {
        //                displaygrp.Add(new UserBasicInfo
        //                {
        //                    EmployeeName = grp.UserName,
        //                    EmployeeNumber = grp.EmployeeName,
        //                    GroupName = grp.GroupName,
        //                    TotalHour = grp.TotalHour,
        //                });
        //            }
        //        }
        //        isRefreshing= false;
        //        isBusy = false;
        //    }
    }

}