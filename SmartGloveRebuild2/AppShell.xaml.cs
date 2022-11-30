using SmartGloveRebuild2.ViewModels;
using SmartGloveRebuild2.Views.Admin;
using SmartGloveRebuild2.Views.Employee;
using SmartGloveRebuild2.Views.Startup;

namespace SmartGloveRebuild2;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        this.BindingContext = new AppShellViewModel();
        Routing.RegisterRoute(nameof(DisplayGroupPage), typeof(DisplayGroupPage));
        Routing.RegisterRoute(nameof(CheckCalendarPage), typeof(CheckCalendarPage));
        Routing.RegisterRoute(nameof(UpdateSlotsPage), typeof(UpdateSlotsPage));
        Routing.RegisterRoute(nameof(ScheduleOT), typeof(ScheduleOT));
        Routing.RegisterRoute(nameof(SignInPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(GenerateReportPage), typeof(GenerateReportPage));

    }
}
