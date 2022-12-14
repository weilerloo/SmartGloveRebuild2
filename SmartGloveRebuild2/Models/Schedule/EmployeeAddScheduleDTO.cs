using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SmartGloveRebuild2.Models.Schedule
{
    public class EmployeeAddScheduleDTO
    {
        public string DayMonthYear
        {
            get => $"{DateValue}";
        }
        public DateTime DateValue { get { return ScheduleDate.Date.ToUniversalTime(); } set { ScheduleDate = value; } }
        public string EmployeeNumber { get; set; }
        public DateTime ScheduleDate { get; set; }
        [MaxLength(20)]
        public string GroupName { get; set; }
        public bool Status { get; set; }
    }


}