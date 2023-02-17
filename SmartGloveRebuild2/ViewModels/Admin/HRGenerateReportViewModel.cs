using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SmartGloveOvertime.Handlers;
using SmartGloveRebuild2.Models.Group;
using SmartGloveRebuild2.Models.ScheduleResponse;
using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.Views.Admin;
using System.Collections.ObjectModel;
using Colors = QuestPDF.Helpers.Colors;
using IContainer = QuestPDF.Infrastructure.IContainer;

namespace SmartGloveRebuild2.ViewModels.Admin
{
    public partial class HRGenerateReportViewModel : BaseViewModel
    {
        private readonly IScheduleServices _scheduleServices;
        private readonly IGroupServices _groupServices;

        private DateTime selectedmindaymonthyear = DateTime.Now.AddDays(-7);
        public DateTime Selectedmindaymonthyear
        {
            get => selectedmindaymonthyear;
            set
            {
                selectedmindaymonthyear = value;
                OnPropertyChanged();
            }
        }

        private DateTime selectedmaxdaymonthyear = DateTime.Now;
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

        private string grouplistfrompicker;
        public string Grouplistfrompicker
        {
            get => grouplistfrompicker;
            set
            {
                grouplistfrompicker = value;
                OnPropertyChanged();
            }
        }

        [ObservableProperty]
        bool isRefreshing;
        public ObservableCollection<GroupList> GroupTitleList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> DepartmentList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> FromGroupList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> EmployeeList { get; set; } = new ObservableCollection<GroupList>();
        public List<ScheduleLogResponses> FinalList { get; set; } = new List<ScheduleLogResponses>();

        public HRGenerateReportViewModel(IScheduleServices scheduleServices, IGroupServices groupServices)
        {
            _scheduleServices = scheduleServices;
            _groupServices = groupServices;
            GetScheduleByDepartment();
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


        #region PickerList
        [RelayCommand]
        public async void GetScheduleByDepartment()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            PopupPages p = new PopupPages();
            Application.Current.MainPage.ShowPopup(p);
            await Task.Delay(100);

            if (DepartmentList != null)
            {
                DepartmentList.Clear();
                FromGroupList.Clear();
                EmployeeList.Clear();
            }
            var response = await _groupServices.DisplayGroupFromUsers();
            if (response.Count > 0)
            {
                if (DepartmentList.Count > 0)
                {
                    DepartmentList.Clear();
                }
                foreach (var grp in response)
                {
                    var res = DepartmentList.Where(f => f.Department.Equals(grp.Department)).FirstOrDefault();
                    if (res != null)
                    {
                        continue;
                    }
                    else
                    {
                        DepartmentList.Add(new GroupList
                        {
                            Department = grp.Department,
                        });
                    }
                }
            }
            p.Close();
            IsBusy = false;
        }

