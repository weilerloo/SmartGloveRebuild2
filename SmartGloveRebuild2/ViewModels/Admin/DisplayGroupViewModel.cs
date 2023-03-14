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
        public ObservableCollection<GroupList> SearchedGroupList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> GroupTitleList { get; set; } = new ObservableCollection<GroupList>();
        public static ObservableCollection<GroupList> DisplayGroupList { get; set; } = new ObservableCollection<GroupList>();
        public ICommand SearchEmptyLoadContactCommand { get; private set; }

        [ObservableProperty]
        string entrygroupname;

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
        private string textsearch;
        public string TxtSearch
        {
            get => textsearch;
            set
            {
                textsearch = value;
                OnPropertyChanged();
                OnSearchContactCommand();
            }
        }
        internal class IntermediateKey
        {
            public string Id { get; set; }
            public int Value { get; set; }
        }

        internal class IntermediateKeyComparer : IEqualityComparer<IntermediateKey>
        {
            public bool Equals(IntermediateKey x, IntermediateKey y)
            {
                return x.Id == y.Id && x.Value == y.Value;
            }

            public int GetHashCode(IntermediateKey obj)
            {
                return obj.Id.GetHashCode() + obj.Value.GetHashCode();
            }
        }

        private void OnSearchContactCommand()
        {
            //if(SearchedGroupList.ToList().Count > EditGroupList.ToList().Count)
            //{
            //    EditGroupList = SearchedGroupList;
            //}
            var someList = SearchedGroupList.ToList();

            var founContacts = someList.Where(found =>
             found.UserName.Contains(TxtSearch.ToUpper()) ||
             found.EmployeeName.Contains(TxtSearch.ToUpper())
             ).ToList();  //12

            if (founContacts.Count > 0)
            {

                GroupNameList.Clear();
                foreach (var contact in founContacts)
                {
                    GroupNameList.Add(contact); //12
                }
            }
            else
            {
                GroupNameList.Clear();
            }

            var comparer = new IntermediateKeyComparer();
            var result = SearchedGroupList
                .GroupJoin(
                    founContacts,
                    uv => new IntermediateKey { Id = uv.UserName, Value = uv.SelectedIndex },
                    lm => new IntermediateKey { Id = lm.UserName, Value = lm.SelectedIndex },
                    (uv, lm) => new { Value = uv, Lookups = lm },
                    comparer)
                .SelectMany(
                    pair => pair.Lookups.DefaultIfEmpty(),
                    (paired, meta) => new { Value = paired.Value, Lookup = meta })
                .Select(res =>
                {
                    res.Value.SelectedIndex = res.Lookup?.SelectedIndex ?? res.Value.SelectedIndex;
                    return res.Value;
                }); // 53
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
            SearchEmptyLoadContactCommand = new Command(async () => await LoadCollectionContacts());
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
            var list = GroupNameList.OrderBy(f => f.EmployeeName).ToList();
            GroupNameList.Clear();
            foreach (var contact in list)
            {
                GroupNameList.Add(contact); //12
            }
            List<GroupList> originalEnityList = GroupNameList.ToList();  //53, 12
            ObservableCollection<GroupList> bRef = new ObservableCollection<GroupList>(originalEnityList);
            SearchedGroupList = bRef;
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