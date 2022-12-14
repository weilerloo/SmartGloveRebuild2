using SmartGloveRebuild2.Models.Schedule;
using SmartGloveRebuild2.Models.ScheduleResponse;
using SmartGloveRebuild2.Models.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Services
{
    public interface IScheduleServices
    {
        Task<AddGroupScheduleResponse> GroupAddSchedule(AddGroupScheduleDTO addGroupScheduleDTO);
        Task<EmployeeAddScheduleResponse> EmployeeAddSchedule(EmployeeAddScheduleDTO addScheduleDTO);
        Task<List<GetScheduleResponse>> GetSchedule();
        Task<List<GetScheduleByGroupResponse>> GetSchedulebyGroup(CreateGroupDTO createGroupDTO);
        Task<UpdateScheduleStatusByGroupNameResponse> updateScheduleStatusByGroupName(UpdateScheduleStatusByGroupNameDTO updateScheduleStatusByGroupNameDTO);
        Task<UpdateScheduleStatusByEmployeeNumberResponse> updateScheduleStatusByEmployeeName(UpdateScheduleStatusByEmployeeNumberDTO updateScheduleStatusByEmployeeNumberDTO);
        Task<RejectScheduleResponse> RejectSchedule(RejectScheduleDTO rejectScheduleDTO);
    }
}
