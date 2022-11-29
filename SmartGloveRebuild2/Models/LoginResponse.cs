using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public UserBasicInfo UserDetail { get; set; }
    }
}
