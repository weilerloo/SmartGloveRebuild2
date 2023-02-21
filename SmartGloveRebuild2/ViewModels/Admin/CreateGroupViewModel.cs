using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.Models.Group;
using SmartGloveRebuild2.Views.Admin;
using SmartGloveRebuild2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SmartGloveRebuild2.Models;
using System.Text.RegularExpressions;
using SmartGloveRebuild2.Models.GroupResponse;

namespace SmartGloveRebuild2.ViewModels.Admin
{
    public partial class CreateGroupViewModel : BaseViewModel
    {
        [ObservableProperty]
        string creategroupname;

        private readonly IGroupServices _groupService;
        public CreateGroupViewModel(IGroupServices groupService)
        {
            _groupService = groupService;
        }

        [RelayCommand]
        public async Task CreateGroup(CreateGroupDTO createGroupDTO)
        {
            if(creategroupname.Count() > 10)
            {
                await Shell.Current.DisplayAlert("Group Name Limits", "Please enter the name only with maximum of 10.", "Ok");
                return;
            }
            var FindGroup = await _groupService.CreateGroup(new CreateGroupDTO
            {
                GroupName = creategroupname,
            });
            if (FindGroup.IsSuccess == true)
            {
                await Shell.Current.DisplayAlert("New Group added", "Group added succesfully", "Ok");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Group Existed", "Group added failed, try other name please.", "Ok");

            }
        }
    }
}
