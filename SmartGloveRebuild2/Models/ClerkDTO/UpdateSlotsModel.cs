using CommunityToolkit.Mvvm.ComponentModel;
using SmartGloveRebuild2.Models.ScheduleResponse;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.ClerkDTO
{
    public class UpdateSlotsModel : ObservableObject
    {
        private string currentday;
        private int day;
        private int currentmonth;
        private int currentyear;
        private int year;
        private string lastcurrentmonth;
        //private string Month => $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Currentmonth)} {" "}";
        private string daymonthyear;


        public string Currentday
        {
            get => currentday;
            set => SetProperty(ref currentday, value);
        }
        public int Day
        {
            get => day;
            set => SetProperty(ref day, value);
        }
        public int Currentmonth
        {
            get => currentmonth;
            set => SetProperty(ref currentmonth, value);
        }
        public int Currentyear
        {
            get => currentyear;
            set => SetProperty(ref currentyear, value);
        }
        public int Year
        {
            get => year;
            set => SetProperty(ref year, value);
        }
        public string LastCurrentMonth
        {
            get => lastcurrentmonth;
            set => SetProperty(ref lastcurrentmonth, value);
        }
        public string Month => $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Currentmonth)} {" "}";
        public string DayMonthYear
        {
            get => daymonthyear;
            set => SetProperty(ref daymonthyear, value);
        }
    }
}
