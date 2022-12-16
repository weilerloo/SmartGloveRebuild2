using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SmartGloveRebuild2.Models.Schedule
{
    public class EmployeeAddScheduleDTO
    {
        public string DayMonthYear { get; set; }

        public DateTime ScheduleDate { get; set; }
        public string EmployeeNumber { get; set; }
        [MaxLength(20)]
        public string GroupName { get; set; }
        public bool Status { get; set; }
    }


}