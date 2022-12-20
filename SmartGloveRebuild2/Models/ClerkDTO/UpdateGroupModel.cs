using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.ClerkDTO
{
    public class UpdateGroupModel : ObservableObject
    {

        private string groupName;
        private string daymonthyear;
        private bool status;
        private int paxs;
        private double hours;
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
        public int Paxs
        {
            get => paxs;
            set => SetProperty(ref paxs, value);
        }
        public string DayMonthYear
        {
            get => daymonthyear;
            set => SetProperty(ref daymonthyear, value);
        }

    }
}
