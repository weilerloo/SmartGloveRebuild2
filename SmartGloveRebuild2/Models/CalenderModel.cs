using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models
{
    public class CalendarModel
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int CurrentGridRow { get; set; }
        public int CurrentGridColumn { get; set; }
        public string Currentday { get; set; }
        public bool IsFull { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsBooked { get; set; }
        public bool IsRejected { get; set; }
        public string LastCurrentMonth { get; set; }
    }
}