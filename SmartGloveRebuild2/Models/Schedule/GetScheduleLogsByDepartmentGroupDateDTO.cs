using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.Schedule
{
    public class GetScheduleLogsByDepartmentGroupDateDTO
    {
        public string GroupName { get; set; }
        public string Department { get; set; }
        public string ScheduleDate { get; set; }
    }
}
