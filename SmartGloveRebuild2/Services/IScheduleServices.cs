using SmartGloveRebuild2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Services
{
    public interface IScheduleServices
    {
        Task<UserListResponseDTO> AddSchedule(UserListResponseDTO userListResponse);
        Task<MainResponse> GetSchedule(GetScheduleDTO getScheduleDTO);
        Task<MainResponse> UpdateGroupStatus(UpdateScheduleStatusDTO ScheduleStatusDTO); //update
        Task<MainResponse> DeleteGroup(DeleteGroupDTO deleteGroupDTO);
        Task<MainResponse> EditGroup(EditGroupDTO editGroupDTO);
        Task<MainResponse> RejectEmployee(RejectDTO rejectDTO);
        Task<MainResponse> GetAllEmployee();
        Task<MainResponse> GetEmployeeByNumber(string studentID);
    }
}
