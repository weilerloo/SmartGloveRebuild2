using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.ScheduleResponse
{
    public class ScheduleLogResponses
    {
        public string EmployeeNumber { get; set; }
        public string DayMonthYear { get; set; }
        public string GroupName { get; set; }
        public string RejectedReason { get; set; }
        public string RejectedDate { get; set; }
        public string RejectedBy { get; set; }
        public double Hours { get; set; }
        public bool IsRejected { get; set; }
        public bool Status { get; set; }
    }
}