        public async void GetScheduleByGroup()
        {
            if (IsBusy || selectedDepartment == null) { return; }
            IsBusy = true;
            PopupPages p = new PopupPages();
            Application.Current.MainPage.ShowPopup(p);
            await Task.Delay(100);

            var groupList = new List<string>();

            var response = await _groupServices.DisplayGroupFromUsers();
            if (response.Count > 0)
            {
                if (FromGroupList.Count > 0)
                {
                    FromGroupList.Clear(); 
                    EmployeeList.Clear();
                }

                foreach (var dep in response)
                {
                    if (dep.GroupName == "Unassigned")
                    {
                        continue;
                    }
                    else if (dep.Department == selectedDepartment.Department)
                    {
                        groupList.Add(dep.GroupName);
                    }
                }

                groupList = groupList.Distinct().ToList();

                foreach (var grp in groupList)
                {
                    FromGroupList.Add(new GroupList
                    {
                        GroupName = grp,
                    });
                }
            }
            p.Close();
            IsBusy = false;
        }
        public async void GetScheduleByEmployee()
        {
            if (IsBusy || selectedGroup == null) { return; }
            IsBusy = true;
            PopupPages p = new PopupPages();
            Application.Current.MainPage.ShowPopup(p);
            await Task.Delay(100);

            var employeeList = new List<string>();

            var response = await _groupServices.DisplayGroupFromUsers();
            if (response.Count > 0)
            {
                if (EmployeeList.Count > 0)
                {
                    EmployeeList.Clear();
                }

                foreach (var employee in response)
                {

                    if (employee.Department == selectedDepartment.Department
                        && employee.GroupName == selectedGroup.GroupName)
                    {
                        EmployeeList.Add(employee);
                    }

                }
            }
            p.Close();
            IsBusy = false;
        }
        #endregion

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public async Task<bool> ConfirmGenerate()
        {
            if (IsBusy == true) { return false; }
            IsBusy = true;
            PopupPages p = new PopupPages();
            Application.Current.MainPage.ShowPopup(p);
            await Task.Delay(100);
            if (selectedDepartment == null)
            {
                await Shell.Current.DisplayAlert("Messages", "Please select a Department First or Select Start/End Date.", "OK");
                p.Close();
                IsBusy = false;
                return false;
            }
            else if (selectedDepartment != null && selectedGroup != null && selectedEmployee != null)
            {
                if (FinalList != null)
                {
                    FinalList.Clear();
                }
                var mindate = selectedmindaymonthyear.ToString("d/m/yyyy");
                var maxdate = selectedmaxdaymonthyear.ToString("d/m/yyyy");
                foreach (DateTime day in EachDay(selectedmindaymonthyear, selectedmaxdaymonthyear))
                {
                    var dymday = day.Day;
                    var dymmonth = day.Month;
                    var dymyear = day.ToString("yyyy");
                    var concatenateddmy = dymday + "%2F" + dymmonth + "%2F" + dymyear;
                    var getfromDep = await _scheduleServices.GetScheduleLogsByDepartmentGroupDate(new Models.Schedule.GetScheduleLogsByDepartmentGroupDateDTO
                    {
                        Department = selectedDepartment.Department,
                        GruopName = selectedGroup.GroupName,
                        ScheduleDate = concatenateddmy,
                    });
                    if (getfromDep != null)
                    {
                        foreach (var item in getfromDep)
                        {
                            var getUsername = await _groupServices.DisplayGroupFromUsers();
                            if (getUsername != null)
                            {
                                foreach (var usn in getUsername)
                                {
                                    if (usn.UserName == item.EmployeeNumber)
                                    {
                                        item.UserName = usn.EmployeeName;
                                    }
                                }
                            }
                            if (item.EmployeeNumber == selectedEmployee.UserName && item.GroupName == selectedGroup.GroupName)
                            {
                                DateTime converted = DateTime.ParseExact(item.DayMonthYear, "d/M/yyyy hh:mm:ss tt", null);
                                item.DayMonthYear = converted.ToString("d");
                                FinalList.Add(item);
                            }
                        }
                        Grouplistfrompicker = await CalculateSection();
                    }
                }
                if (FinalList.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Messages", "No Schedule Found. Please change to a valid date.", "OK");
                    p.Close();
                    IsBusy = false;
                    return false;
                }
                p.Close();
                IsBusy = false;
                return true;
            }
            else if (selectedDepartment != null && selectedGroup != null)
            {
                if (FinalList != null)
                {
                    FinalList.Clear();
                }
                var mindate = selectedmindaymonthyear.ToString("d/m/yyyy");
                var maxdate = selectedmaxdaymonthyear.ToString("d/m/yyyy");
                foreach (DateTime day in EachDay(selectedmindaymonthyear, selectedmaxdaymonthyear))
                {
                    var dymday = day.Day;
                    var dymmonth = day.Month;
                    var dymyear = day.ToString("yyyy");
                    var concatenateddmy = dymday + "%2F" + dymmonth + "%2F" + dymyear;
                    var getfromDep = await _scheduleServices.GetScheduleLogsByDepartmentGroupDate(new Models.Schedule.GetScheduleLogsByDepartmentGroupDateDTO
                    {
                        Department = selectedDepartment.Department,
                        GruopName = selectedGroup.GroupName,
                        ScheduleDate = concatenateddmy,
                    });
                    if (getfromDep != null)
                    {
                        foreach (var item in getfromDep)
                        {
                            var getUsername = await _groupServices.DisplayGroupFromUsers();
                            if (getUsername != null)
                            {
                                foreach (var usn in getUsername)
                                {
                                    if (usn.UserName == item.EmployeeNumber)
                                    {
                                        item.UserName = usn.EmployeeName;
                                    }
                                }
                            }
                            if (item.GroupName == selectedGroup.GroupName)
                            {
                                FinalList.Add(item);
                            }
                        }
                        Grouplistfrompicker = await CalculateSection();
                    }
                }
                if (FinalList.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Messages", "No Schedule Found. Please change to a valid date.", "OK");
                    p.Close();
                    IsBusy = false;
                    return false;
                }
                p.Close();
                IsBusy = false;
                return true;
            }
            else if (selectedDepartment != null)
            {
                if (FinalList != null)
                {
                    FinalList.Clear();
                }
                var mindate = selectedmindaymonthyear.ToString("d/m/yyyy");
                var maxdate = selectedmaxdaymonthyear.ToString("d/m/yyyy");
                foreach (DateTime day in EachDay(selectedmindaymonthyear, selectedmaxdaymonthyear))
                {
                    var getfromDep = await _scheduleServices.GetScheduleLogsByDepartmentandDate(new Models.Schedule.GetSchedulebyGroupandDateDTO
                    {
                        GroupName = selectedDepartment.Department,
                        ScheduleDate = day.ToString("d/M/yyyy"),
                    });
                    if (getfromDep != null)
                    {
                        foreach (var item in getfromDep)
                        {
                            var getUsername = await _groupServices.DisplayGroupFromUsers();
                            if (getUsername != null)
                            {
                                foreach (var usn in getUsername)
                                {
                                    if (usn.UserName == item.EmployeeNumber)
                                    {
                                        item.UserName = usn.EmployeeName;
                                    }
                                }
                            }

                            FinalList.Add(item);
                        }
                    }
                }

                if (FinalList.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Messages", "No Schedule Found. Please change to a valid date.", "OK");
                    p.Close();
                    IsBusy = false;
                    return false;
                }
                p.Close();
                IsBusy = false;
                return true;
            }
            else
            {
                p.Close();
                IsBusy = false;
                return false;
            }
        }



