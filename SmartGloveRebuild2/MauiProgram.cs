﻿using SmartGloveRebuild2.ViewModels;
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

namespace SmartGloveRebuild2;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {

        #if WINDOWS
            // using Microsoft.Maui.LifecycleEvents;
            // #if WINDOWS
            //            using Microsoft.UI;
            //            using Microsoft.UI.Windowing;
            //            using Windows.Graphics;
            // #endif

            builder.ConfigureLifecycleEvents(events =>
                {
                    events.AddWindows(windowsLifecycleBuilder =>
                        {
                            windowsLifecycleBuilder.OnWindowCreated(window =>
                                {
                                    window.ExtendsContentIntoTitleBar = false;
                                    var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                                    var id = Win32Interop.GetWindowIdFromWindow(handle);
                                    var appWindow = AppWindow.GetFromWindowId(id);
                                    switch (appWindow.Presenter)
                                    {
                                        case OverlappedPresenter overlappedPresenter:
                                            overlappedPresenter.SetBorderAndTitleBar(false, false);
                                            overlappedPresenter.Maximize();
                                            break;
                                    }
                                });
                        });
                });
#endif
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();
        builder.Services.AddSingleton<ILoginService, LoginServices>();
        //Views
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<DashboardPage>();
        builder.Services.AddSingleton<LoadingPage>();
        builder.Services.AddSingleton<ScheduleOT>();
        builder.Services.AddSingleton<RejectedOT>();
        builder.Services.AddSingleton<DisplayGroupPage>();
        builder.Services.AddSingleton<CheckCalendarPage>();
        builder.Services.AddSingleton<UpdateSlotsPage>();
        builder.Services.AddSingleton<GenerateReportPage>();
        //View Models
        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddSingleton<DashboardPageViewModel>();
        builder.Services.AddSingleton<LoadingPageViewModel>();
        builder.Services.AddSingleton<ScheduleViewModel>();
        builder.Services.AddSingleton<DisplayGroupViewModel>();
        builder.Services.AddSingleton<CheckCalendarViewModel>();
        builder.Services.AddSingleton<UpdateSlotsViewModel>();
        builder.Services.AddSingleton<SignInViewModel>();
        return builder.Build();
    }
}