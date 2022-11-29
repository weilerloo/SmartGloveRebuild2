using SmartGloveRebuild2.Handlers;
using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Services;
using Microsoft.Maui.Platform;

namespace SmartGloveRebuild2;

public partial class App : Application
{
    public static UserBasicInfo UserDetails;
    public static string Token;
    public static LoginServices employeeService;
    public App()
    {
        InitializeComponent();
        //Border less entry
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
#elif __IOS__
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            }
        });
        MainPage = new AppShell();
    }
}
