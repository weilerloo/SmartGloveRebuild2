using SmartGloveRebuild2.Models.Schedule;
using SmartGloveRebuild2.Models.ScheduleResponse;
using SmartGloveRebuild2.Models.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartGloveRebuild2.Models.ClerkDTO;

namespace SmartGloveRebuild2.Services
{
    public interface IScheduleServices
    {
        Task<AddGroupScheduleResponse> GroupAddSchedule(AddGroupScheduleDTO addGroupScheduleDTO);
        Task<EmployeeAddScheduleResponse> EmployeeAddSchedule(EmployeeAddScheduleDTO addScheduleDTO);
        Task<GetScheduleByGroupResponse> GetScheduleByEmployeeNumberandDate(GetScheduleByEmployeeNumberandDateDTO getScheduleByEmployeeNumberandDateDTO);
        Task<List<GetScheduleByGroupResponse>> GetSchedulebyGroup(CreateGroupDTO createGroupDTO);
        Task<List<GetScheduleByGroupandDateResponse>> GetSchedulebyGroupandDate(GetSchedulebyGroupandDateDTO getSchedulebyGroupandDateDTO);
        Task<List<GetScheduleByGroupandDateResponse>> GetSchedulebyDate(GetSchedulebyDateDTO getSchedulebyDateDTO);
        Task<UpdateScheduleStatusByGroupNameResponse> updateScheduleStatusByGroupName(UpdateScheduleStatusByGroupNameDTO updateScheduleStatusByGroupNameDTO);
        //Task<UpdateScheduleStatusByEmployeeNumberResponse> updateScheduleStatusByEmployeeName(UpdateScheduleStatusByEmployeeNumberDTO updateScheduleStatusByEmployeeNumberDTO);
        Task<RejectScheduleResponse> RejectSchedule(RejectScheduleDTO rejectScheduleDTO);
    }
}
