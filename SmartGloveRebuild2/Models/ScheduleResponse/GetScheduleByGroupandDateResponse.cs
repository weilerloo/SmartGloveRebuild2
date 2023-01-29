using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.ScheduleResponse
{
    public class GetScheduleByGroupandDateResponse
    {
        public string UserName { get; set; }
        public string EmployeeNumber { get; set; }
        public string GroupName { get; set; }
        public double Hours { get; set; }
        public int Paxs { get; set; }
        public int AvailablePaxs { get; set; }
        public string Remarks { get; set; }
        public bool Status { get; set; }
        public string DayMonthYear { get; set; }

    }
}
