using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.Views.Admin;
using System;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartGloveRebuild2.Models.Group;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SmartGloveRebuild2.Models;

namespace SmartGloveRebuild2.ViewModels.Admin
{
    public partial class HRGenerateReportViewModel : BaseViewModel
    {
        private readonly IScheduleServices _scheduleServices;
        private readonly IGroupServices _groupServices;

        private DateTime selectedmindaymonthyear;
        public DateTime Selectedmindaymonthyear
        {
            get => selectedmindaymonthyear;
            set
            {
                selectedmindaymonthyear = value;
                OnPropertyChanged();
            }
        }

        private DateTime selectedmaxdaymonthyear;
        public DateTime Selectedmaxdaymonthyear
        {
            get => selectedmaxdaymonthyear;
            set
            {
                selectedmaxdaymonthyear = value;
                OnPropertyChanged();
            }
        }

        private GroupList selectedDepartment;
        public GroupList SelectedDepartment
        {
            get => selectedDepartment;
            set
            {
                selectedDepartment = value;
                GetScheduleByGroup();
                OnPropertyChanged();
            }
        }

        private GroupList selectedGroup;
        public GroupList SelectedGroup
        {
            get => selectedGroup;
            set
            {
                selectedGroup = value;
                GetScheduleByEmployee();
                OnPropertyChanged();
            }
        }

