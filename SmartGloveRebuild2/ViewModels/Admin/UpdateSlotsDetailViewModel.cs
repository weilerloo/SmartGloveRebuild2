using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.Models.ClerkDTO;
using SmartGloveRebuild2.Models.Group;
using SmartGloveRebuild2.Models.Schedule;
using SmartGloveRebuild2.Models.ScheduleResponse;
using SmartGloveRebuild2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.ViewModels.Admin
{
    [QueryProperty(nameof(UpdateGroupModel), "UpdateGroupModel")]
    public partial class UpdateSlotsDetailViewModel : BaseViewModel
    {
        #region ObservableCollections
        public ObservableCollection<GroupScheduleModel> addedGroupSchedule { get; set; } = new ObservableCollection<GroupScheduleModel>();

        #endregion

        private readonly IScheduleServices _scheduleServices;
        public UpdateSlotsDetailViewModel(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
            foreach (var groups in UpdateSlotsViewModel.GroupSchedule)
            {
                addedGroupSchedule.Add(new GroupScheduleModel
                {
                    GroupName = groups.GroupName,
                    Hours = groups.Hours,
                    Paxs = groups.Paxs,
                    Status = groups.Status,
                });
            }
        }

        [ObservableProperty]
        UpdateGroupModel updateGroupModel;

        [ObservableProperty]
        string daymonthyear, groupname, paxs, status;

        [ObservableProperty]
        double hours;

    }
}
