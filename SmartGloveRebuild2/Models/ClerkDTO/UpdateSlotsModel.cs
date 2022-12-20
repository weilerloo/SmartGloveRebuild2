using SmartGloveRebuild2.Models.ScheduleResponse;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.ClerkDTO
{
    public class UpdateSlotsModel
    {
        public string Currentday { get; set; }
        public int Day { get; set; }
        public int Currentmonth { get; set; }
        public int Currentyear { get; set; }
        public int Year { get; set; }
        public string LastCurrentMonth { get; set; }
        public string Month => $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Currentmonth)} {" "}";
        public string DayMonthYear { get; set; }
    }
}
