using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.ScheduleResponse
{
    public class GetScheduleByGroupandDateResponse
    {
        public string GroupName { get; set; }
        public double Hours { get; set; } = 0;
        public int Paxs { get; set; } = 0;
        public bool Status { get; set; } = false;


    }
}
