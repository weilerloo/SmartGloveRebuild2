using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.Platforms.Android;

namespace SmartGloveRebuild2;

[Activity(Theme = "@style/My.SplashTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    NotificationBroadcastReciever receiver;
    IntentFilter intentFilter;
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Platform.Init(this, savedInstanceState);

#if ANDROID
        Android.Content.Intent intent = new Android.Content.Intent(Android.App.Application.Context, typeof(NotificationForegroundServices));
        Android.App.Application.Context.StartForegroundService(intent);
#endif
        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.PostNotifications) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.PostNotifications }, 0);
        }

        receiver = new();
        intentFilter = new(Intent.ActionBootCompleted);

        intentFilter.AddAction(Intent.ActionPowerDisconnected);
        intentFilter.AddAction(Intent.ActionDockEvent);
        intentFilter.AddAction(Intent.ActionBatteryChanged);
        intentFilter.Priority = (int)IntentFilterPriority.HighPriority;

        LocalNotificationCenter.CreateNotificationChannelGroup(new NotificationChannelGroupRequest
        {
            Group = "Schedule OT",
            Name = "Schedule OT Channel Group",
        });

        LocalNotificationCenter.CreateNotificationChannel(new NotificationChannelRequest
        {
            CanBypassDnd = true,
            Description = "This is a channel from Schedule OT, please do not turn it off.",
            EnableLights = true,
            EnableSound = true,
            EnableVibration = true,
            Group = "Schedule OT",
            Id = "0829",
            Importance = AndroidImportance.High,
            Name = "Schedule Details",
            LockScreenVisibility = AndroidVisibilityType.Public,
        });
    }
}



