using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.ClerkDTO
{
    public class GroupScheduleModel : ObservableObject
    {
        private string groupName;
        private string daymonthyear;
        private string onoff;
        private bool status;
        private Color color;
        private int paxs;
        private bool isselected;
        private double hours;

        public int Paxs
        {
            get => paxs;
            set => SetProperty(ref paxs, value);
        }
        public double Hours
        {
            get => hours;
            set => SetProperty(ref hours, value);
        }
        public string GroupName
        {
            get => groupName;
            set => SetProperty(ref groupName, value);
        }
        public bool Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }        
        public bool IsSelected
        {
            get => isselected;
            set => SetProperty(ref isselected, value);
        }
        public string DayMonthYear
        {
            get => daymonthyear;
            set => SetProperty(ref daymonthyear, value);
        }
        public string OnOff
        {
            get => onoff;
            set => SetProperty(ref onoff, value);
        }
        public Color Color
        {
            get => color;
            set => SetProperty(ref color, value);
        }
    }
}
