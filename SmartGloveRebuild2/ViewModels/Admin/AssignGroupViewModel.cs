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

namespace SmartGloveRebuild2.ViewModels.Admin
{
    public partial class AssignGroupViewModel : BaseViewModel
    {
        public static ObservableCollection<AssignGroupDTO> GroupList { get; set; } = new ObservableCollection<AssignGroupDTO>();


        [ObservableProperty]
        string existedgroupname;

        [ObservableProperty]
        string existedemployeenumber;

        private readonly IGroupServices _groupService;
        public AssignGroupViewModel(IGroupServices groupService)
        {
            _groupService = groupService;
        }

    }
}