using SmartGloveRebuild2.ViewModels;
using SmartGloveRebuild2.ViewModels.Dashboard;
using SmartGloveRebuild2.ViewModels.Admin;
using SmartGloveRebuild2.ViewModels.Startup;
using SmartGloveRebuild2.ViewModels.Employee;
using SmartGloveRebuild2.Views.Admin;
using SmartGloveRebuild2.Views.Dashboard;
using SmartGloveRebuild2.Views.Employee;
using SmartGloveRebuild2.Views.Startup;
using CommunityToolkit.Maui;
using SmartGloveRebuild2.Services;
using Microsoft.Maui.LifecycleEvents;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

namespace SmartGloveRebuild2;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();


        builder.Services.AddSingleton<ILoginService, LoginServices>();
        builder.Services.AddSingleton<IScheduleServices, ScheduleServices>();
        builder.Services.AddSingleton<IGroupServices, GroupServices>();
#if WINDOWS
    

builder.ConfigureLifecycleEvents(events =>
        {
            events.AddWindows(wndLifeCycleBuilder =>
            {
                wndLifeCycleBuilder.OnWindowCreated(window =>
                {
                    IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                    AppWindow winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);
                    if(winuiAppWindow.Presenter is OverlappedPresenter p)
                        p.Maximize();
                    else
                    {
                        const int width = 1200;
                        const int height = 800;
                        winuiAppWindow.MoveAndResize(new RectInt32(1920 / 2 - width / 2, 1080 / 2 - height / 2, width, height));
                    }
                });
            });
        });
#endif
        //Views
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddSingleton<LoadingPage>();
        builder.Services.AddTransient<ScheduleOT>();
        builder.Services.AddTransient<AssignGroupPage>();
        builder.Services.AddTransient<CreateGroupPage>();
        builder.Services.AddTransient<GroupPage>();
        builder.Services.AddTransient<RejectedOT>();
        builder.Services.AddTransient<DisplayGroupPage>();
        builder.Services.AddTransient<UpdateSlotsDetails>();
        builder.Services.AddTransient<CheckCalendarPage>();
        builder.Services.AddTransient<UpdateSlotsPage>();
        builder.Services.AddSingleton<GenerateReportPage>();
        //View Models
        builder.Services.AddSingleton<GroupViewModel>();
        builder.Services.AddTransient<AssignGroupViewModel>();
        builder.Services.AddTransient<DisplayGroupViewModel>();
        builder.Services.AddTransient<CreateGroupViewModel>();
        builder.Services.AddTransient<RejectViewModel>();
        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddSingleton<DashboardPageViewModel>();
        builder.Services.AddSingleton<LoadingPageViewModel>();
        builder.Services.AddTransient<UpdateSlotsDetailViewModel>();
        builder.Services.AddTransient<ScheduleViewModel>();
        builder.Services.AddTransient<CheckCalendarViewModel>();
        builder.Services.AddTransient<UpdateSlotsViewModel>();
        return builder.Build();
    }
}