        [RelayCommand]
        private async void HRGenerateReport()
        {
            await Shell.Current.GoToAsync(nameof(GenerateReportPage));
        }

        public async Task<string> CalculateSection()
        {
            List<string> GroupsName = new List<string>();
            var GroupsfromDepartment = await _groupServices.DisplayGroupFromUsers();
            if (GroupsfromDepartment != null)
            {
                foreach (var grp in GroupsfromDepartment)
                {
                    if(grp.GroupName == "Unassigned")
                    {
                        continue;
                    }
                    else if (grp.Department == selectedDepartment.Department)
                    {
                        GroupsName.Add(grp.GroupName);
                    }
                }
            }
            GroupsName = GroupsName.Distinct().ToList();
            string Gn = "";
            if (GroupsName.Count > 0)
            {
                foreach (var words in GroupsName)
                {
                    Gn = $" {words} ," + Gn;
                }
            }
            return Gn.Remove(Gn.Length - 2); ;
        }


        public async Task<Stream> imagePth(string filepath)
        {
            var imageStream = await FileSystem.OpenAppPackageFileAsync(filepath);
            return imageStream;
        }

        async Task<IContainer> ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(12).SemiBold().FontColor(Colors.Blue.Medium);
            //var imageStream = await imagePth("planet.png");