        private GroupList selectedEmployee;
        public GroupList SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = value;
                OnPropertyChanged();
            }
        }

        [ObservableProperty]
        bool isRefreshing;
        public ObservableCollection<GroupList> GroupTitleList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> DepartmentList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> FromGroupList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> EmployeeList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> FinalList { get; set; } = new ObservableCollection<GroupList>();

        public HRGenerateReportViewModel(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
        }
        //var response = await _scheduleServices.GetScheduleLogsByGroupandDate(new Models.Schedule.GetSchedulebyGroupandDateDTO
        //{
        //    GroupName = selecteddaymonthyear.GroupName,
        //    ScheduleDate = selecteddaymonthyear.DayMonthYear,
        //}); 
        //var response = await _scheduleServices.GetScheduleLogsByGroupandDate(new Models.Schedule.GetSchedulebyGroupandDateDTO
        //{
        //    GroupName = selecteddaymonthyear.GroupName,
        //    ScheduleDate = selecteddaymonthyear.DayMonthYear,
        //});

        public async void GetScheduleByDepartment()
        {
            if (IsBusy) { return; }
            IsBusy = true;

            if (DepartmentList != null)
            {
                DepartmentList.Clear();
            }

            var response = await _groupServices.DisplayGroupFromUsers();
            foreach (var item in response)
            {
                if (DepartmentList.Count == 0)
                {
                    DepartmentList.Add(new GroupList
                    {
                        Department = item.Department,
                    });
                }
                foreach (var department in DepartmentList)
                {
                    if (item.Department == department.Department)
                    {
                        continue;
                    }
                    else
                    {
                        DepartmentList.Add(new GroupList
                        {
                            Department = item.Department,
                        });
                    }
                }
            }
            IsBusy = false;
        }

        public async void GetScheduleByGroup()
        {
            if (IsBusy || selectedGroup == null) { return; }
            IsBusy = true;
            var response = await _groupServices.DisplayGroupFromUsers();
            foreach (var item in response)
            {
                if (FromGroupList.Count == 0)
                {
                    FromGroupList.Add(new GroupList
                    {
                        GroupName = item.GroupName,
                    });
                }
                if (selectedGroup.GroupName == item.GroupName)
                {
                    foreach (var groups in FromGroupList)
                    {
                        if (item.GroupName == groups.GroupName)
                        {
                            continue;
                        }
                        else
                        {
                            FromGroupList.Add(new GroupList
                            {
                                GroupName = item.GroupName,
                            });
                        }
                    }
                }
            }
            IsBusy = false;
        }

        public async void GetScheduleByEmployee()
        {
            if (IsBusy || selectedEmployee == null) { return; }
            IsBusy = true;
            var response = await _groupServices.DisplayGroupFromUsers();
            foreach (var item in response)
            {
                if (EmployeeList.Count == 0)
                {
                    EmployeeList.Add(new GroupList
                    {
                        UserName = item.UserName,
                    });
                }
                if (selectedEmployee.UserName == item.UserName)
                {
                    foreach (var employee in EmployeeList)
                    {
                        if (item.UserName == employee.UserName)
                        {
                            continue;
                        }
                        else
                        {
                            EmployeeList.Add(new GroupList
                            {
                                UserName = item.UserName,
                            });
                        }
                    }
                }
            }
            IsBusy = false;
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public async void ConfirmGenerate()
        {
            if (IsBusy == true) { return; }

            #region GenerateByselectedDepartment

            if (selectedDepartment == null)
            {
                await Shell.Current.DisplayAlert("Messages", "Please select a Department First.", "OK");
                return;
            }
            else
            {
                //var mindate = selectedmindaymonthyear.ToString("d/m/yyyy");
                //var maxdate = selectedmaxdaymonthyear.ToString("d/m/yyyy");
                foreach (DateTime day in EachDay(selectedmindaymonthyear, selectedmaxdaymonthyear))
                {
                    var getfromDep = await _scheduleServices.GetScheduleLogsByDepartmentandDate(new Models.Schedule.GetSchedulebyGroupandDateDTO
                    {
                        GroupName = selectedDepartment.Department,
                        ScheduleDate = day.ToString("d/M/yyyy"),
                    });
                    if (getfromDep != null)
                    {

                    }
                }
            }


            #endregion

            #region GenerateByselectedGroup




            #endregion

            #region GenerateByselectedEmployee




            #endregion



        }


        #region PickerList
        [RelayCommand]
        public async void DisplayGroupMember()
        {
            if (IsBusy) { return; }

            IsBusy = true;
            var response = await _groupServices.DisplayGroupFromUsers();

            if (response.Count > 0)
            {
                foreach (var grp in response)
                {
                    var res = GroupTitleList.Where(f => f.GroupName.Equals(grp.GroupName)).FirstOrDefault();
                    if (res != null)
                    {
                        continue;
                    }
                    else
                    {
                        if (grp.GroupName == "Unassigned")
                        {
                            continue;
                        }
                        else
                        {
                            GroupTitleList.Add(new GroupList
                            {
                                GroupName = grp.GroupName,
                                SelectedIndex = GroupTitleList.IndexOf(new GroupList
                                {
                                    GroupName = grp.GroupName,
                                }),
                            });

                        }
                    }
                }
            }
            IsRefreshing = false;
            IsBusy = false;
        }
        #endregion

        [RelayCommand]
        private async void HRGenerateReport()
        {
            await Shell.Current.GoToAsync(nameof(GenerateReportPage));
        }

        void ComposeHeader(QuestPDF.Infrastructure.IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(12).SemiBold().FontColor(QuestPDF.Helpers.Colors.Blue.Medium);
            byte[] imageData = File.ReadAllBytes("C:\\Users\\Intern 2\\source\\repos\\SmartGloveRebuild2\\SmartGloveRebuild2\\Resources\\Images\\checkboxblankoutline.png");

            container
                .Row(row =>
            {
                row.ConstantItem(50).Column(column =>
                {
                    column.Item().Image(imageData, ImageScaling.FitArea);

                });
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Overtime Requisition Form").FontSize(20).Style(titleStyle);

                    column.Item()
                    .Text(text =>
                    {
                        text.Span("SMART GLOVE CORPORATION SDN BHD").SemiBold();
                        //text.Span($"{Model.IssueDate:d}");
                    })
                    
                    ;                    
                    
                    column.Item().Text(text =>
                    {
                        text.Span("PLATINIUM GLOVE INDUSTRIES SDN BHD").SemiBold();
                        //text.Span($"{Model.IssueDate:d}");
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("RICH CONTRACTS SDN BHD").SemiBold();
                        //text.Span($"{Model.DueDate:d}");
                    });
                });
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("").Style(titleStyle);

                    column.Item().Text(text =>
                    {
                        text.Span("GX CORPORATION SDN BHD(GX1)").SemiBold();
                        //text.Span($"{Model.IssueDate:d}");
                    });                    
                    
                    column.Item().Text(text =>
                    {
                        text.Span("GX CORPORATION SDN BHD(GX2)").SemiBold();
                        //text.Span($"{Model.IssueDate:d}");
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("GX CORPORATION SDN BHD(GX3)").SemiBold();
                        //text.Span($"{Model.DueDate:d}");
                    });
                });

            });
        }

        void ComposeContent(QuestPDF.Infrastructure.IContainer container)
        {
            container
                .PaddingVertical(40)
                .Height(250)
                .Background(QuestPDF.Helpers.Colors.Grey.Lighten3)
                .AlignCenter()
                .AlignMiddle()
                .Text("Content").FontSize(16);
        }

        [RelayCommand]
        public async Task DownloadReport()
        {

#if ANDROID
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(QuestPDF.Helpers.Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            }).GeneratePdf(Path.Combine("/storage/emulated/0/Documents", "hello.pdf"));

            var filepath = "/storage/emulated/0/Documents/hello.pdf";
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filepath),

            });
#elif WINDOWS
                    Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(QuestPDF.Helpers.Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);


                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            })
            
            
            
            
            
            
            .GeneratePdf(Path.Combine(FileSystem.Current.AppDataDirectory, "hello.pdf"));


            var filepath = Path.Combine(FileSystem.Current.AppDataDirectory, "hello.pdf");
                        await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filepath),

            });
#endif
        }
    }
}
