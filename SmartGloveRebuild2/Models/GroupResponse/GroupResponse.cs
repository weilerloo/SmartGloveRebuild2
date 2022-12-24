using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.GroupResponse
{
    public class GroupResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public object Content { get; set; }
    }
}
