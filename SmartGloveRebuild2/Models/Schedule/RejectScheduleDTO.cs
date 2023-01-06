using System.ComponentModel.DataAnnotations;

namespace SmartGloveRebuild2.Models.Schedule
{
    public class RejectScheduleDTO
    {
        public string DayMonthYear { get; set; }
        public string EmployeeNumber { get; set; }
        public string GroupName { get; set; }
        public bool IsRejected { get; set; } = true;
        public string RejectedReason { get; set; }
        public string RejectedBy { get; set; }
        public DateTime RejectedDate { get; set; }
    }
}
