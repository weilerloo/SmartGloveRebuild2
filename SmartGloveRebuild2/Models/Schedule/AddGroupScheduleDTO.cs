using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.Schedule
{
    public class AddGroupScheduleDTO
    {
        public string DayMonthYear
        {
            get => $"{DateValue}";
        }
        public DateTime DateValue { get { return ScheduleDate.Date.ToUniversalTime(); } set { ScheduleDate = value; } }
        public string GroupName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public int Paxs { get; set; }
        public double Hours { get; set; }
        public bool Status { get; set; } = true;
    }

}
