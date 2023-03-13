#if ANDROID
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using SmartGloveRebuild2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: UsesPermission(Manifest.Permission.ReceiveBootCompleted)]
namespace SmartGloveRebuild2.Services
{
    [BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    [IntentFilter(new[] { Intent.ActionDockEvent })]
    [IntentFilter(new[] { Intent.ActionBatteryChanged })]
    public class NotificationBroadcastReciever : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Intent i = new Intent(context, typeof(NotificationBroadcastReciever));
            i.AddFlags(ActivityFlags.NewTask);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                context.StartForegroundService(i);
            }
            else
            {
                context.StartService(i);
            }
        }
    }
}
#endif