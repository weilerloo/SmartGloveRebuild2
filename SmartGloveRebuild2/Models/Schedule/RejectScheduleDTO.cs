using System.ComponentModel.DataAnnotations;

namespace SmartGloveRebuild2.Models.Schedule
{
    public class RejectScheduleDTO
    {
        public string DayMonthYear
        {
            get => $"{DateValue}";
        }
        public DateTime DateValue { get { return RejectedDate.Date.ToUniversalTime(); } set { RejectedDate = value; } }
        public string EmployeeNumber { get; set; }
        public string GroupName { get; set; }
        public bool IsRejected { get; set; } = true;
        public string RejectedReason { get; set; }
        public DateTime RejectedDate { get; set; }
    }
}
