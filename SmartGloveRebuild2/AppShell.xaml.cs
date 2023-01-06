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
        Routing.RegisterRoute(nameof(AssignGroupPage), typeof(AssignGroupPage));
        Routing.RegisterRoute(nameof(ExclusionListPage), typeof(ExclusionListPage));
        Routing.RegisterRoute(nameof(ExclusionMultipleDatePage), typeof(ExclusionMultipleDatePage));
        Routing.RegisterRoute(nameof(NextReasonRejectListPage), typeof(NextReasonRejectListPage));
        Routing.RegisterRoute(nameof(CreateGroupPage), typeof(CreateGroupPage));
        Routing.RegisterRoute(nameof(GroupPage), typeof(GroupPage));
        Routing.RegisterRoute(nameof(DisplayGroupPage), typeof(DisplayGroupPage));
        Routing.RegisterRoute(nameof(CheckCalendarPage), typeof(CheckCalendarPage));
        Routing.RegisterRoute(nameof(UpdateSlotsPage), typeof(UpdateSlotsPage));
        Routing.RegisterRoute(nameof(UpdateSlotsDetails), typeof(UpdateSlotsDetails));
        Routing.RegisterRoute(nameof(GenerateReportPage), typeof(GenerateReportPage));
        Routing.RegisterRoute(nameof(ScheduleOT), typeof(ScheduleOT));
        Routing.RegisterRoute($"//{nameof(LoadingPage)}/{nameof(LoginPage)}", typeof(LoginPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
    }
}
