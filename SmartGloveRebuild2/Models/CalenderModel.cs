using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models
{
    public class CalendarModel : ObservableObject
    {

        private int day;
        private int month;
        private int year;
        private string currentday;
        private string rejectedreason;
        private double hours;
        private bool isfull;
        private bool isavailable;
        private bool isbooked;
        private bool isrejected;
        private bool isselected;
        private string lastcurrentmonth;
        private Color color;
        private string daymonthyear;
        private DateTime scheduledate;

        public string GroupName { get; set; }
        public string EmployeeNumber { get; set; }
        public string DayMonthYear
        {
            get => $"{day}/{month}/{year}";
            set => SetProperty(ref daymonthyear, value);
        }
        public DateTime ScheduleDate
        {
            get => scheduledate;
            set => SetProperty(ref scheduledate, value);
        }

        public int Day
        {
            get => day;
            set => SetProperty(ref day, value);
        }

        public int Month
        {
            get => month;
            set => SetProperty(ref month, value);
        }

        public int Year
        {
            get => year;
            set => SetProperty(ref year, value);
        }

        public string Currentday
        {
            get => currentday;
            set => SetProperty(ref currentday, value);
        }

        public string Rejectedreason
        {
            get => rejectedreason;
            set => SetProperty(ref rejectedreason, value);
        }

        public double Hours
        {
            get => hours;
            set => SetProperty(ref hours, value);
        }

        public bool Isfull
        {
            get => isfull;
            set => SetProperty(ref isfull, value);
        }

        public bool IsAvailable
        {
            get => isavailable;
            set => SetProperty(ref isavailable, value);
        }

        public bool IsBooked
        {
            get => isbooked;
            set => SetProperty(ref isbooked, value);
        }
        public bool IsRejected
        {
            get => isrejected;
            set => SetProperty(ref isrejected, value);
        }        
        public bool IsSelected
        {
            get => isselected;
            set => SetProperty(ref isselected, value);
        }
        public string LastCurrentMonth
        {
            get => lastcurrentmonth;
            set => SetProperty(ref lastcurrentmonth, value);
        }
        public Color Color
        {
            get => color;
            set => SetProperty(ref color, value);
        }
    }
}