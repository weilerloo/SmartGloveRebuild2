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
using CommunityToolkit.Maui.Views;
using SmartGloveOvertime.Handlers;
using System.Windows.Input;

namespace SmartGloveRebuild2.ViewModels.Admin
{
    public partial class DisplayGroupViewModel : BaseViewModel
    {
        public ObservableCollection<GroupList> GroupNameList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> GroupTitleList { get; set; } = new ObservableCollection<GroupList>();
        public static ObservableCollection<GroupList> DisplayGroupList { get; set; } = new ObservableCollection<GroupList>();
        public ICommand SearchEmptyLoadContactCommand { get; private set; }

        [ObservableProperty]
        string entrygroupname, textsearch;

        [ObservableProperty]
        bool isRefreshing;

        private int numworker;
        public int Numworker
        {
            get => numworker;
            set
            {
                numworker = value;
                DisplayGroupMember();
                OnPropertyChanged("Numworker");
            }
        }

        private bool cansee;
        public bool Cansee
        {
            get => cansee;
            set => SetProperty(ref cansee, value);
        }
        public string TextSearch
        {
            get => textsearch;
            set
            {
                textsearch = value;
                OnPropertyChanged();
                if (textsearch.Length > 0)
                {
                    OnSearchContactCommand();
                }
                else
                {
                    SearchEmptyLoadContactCommand.Execute(null);
                }
            }
        }
        private void OnSearchContactCommand()
        {
            var founContacts = GroupNameList.Where(found =>
            found.UserName.Contains(TextSearch) ||
            found.EmployeeName.Contains(TextSearch)
            ).ToList();

            if (founContacts.Count > 0)
            {
                GroupNameList.Clear();
                foreach (var contact in founContacts)
                {
                    GroupNameList.Add(contact);
                }
            }
        }

        private GroupList selectedgroupname;
        public GroupList SelectedGroupname
        {
            get => selectedgroupname;
            set
            {
                selectedgroupname = value;
                DisplayGroupMember();
                OnPropertyChanged();
            }
        }



        private readonly IGroupServices _groupServices;
        public DisplayGroupViewModel(IGroupServices groupServices)
        {
            _groupServices = groupServices;
            DisplayGroupMember();
            SearchEmptyLoadContactCommand = new Command(async ()=> await LoadCollectionContacts());
        }

        private async Task LoadCollectionContacts()
        {
            GroupNameList.Clear();
            var contacts = await _groupServices.DisplayGroupbyGroupName(selectedgroupname.GroupName);
            foreach (var contact in contacts)
            {
                GroupNameList.Add(contact);
            }
        }
        [RelayCommand]
        public async void DisplayGroupMember()
        {
            if (IsBusy) { return; }

            IsBusy = true;
            var responsefromdisplaygrp = await _groupServices.DisplayGroup();
            var res1 = GroupTitleList.Where(f => f.GroupName.Equals("Unassigned")).FirstOrDefault();
            if (res1 == null)
            {
                GroupTitleList.Add(new GroupList
                {
                    GroupName = "Unassigned",
                });
            }

            if (responsefromdisplaygrp.Count > 0)
            {
                foreach (var grp in responsefromdisplaygrp)
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
                    Numworker = 0;
                }
                var response = await _groupServices.DisplayGroupFromUsers();

                foreach (var grp in response)
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

                if (GroupNameList != null)
                {
                    Cansee = true;
                }
            }

            Numworker = GroupNameList.Count();
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
            PopupPages p = new PopupPages();
            Application.Current.MainPage.ShowPopup(p);

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
            p.Close();
        }
    }
}