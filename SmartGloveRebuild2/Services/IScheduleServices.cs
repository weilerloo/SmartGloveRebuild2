using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Services
{
    public interface IScheduleServices
    {
        Task<MainResponse> AddSchedule(AddScheduleDTO ScheduleDateDTO);
        Task<MainResponse> UpdateGroupStatus(UpdateScheduleStatusDTO ScheduleStatusDTO); //update
        Task<MainResponse> GetSchedule(GetScheduleDTO getScheduleDTO);
        Task<MainResponse> DeleteGroup(DeleteGroupDTO deleteGroupDTO);
        Task<MainResponse> EditGroup(EditGroupDTO editGroupDTO);
        Task<MainResponse> RejectEmployee(RejectDTO rejectDTO);
        Task<MainResponse> GetAllEmployee();
        Task<MainResponse> GetEmployeeByNumber(string studentID);
    }
}
