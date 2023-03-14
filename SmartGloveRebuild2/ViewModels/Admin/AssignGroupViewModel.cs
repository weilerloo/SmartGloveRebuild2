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
using SmartGloveRebuild2.Models.Schedule;
using Microsoft.IdentityModel.Tokens;
using SmartGloveRebuild2.Views.Dashboard;
using System.Windows.Input;

namespace SmartGloveRebuild2.ViewModels.Admin
{
    [QueryProperty(nameof(GroupList), nameof(GroupList))]
    public partial class AssignGroupViewModel : BaseViewModel
    {
        public ObservableCollection<GroupList> EditGroupList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> SearchedGroupList { get; set; } = new ObservableCollection<GroupList>();
        public IList<CreateGroupDTO> NameGroupList { get; set; } = new List<CreateGroupDTO>();

        //public ICommand SearchEmptyLoadContactCommand { get; private set; }

        [ObservableProperty]
        bool isRefreshing, cannotdelete;


        [ObservableProperty]
        GroupList groupList;



        private readonly IGroupServices _groupService;
        public AssignGroupViewModel(IGroupServices groupService)
        {
            _groupService = groupService;
            DisplayGroupMember();
            //SearchEmptyLoadContactCommand = new Command(async () => await LoadCollectionContacts());
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

                EditGroupList.Clear();
                foreach (var contact in founContacts)
                {
                    EditGroupList.Add(contact); //12
                }
            }
            else
            {
                EditGroupList.Clear();
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
        public async Task CheckUnassignedGroup()
        {
            var response = await _groupService.DisplayGroup();
            var create = response.Where(f => f.GroupName.Equals("Unassigned"));

            int count = 0;
            foreach (var s in create)
            {
                if (s != null && s.GroupName.Equals("Unassigned")) count++;
            }


            if (count == 1)
            {
                cannotdelete = true;
            }
            else
            {
                var creategroupUnassigned = await _groupService.CreateGroup(new CreateGroupDTO
                {
                    GroupName = "Unassigned",
                });
            }
        }


        public async void DisplayGroupMember()
        {
            IsBusy = true;
            await CheckUnassignedGroup();

            var response = await _groupService.DisplayGroup();
            if (response.Count > 0)
            {
                foreach (var grp in response)
                {
                    NameGroupList.Add(new CreateGroupDTO
                    {
                        GroupName = grp.GroupName,
                    });
                }
            }


            foreach (var groups in DisplayGroupViewModel.DisplayGroupList)
            {
                EditGroupList.Add(new GroupList
                {
                    GroupName = groups.GroupName,
                    UserName = groups.UserName,
                    EmployeeName = groups.EmployeeName,
                    TotalHour = groups.TotalHour,
                    TitleGroup = NameGroupList,
                    SelectedIndex = NameGroupList.IndexOf(new CreateGroupDTO
                    {
                        GroupName = groups.GroupName,
                    }),
                });
            }
            var list = EditGroupList.OrderBy(f => f.EmployeeName).ToList();
            EditGroupList.Clear();
            foreach (var contact in list)
            {
                EditGroupList.Add(contact); //12
            }
            List<GroupList> originalEnityList = EditGroupList.ToList();  //53, 12
            ObservableCollection<GroupList> bRef = new ObservableCollection<GroupList>(originalEnityList);
            SearchedGroupList = bRef;
            IsBusy = false;
        }

        public async Task CreateUnassigned()
        {

            var newgroup = await _groupService.CreateGroup(new CreateGroupDTO
            {
                GroupName = "Unassigned",
            });
        }


        [RelayCommand]
        public async Task SaveGroupChanges()
        {
            if (IsBusy) { return; }
            IsBusy = true;
            var checkgroup = await _groupService.DisplayGroupbyGroupName("Unassigned");
            if (checkgroup == null)
            {
                await CreateUnassigned();
            }

            foreach (var items in EditGroupList)
            {
                int gnmpindex = items.SelectedIndex;
                var gnmp = items.TitleGroup[gnmpindex];
                if (items.SelectedGroup == null)
                {
                    var response = await _groupService.AssignGroup(new AssignGroupDTO
                    {
                        EmployeeNumber = items.UserName,
                        GroupName = gnmp.GroupName,
                    });
                    if (response.IsSuccess == false)
                    {
                        IsRefreshing = false;
                        IsBusy = false;
                        await Shell.Current.DisplayAlert("Messages", "Internal Error.", "OK");
                        return;
                    }
                }
                else
                {
                    var response = await _groupService.AssignGroup(new AssignGroupDTO
                    {
                        EmployeeNumber = items.UserName,
                        GroupName = items.SelectedGroup.GroupName,
                    });
                    if (response.IsSuccess == false)
                    {
                        IsRefreshing = false;
                        IsBusy = false;
                        await Shell.Current.DisplayAlert("Messages", "Internal Error.", "OK");
                        return;
                    }
                }
            }
            IsRefreshing = false;
            IsBusy = false;
            await Shell.Current.DisplayAlert("Messages", "Group Updated.", "OK");
            await Shell.Current.GoToAsync("../..");
        }

        [RelayCommand]
        public async Task DeleteGroups()
        {
            if (IsBusy) { return; }
            if (GroupList.GroupName == "Unassigned")
            {
                await Shell.Current.DisplayAlert("Messages", "Unable to delete Group 'Unassinged'.", "OK");
                await Shell.Current.GoToAsync("../..");
                IsRefreshing = false;
                IsBusy = false;
                return;
            }
            var action = await Shell.Current.DisplayAlert("Messages", "Are you sure to delete?", "Yes", "No");
            if (action)
            {
                IsBusy = true;
                var checkgroup = await _groupService.DisplayGroupbyGroupName("Unassigned");
                if (checkgroup == null)
                {
                    await CreateUnassigned();
                }

                foreach (var items in EditGroupList)
                {
                    int gnmpindex = items.SelectedIndex;
                    var gnmp = items.TitleGroup[gnmpindex];
                    if (items.SelectedGroup == null)
                    {
                        var response = await _groupService.AssignGroup(new AssignGroupDTO
                        {
                            EmployeeNumber = items.UserName,
                            GroupName = "Unassigned",
                        });
                        if (response.IsSuccess == false)
                        {
                            await Shell.Current.DisplayAlert("Messages", "Internal Error.", "OK");
                            return;
                        }
                    }
                    else
                    {
                        var response = await _groupService.AssignGroup(new AssignGroupDTO
                        {
                            EmployeeNumber = items.UserName,
                            GroupName = "Unassigned",
                        });
                        if (response.IsSuccess == false)
                        {
                            await Shell.Current.DisplayAlert("Messages", "Internal Error.", "OK");
                            return;
                        }
                    }
                }
                foreach (var ies in EditGroupList)
                {
                    int gnmpindex = ies.SelectedIndex;
                    var gnmp = ies.TitleGroup[gnmpindex];

                    var fordelelte = await _groupService.DeleteGroup(new DeleteGroupDTO
                    {
                        GroupName = gnmp.GroupName,
                    });

                    if (fordelelte.IsSuccess == false)
                    {
                        await Shell.Current.DisplayAlert("Messages", "Unable to delete. Grouping Error.", "OK");

                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Messages", "Group Deleted.", "OK");
                    }
                    break;
                }
                if (EditGroupList.Count() == 0)
                {
                    var fordelelte = await _groupService.DeleteGroup(new DeleteGroupDTO
                    {
                        GroupName = GroupList.GroupName,
                    });
                    if (fordelelte.IsSuccess == false)
                    {
                        await Shell.Current.DisplayAlert("Messages", "Unable to delete. Grouping Error.", "OK");

                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Messages", "Group Deleted.", "OK");
                    }
                }
                await Shell.Current.GoToAsync("../..");
                IsRefreshing = false;
                IsBusy = false;
            }
            else
            {
                return;
            }
        }
    }
}