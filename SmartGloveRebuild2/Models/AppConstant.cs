using SmartGloveRebuild2.Controls;
using SmartGloveRebuild2.Views.Dashboard;
using SmartGloveRebuild2.Views.Employee;
using SmartGloveRebuild2.Views.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models
{
    public class AppConstant
    {

        public async static Task AddFlyoutMenusDetails()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();

            var studentDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(EmployeeDashboardPage)).FirstOrDefault();
            if (studentDashboardInfo != null) AppShell.Current.Items.Remove(studentDashboardInfo);

            var teacherDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(SupervisorDashboardPage)).FirstOrDefault();
            if (teacherDashboardInfo != null) AppShell.Current.Items.Remove(teacherDashboardInfo);

            var HODDashboard = AppShell.Current.Items.Where(f => f.Route == nameof(SupervisorDashboardPage)).FirstOrDefault();
            if (HODDashboard != null) AppShell.Current.Items.Remove(HODDashboard);

            var HRDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(HRDashboardPage)).FirstOrDefault();
            if (HRDashboardInfo != null) AppShell.Current.Items.Remove(HRDashboardInfo);            
            


            if (App.UserDetails.RoleID == (int)RoleDetails.Employee)
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard Page",
                    Route = nameof(EmployeeDashboardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                            {
                                new ShellContent
                                {
                                    Icon = Icons.Dashboard,
                                    Title = "Employee Dashboard",
                                    ContentTemplate = new DataTemplate(typeof(EmployeeDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.ScheduleOT,
                                    Title = "Schdedule OT",
                                    ContentTemplate = new DataTemplate(typeof(ScheduleOT)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.RejectedOT,
                                    Title = "Rejected OT",
                                    ContentTemplate = new DataTemplate(typeof(RejectedOT)),
                                },

                            }
                };
                if (!AppShell.Current.Items.Contains(flyoutItem))
                {
                    AppShell.Current.Items.Add(flyoutItem);
                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        AppShell.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Shell.Current.GoToAsync($"//{nameof(EmployeeDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(EmployeeDashboardPage)}");
                    }
                }

            }

            if (App.UserDetails.RoleID == (int)RoleDetails.Supervisor)
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard Page",
                    Route = nameof(SupervisorDashboardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                                new ShellContent
                                {
                                    Icon = Icons.Dashboard,
                                    Title = "Supervisor Dashboard",
                                    ContentTemplate = new DataTemplate(typeof(SupervisorDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.UpdateSlots,
                                    Title = "Update Slots",
                                    ContentTemplate = new DataTemplate(typeof(SupervisorDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.CheckCalendar,
                                    Title = "Check Calendar",
                                    ContentTemplate = new DataTemplate(typeof(SupervisorDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.Groups,
                                    Title = "Groups",
                                    ContentTemplate = new DataTemplate(typeof(SupervisorDashboardPage)),
                                },
                                
                   }
                };

                if (!AppShell.Current.Items.Contains(flyoutItem))
                {
                    AppShell.Current.Items.Add(flyoutItem);
                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        AppShell.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Shell.Current.GoToAsync($"//{nameof(SupervisorDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(SupervisorDashboardPage)}");
                    }
                }
            }

            if (App.UserDetails.RoleID == (int)RoleDetails.HOD)
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard Page",
                    Route = nameof(SupervisorDashboardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                                new ShellContent
                                {
                                    Icon = Icons.Dashboard,
                                    Title = "HOD Dashboard",
                                    ContentTemplate = new DataTemplate(typeof(SupervisorDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.CheckCalendar,
                                    Title = "HOD Check Calendar",
                                    ContentTemplate = new DataTemplate(typeof(SupervisorDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.Groups,
                                    Title = "HOD Groups",
                                    ContentTemplate = new DataTemplate(typeof(SupervisorDashboardPage)),
                                },

                   }
                };

                if (!AppShell.Current.Items.Contains(flyoutItem))
                {
                    AppShell.Current.Items.Add(flyoutItem);
                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        AppShell.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Shell.Current.GoToAsync($"//{nameof(SupervisorDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(SupervisorDashboardPage)}");
                    }
                }
            }

            if (App.UserDetails.RoleID == (int)RoleDetails.HR)
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard Page",
                    Route = nameof(HRDashboardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                                new ShellContent
                                {
                                    Icon = Icons.Dashboard,
                                    Title = "HR Dashboard",
                                    ContentTemplate = new DataTemplate(typeof(HRDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.AboutUs,
                                    Title = "HR Profile",
                                    ContentTemplate = new DataTemplate(typeof(HRDashboardPage)),
                                },
                   }
                };

                if (!AppShell.Current.Items.Contains(flyoutItem))
                {
                    AppShell.Current.Items.Add(flyoutItem);
                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        AppShell.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Shell.Current.GoToAsync($"//{nameof(HRDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(HRDashboardPage)}");
                    }
                }


            }
        }
    }
}
