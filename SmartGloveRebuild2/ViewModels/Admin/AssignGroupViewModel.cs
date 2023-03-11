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
        public IList<CreateGroupDTO> NameGroupList { get; set; } = new List<CreateGroupDTO>();

        public ICommand SearchEmptyLoadContactCommand { get; private set; }

        [ObservableProperty]
        bool isRefreshing, cannotdelete;


        [ObservableProperty]
        GroupList groupList;

        [ObservableProperty]
        string textsearch;


        private readonly IGroupServices _groupService;
        public AssignGroupViewModel(IGroupServices groupService)
        {
            _groupService = groupService;
            DisplayGroupMember();
            SearchEmptyLoadContactCommand = new Command(async () => await LoadCollectionContacts());
        }

        public string TxtSearch
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
            var founContacts = EditGroupList.Where(found =>
            found.UserName.Contains(TxtSearch) ||
            found.EmployeeName.Contains(TxtSearch)
            ).ToList();

            if (founContacts.Count > 0)
            {
                EditGroupList.Clear();
                foreach (var contact in founContacts)
                {
                    EditGroupList.Add(contact);
                }
            }
        }
        private async Task LoadCollectionContacts()
        {
            EditGroupList.Clear();
            var contacts = await _groupService.DisplayGroupFromUsers();
            foreach (var contact in contacts)
            {
                EditGroupList.Add(contact);
            }
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