using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.ClerkDTO
{
    public class UpdateGroupModel
    {
        public string GroupName { get; set; }
        public int Paxs { get; set; }
        public double Hours { get; set; }
        public bool Status { get; set; }
        public string DayMonthYear { get; set; }
    }
}
