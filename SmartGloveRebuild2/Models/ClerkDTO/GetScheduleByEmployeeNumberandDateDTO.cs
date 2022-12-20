using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.ClerkDTO
{
    public class GetScheduleByEmployeeNumberandDateDTO
    {
        public string EmployeeNumber { get; set; }
        public string DayMonthYear { get; set; }
        public DateTime ConvertedDayMonthYear
        {
            get => DateTime.ParseExact(DayMonthYear, "d/M/yyyy", null);

        }
    }
}