            container
                .PaddingLeft(5)
                .PaddingRight(5)
                .Table(async table =>
                {
                    table
                    .ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(20);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.ConstantColumn(20);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Cell().ColumnSpan(6)
                                .AlignCenter()
                                .HeaderEmptyCell("WORKING EXTRA HOURS REQUISITION FORM");
                    table.Cell().CheckBox();
                    table.Cell().ColumnSpan(2).EmptyCell("SMART GLOVE CORPORATION SDN BHD");
                    table.Cell().CheckBox();
                    table.Cell().ColumnSpan(2).EmptyCell("SMART GLOVE INDUSTRIES SDN BHD");
                    table.Cell().CheckBox();
                    table.Cell().ColumnSpan(2).EmptyCell("PLATINIUM GLOVE INDUSTRIES SDN BHD");
                    table.Cell().CheckBox();
                    table.Cell().ColumnSpan(2).EmptyCell("GX CORPORATION SDN BHD(GX2)");
                    table.Cell().CheckBox();
                    table.Cell().ColumnSpan(2).EmptyCell("SIGMA GLOVE INDUSTRIES SDN BHD");
                    table.Cell().CheckBox();
                    table.Cell().ColumnSpan(2).EmptyCell("GX CORPORATION SDN BHD(GX3)");
                    table.Cell().CheckBox();
                    table.Cell().ColumnSpan(2).EmptyCell("SMART GLOVE HOLDINGS BHD");
                    table.Cell().CheckBox();
                    table.Cell().ColumnSpan(2).EmptyCell("SHITAKA CHEMICAL SUPPLIES SDN BHD");


                    if (selectedGroup == null && selectedDepartment != null) //When department, selectgroup is none
                    {
                        table.Cell().Row(6).Column(1).ColumnSpan(2).BoldEmptyCell("DEPARTMENT :");
                        table.Cell().Row(6).Column(2).ColumnSpan(2).PaddingLeft(60).AlignRight().EmptyCell($"{SelectedDepartment.Department}"); // Report Date
                        table.Cell().Row(6).Column(4).ColumnSpan(2).BoldEmptyCell("DATE :");
                        table.Cell().Row(6).Column(5).ColumnSpan(2).PaddingLeft(60).AlignRight().EmptyCell($"{Selectedmindaymonthyear.ToString("d")} ~ {Selectedmaxdaymonthyear.ToString("d")}"); // Report Date
                        table.Cell().Row(7).Column(1).ColumnSpan(2).BoldEmptyCell("SECTION :");
                        table.Cell().Row(7).Column(2).ColumnSpan(2).PaddingLeft(60).AlignRight().EmptyCell($"Unavaible"); // Report Date
                    }
                    else if (selectedGroup != null && selectedDepartment != null)  // When department, and selected group is there
                    {
                        table.Cell().Row(6).Column(1).ColumnSpan(2).BoldEmptyCell("DEPARTMENT :");
                        table.Cell().Row(6).Column(2).ColumnSpan(2).PaddingLeft(60).AlignRight().EmptyCell($"{SelectedDepartment.Department}"); // Report Date
                        table.Cell().Row(6).Column(4).ColumnSpan(2).BoldEmptyCell("DATE :");
                        table.Cell().Row(6).Column(5).ColumnSpan(2).PaddingLeft(60).AlignRight().EmptyCell($"{Selectedmindaymonthyear.ToString("d")} ~ {Selectedmaxdaymonthyear.ToString("d")}"); // Report Date
                        table.Cell().Row(7).Column(1).ColumnSpan(2).BoldEmptyCell("SECTION :");
                        table.Cell().Row(7).Column(2).ColumnSpan(2).PaddingLeft(60).AlignRight().EmptyCell($"{Grouplistfrompicker}"); // Report Date)
                    }
                    else
                    {
                        table.Cell().Row(7).Column(1).ColumnSpan(2).BoldEmptyCell("SECTION :");
                        table.Cell().Row(6).Column(2).ColumnSpan(2).PaddingLeft(60).AlignRight().EmptyCell("Unavaible"); // Report Date
                    }
                    //table.Cell().Column(4).Row(7).CheckBox();
                    //table.Cell().Column(5).ColumnSpan(2).Row(7).EmptyCell("NORMAL DAY");
                    //table.Cell().Column(4).Row(8).CheckBox();
                    //table.Cell().Column(5).ColumnSpan(2).Row(8).EmptyCell("REST DAY");
                    //table.Cell().Column(4).Row(9).CheckBox();
                    //table.Cell().Column(5).ColumnSpan(2).Row(9).EmptyCell("PUBLIC DAY");

                });
            return container;
        }

