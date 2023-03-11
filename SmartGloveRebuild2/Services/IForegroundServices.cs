using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Services
{
    public interface IForegroundServices
    {
        void StartMyForegroundService();
        void StopMyForegroundService();
        bool IsForeGroundServiceRunning();
    }
}
