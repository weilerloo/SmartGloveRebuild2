using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.Schedule
{
    public class GetScheduleDTO
    {
        public string EmployeeName { get; set; } = null!;
        public string EmployeeNumber { get; set; } = null!;
        public string Group { get; set; } = null!;
        public string Department { get; set; }
    }
}