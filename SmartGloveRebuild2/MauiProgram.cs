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