        async Task<IContainer> ComposeContent(IContainer container)
        {

            var pageSizes = FinalList;

            int counter = 1;

            container
            .PaddingTop(10)
            .PaddingHorizontal(2)
            .MinimalBox()
            .Border(1)
            .Table(table =>
            {
                IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                {
                    return container
                        .Border(1)
                        .BorderColor(Colors.Grey.Lighten1)
                        .Padding(2)
                        .Background(backgroundColor)
                        .AlignCenter()
                        .AlignMiddle();
                }

                IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.White).ShowOnce();


                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(40);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();

                    columns.ConstantColumn(30);
                    columns.ConstantColumn(30);
                    columns.ConstantColumn(30);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {

                    header.Cell().RowSpan(2).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("NO.").FontSize(6);
                    header.Cell().RowSpan(2).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("EMP NO.").FontSize(6);
                    header.Cell().RowSpan(2).ColumnSpan(2).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("NAME").FontSize(6);
                    //header.Cell().RowSpan(2).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("TIME").FontSize(6);

                    header.Cell().ColumnSpan(3).Element(CellStyle).Text("OVERTIME DETAILS").FontSize(6);

                    header.Cell().Row(2).Column(5).Element(CellStyle).Text("HOURS").FontSize(4);
                    header.Cell().Row(2).Column(6).Element(CellStyle).Text("EXCLUDED").FontSize(4);
                    header.Cell().Row(2).Column(7).Element(CellStyle).Text("APPROVED").FontSize(4);

                    header.Cell().ColumnSpan(2).RowSpan(2).Row(1).Column(8).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("DATE").FontSize(6);
                    header.Cell().ColumnSpan(2).RowSpan(2).Row(1).Column(10).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("PURPOSE OF OVERTIME/ REJECTED BY").FontSize(6);

                    // you can extend already existing styles by creating additional methods
                    IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                });

                foreach (var page in pageSizes)
                {


                    table.Cell().Element(CellStyle).Text(counter++).FontSize(6);
                    table.Cell().Element(CellStyle).Text(page.EmployeeNumber).FontSize(6);
                    table.Cell().ColumnSpan(2).Element(CellStyle).Text(page.UserName).FontSize(6);
                    table.Cell().Element(CellStyle).Text(page.Hours).FontSize(6);
                    if (page.IsRejected == true)
                    {
                        table.Cell().Element(CellStyle).Text("\u221A").FontSize(6);
                        table.Cell().Element(CellStyle).Text("").FontSize(6);
                    }
                    else
                    {
                        table.Cell().Element(CellStyle).Text("").FontSize(6);
                        table.Cell().Element(CellStyle).Text("\u221A").FontSize(6);
                    }
                    table.Cell().ColumnSpan(2).Element(CellStyle).Text(page.DayMonthYear).WrapAnywhere(true).FontSize(6);
                    if (page.RejectedBy != null)
                    {
                        table.Cell().ColumnSpan(2).Element(CellStyle).Text(
                            $"Schedule is Rejected by {page.RejectedBy} " +
                            $"because of {page.RejectedReason} " +
                            $"at {page.RejectedDate}.").FontSize(6);
                    }
                    else if (page.Remark != null)
                    {
                        table.Cell().ColumnSpan(2).Element(CellStyle).Text(page.Remark).FontSize(6);
                    }
                    else
                    {
                        table.Cell().ColumnSpan(2).Element(CellStyle).Text("Not Stated.").FontSize(6);

                    }

                }
            });

            return container;
        }

        [RelayCommand]
        public async Task DownloadReport()
        {
            if (await ConfirmGenerate())
            {
                //var imageStream = await imagePth("planet.png");
#if ANDROID
                Document.Create(container =>
                {
                    container.Page(async page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12));
                        await ComposeHeader(page.Header().ShowOnce());
                        await ComposeContent(page.Content());
                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Page ");
                                x.CurrentPageNumber();
                            });
                    });
                }).GeneratePdf(Path.Combine(FileSystem.Current.AppDataDirectory, $"REPORT_OT_{DateTime.Now.ToString("d_M_yyyy")}.pdf"));


                var filepath = Path.Combine(FileSystem.Current.AppDataDirectory, $"REPORT_OT_{DateTime.Now.ToString("d_M_yyyy")}.pdf");
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filepath),

                });
#elif WINDOWS
                    Document.Create(container =>
            {
               container.Page(async page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));
                    await ComposeHeader(page.Header().ShowOnce());
                    await ComposeContent(page.Content());
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            })
            
            
            
            
            
            
            .GeneratePdf(Path.Combine(FileSystem.Current.AppDataDirectory, $"REPORT_OT_{DateTime.Now.ToString("d_M_yyyy")}.pdf"));


            var filepath = Path.Combine(FileSystem.Current.AppDataDirectory, $"REPORT_OT_{DateTime.Now.ToString("d_M_yyyy")}.pdf");
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filepath),

            });
#endif
            }
        }
    }
}