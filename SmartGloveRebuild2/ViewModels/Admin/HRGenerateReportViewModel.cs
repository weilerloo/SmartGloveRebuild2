﻿using CommunityToolkit.Maui.Views;
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
            if (App.UserDetails.Role == "EXECUTIVE")
            {
                Title = "Generate Daily Overtime Page";
            }
            else
            {
                Title = "Generate Overtime Page";
            }
            _scheduleServices = scheduleServices;
            _groupServices = groupServices;
            GetScheduleByDepartment();
            GetStorageAccess();
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
            IsBusy = false;
        }
        public async void GetScheduleByGroup()
        {
            if (IsBusy || selectedDepartment == null) { return; }
            IsBusy = true;
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
            IsBusy = false;
        }
        public async void GetScheduleByEmployee()
        {
            if (IsBusy || selectedGroup == null) { return; }
            IsBusy = true;
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
            IsBusy = false;
        }
        #endregion

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public async void GetStorageAccess()
        {
            var status = PermissionStatus.Unknown;
            status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status == PermissionStatus.Granted)
            {
                return;
            }

            if (Permissions.ShouldShowRationale<Permissions.StorageWrite>())
            {
                await Shell.Current.DisplayAlert("Alert", "Storage Permission Required", "OK");
            }

            status = await Permissions.RequestAsync<Permissions.StorageWrite>();

            if (status != PermissionStatus.Granted)
            {
                await Shell.Current.DisplayAlert("Alert", "Storage Permission Required", "OK");
            }
        }

        public async Task<bool> ConfirmGenerate()
        {
            if (IsBusy == true) { return false; }
            try
            {
                IsBusy = true;
#if ANDROID
                PopupPages p = new PopupPages();
                Application.Current.MainPage.ShowPopup(p);

                if (selectedDepartment == null)
                {
                    await Shell.Current.DisplayAlert("Messages", "Please select a Department First or Select Start/End Date.", "OK");
                    IsBusy = false;
                    p.Close();
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
                                    item.DayMonthYear = converted.ToString("d/M/yyyy");
                                    var getRemark = await _scheduleServices.GetSchedulebyGroupandDate(new Models.Schedule.GetSchedulebyGroupandDateDTO
                                    {
                                        GroupName = selectedGroup.GroupName,
                                        ScheduleDate = item.DayMonthYear,
                                    });
                                    foreach (var rmk in getRemark)
                                    {
                                        if (getRemark != null)
                                        {
                                            item.Remark = rmk.Remarks;
                                        }
                                        break;
                                    }
                                    FinalList.Add(item);
                                }
                            }
                            Grouplistfrompicker = await CalculateSection();
                        }
                    }
                    if (FinalList.Count == 0)
                    {
                        await Shell.Current.DisplayAlert("Messages", "No Schedule Found. Please change to a valid date.", "OK");
                        IsBusy = false;
                        p.Close();
                        return false;
                    }
                    IsBusy = false;
                    p.Close();
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
                                DateTime converted = DateTime.ParseExact(item.DayMonthYear, "d/M/yyyy hh:mm:ss tt", null);
                                item.DayMonthYear = converted.ToString("d/M/yyyy");
                                var getRemark = await _scheduleServices.GetSchedulebyGroupandDate(new Models.Schedule.GetSchedulebyGroupandDateDTO
                                {
                                    GroupName = selectedGroup.GroupName,
                                    ScheduleDate = item.DayMonthYear,
                                });
                                foreach (var rmk in getRemark)
                                {
                                    if (getRemark != null)
                                    {
                                        item.Remark = rmk.Remarks;
                                    }
                                    break;
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
                        IsBusy = false;
                        p.Close();
                        return false;
                    }
                    IsBusy = false;
                    p.Close();
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
                                DateTime converted = DateTime.ParseExact(item.DayMonthYear, "d/M/yyyy hh:mm:ss tt", null);
                                item.DayMonthYear = converted.ToString("d/M/yyyy");
                                var getRemark = await _scheduleServices.GetSchedulebyGroupandDate(new Models.Schedule.GetSchedulebyGroupandDateDTO
                                {
                                    GroupName = item.GroupName,
                                    ScheduleDate = item.DayMonthYear,
                                });
                                foreach (var rmk in getRemark)
                                {
                                    if (getRemark != null)
                                    {
                                        item.Remark = rmk.Remarks;
                                    }
                                    break;
                                }
                                FinalList.Add(item);
                            }
                        }
                    }
                    if (FinalList.Count == 0)
                    {
                        await Shell.Current.DisplayAlert("Messages", "No Schedule Found. Please change to a valid date.", "OK");
                        IsBusy = false;
                        p.Close();
                        return false;
                    }
                    IsBusy = false;
                    p.Close();
                    return true;
                }
                else
                {
                    IsBusy = false;
                    p.Close();
                    return false;
                }
                IsBusy = false;
                p.Close();
                return false;
#elif WINDOWS

            if (selectedDepartment == null)
            {
                await Shell.Current.DisplayAlert("Messages", "Please select a Department First or Select Start/End Date.", "OK");
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
                    IsBusy = false;
                    return false;
                }
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
                    IsBusy = false;
                    return false;
                }
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
                Grouplistfrompicker = await CalculateSection();

                if (FinalList.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Messages", "No Schedule Found. Please change to a valid date.", "OK");
                    IsBusy = false;
                    return false;
                }
                IsBusy = false;
                return true;
            }
            else
            {
                IsBusy = false;
                return false;
            }
#endif
                return false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Failed to Confirm Generate Report", "Please Try Again", "Ok");
                return false;
            }
            finally
            {
                IsBusy = false;
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
                    if (grp.GroupName == "Unassigned")
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
                    if (App.UserDetails.Role == "EXECUTIVE")
                    {
                        table.Cell().ColumnSpan(6)
                                    .AlignCenter()
                                    .HeaderEmptyCell("DAILY OVERTIME REQUISITION FORM");
                    }
                    else
                    {
                        table.Cell().ColumnSpan(6)
                                        .AlignCenter()
                                        .HeaderEmptyCell("OVERTIME REQUISITION FORM");
                    }
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


                    if (selectedGroup == null && selectedDepartment != null && Grouplistfrompicker == null) //When department, selectgroup is none, and no group found from department
                    {
                        table.Cell().Row(6).Column(1).ColumnSpan(2).BoldEmptyCell("DEPARTMENT :");
                        table.Cell().Row(6).Column(2).ColumnSpan(2).PaddingLeft(60).AlignCenter().EmptyCell($"{SelectedDepartment.Department}"); // Report Date
                        table.Cell().Row(6).Column(4).ColumnSpan(2).BoldEmptyCell("DATE :");
                        table.Cell().Row(6).Column(5).ColumnSpan(2).PaddingLeft(60).AlignCenter().EmptyCell($"{Selectedmindaymonthyear.ToString("d")} ~ {Selectedmaxdaymonthyear.ToString("d")}"); // Report Date
                        table.Cell().Row(7).Column(1).ColumnSpan(2).BoldEmptyCell("SECTION :");
                        table.Cell().Row(7).Column(2).ColumnSpan(2).PaddingLeft(60).AlignCenter().EmptyCell($"Unavailable"); // Report Date
                    }
                    else if (selectedGroup == null && selectedDepartment != null && Grouplistfrompicker != null)  // When department, and selected group is there
                    {
                        table.Cell().Row(6).Column(1).ColumnSpan(2).BoldEmptyCell("DEPARTMENT :");
                        table.Cell().Row(6).Column(2).ColumnSpan(2).PaddingLeft(60).AlignCenter().EmptyCell($"{SelectedDepartment.Department}"); // Report Date
                        table.Cell().Row(6).Column(4).ColumnSpan(2).BoldEmptyCell("DATE :");
                        table.Cell().Row(6).Column(5).ColumnSpan(2).PaddingLeft(60).AlignCenter().EmptyCell($"{Selectedmindaymonthyear.ToString("d")} ~ {Selectedmaxdaymonthyear.ToString("d")}"); // Report Date
                        table.Cell().Row(7).Column(1).ColumnSpan(2).BoldEmptyCell("SECTION :");
                        table.Cell().Row(7).Column(2).ColumnSpan(2).PaddingLeft(60).AlignCenter().EmptyCell($"{Grouplistfrompicker}"); // Report Date)
                    }
                    else if (selectedGroup != null && selectedDepartment != null)  // When department, and selected group is there
                    {
                        table.Cell().Row(6).Column(1).ColumnSpan(2).BoldEmptyCell("DEPARTMENT :");
                        table.Cell().Row(6).Column(2).ColumnSpan(2).PaddingLeft(60).AlignCenter().EmptyCell($"{SelectedDepartment.Department}"); // Report Date
                        table.Cell().Row(6).Column(4).ColumnSpan(2).BoldEmptyCell("DATE :");
                        table.Cell().Row(6).Column(5).ColumnSpan(2).PaddingLeft(60).AlignCenter().EmptyCell($"{Selectedmindaymonthyear.ToString("d")} ~ {Selectedmaxdaymonthyear.ToString("d")}"); // Report Date
                        table.Cell().Row(7).Column(1).ColumnSpan(2).BoldEmptyCell("SECTION :");
                        table.Cell().Row(7).Column(2).ColumnSpan(2).PaddingLeft(60).AlignCenter().EmptyCell($"{selectedGroup.GroupName}"); // Report Date)
                    }
                    else
                    {
                        table.Cell().Row(7).Column(1).ColumnSpan(2).BoldEmptyCell("SECTION :");
                        table.Cell().Row(6).Column(2).ColumnSpan(2).PaddingLeft(60).AlignCenter().EmptyCell("Unavailable"); // Report Date
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

        [Obsolete]
        async Task<IContainer> ComposeContent(IContainer container)
        {

            var pageSizes = FinalList.OrderBy(n => n.UserName);

            int totalemployee = 0;
            double totalhour = 0;
            int counter = 1;

            container.Column(column =>
            {
                column.Item()  //Table for whole report
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
                    IContainer CellStyle2(IContainer container) => DefaultCellStyle(container, Colors.Grey.Medium);

                    if (App.UserDetails.Role == "EXECUTIVE")
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30); //1
                            columns.RelativeColumn();//2
                            columns.RelativeColumn();//3
                            columns.RelativeColumn();//3,4

                            columns.ConstantColumn(25);//5
                            columns.ConstantColumn(25);   //6                         
                            columns.ConstantColumn(25);      //7                     
                            columns.ConstantColumn(30);          //8                  
                            columns.ConstantColumn(30);             //9               

                            columns.ConstantColumn(25);//10
                            columns.ConstantColumn(25);//11
                            columns.ConstantColumn(25);//12
                            columns.ConstantColumn(40);//13

                            columns.RelativeColumn();//14
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });
                    }  //Format
                    else
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(20);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();

                            columns.ConstantColumn(20);
                            columns.ConstantColumn(30);
                            columns.ConstantColumn(30);
                            columns.ConstantColumn(25);
                            columns.ConstantColumn(25);
                            columns.ConstantColumn(25);
                            columns.ConstantColumn(40);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });
                    }

                    table.Header(header =>
                    {
                        if (App.UserDetails.Role == "EXECUTIVE")
                        {
                            header.Cell().RowSpan(3).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("NO.").FontSize(6); //1
                            header.Cell().RowSpan(3).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("EMP NO.").FontSize(6); //2
                            header.Cell().RowSpan(3).ColumnSpan(2).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("EMP NAME").FontSize(6); //3,4
                            //header.Cell().RowSpan(2).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("TIME").FontSize(6);

                            header.Cell().ColumnSpan(5).Element(CellStyle).Text("OVERTIME DETAILS").FontSize(6);

                            header.Cell().Row(2).RowSpan(2).Column(5).Element(CellStyle).Text("HOURS").FontSize(4);
                            header.Cell().Row(2).RowSpan(2).Column(6).Element(CellStyle).Text("YES/ NO").FontSize(4);
                            header.Cell().Row(2).RowSpan(2).Column(7).ColumnSpan(3).Element(CellStyle).Text("APPROVED BY").FontSize(4);

                            header.Cell().ColumnSpan(4).Row(1).Column(10).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("DAY").FontSize(6);

                            header.Cell().Row(2).RowSpan(2).Column(10).Element(CellStyle).Text("NORMAL DAY").FontSize(4);
                            header.Cell().Row(2).RowSpan(2).Column(11).Element(CellStyle).Text("REST DAY").FontSize(4);
                            header.Cell().Row(2).RowSpan(2).Column(12).Element(CellStyle).Text("PUBLIC HOLIDAY").FontSize(4);
                            header.Cell().Row(2).RowSpan(2).Column(13).Element(CellStyle).Text("DATE").FontSize(4);
                            //header.Cell().ColumnSpan(2).RowSpan(2).Row(1).Column(8).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("DATE").FontSize(6);
                            header.Cell().ColumnSpan(3).RowSpan(3).Row(1).Column(14).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("TIME /REMARKS").FontSize(6);


                            // you can extend already existing styles by creating additional methods
                            IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                        }
                        else
                        {
                            header.Cell().RowSpan(2).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("NO.").FontSize(6);
                            header.Cell().RowSpan(2).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("EMP NO.").FontSize(6);
                            header.Cell().RowSpan(2).ColumnSpan(2).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("EMP NAME").FontSize(6);
                            //header.Cell().RowSpan(2).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("TIME").FontSize(6);

                            header.Cell().ColumnSpan(3).Element(CellStyle).Text("OVERTIME DETAILS").FontSize(6);

                            header.Cell().Row(2).Column(5).Element(CellStyle).Text("HOURS").FontSize(4);
                            //header.Cell().Row(2).Column(6).Element(CellStyle).Text("EXCLUDED").FontSize(4);
                            header.Cell().Row(2).Column(6).ColumnSpan(2).Element(CellStyle).Text("APPROVED BY").FontSize(4);

                            header.Cell().ColumnSpan(4).Row(1).Column(8).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("DAY").FontSize(6);

                            header.Cell().Row(2).Column(8).Element(CellStyle).Text("NORMAL DAY").FontSize(4);
                            header.Cell().Row(2).Column(9).Element(CellStyle).Text("REST DAY").FontSize(4);
                            header.Cell().Row(2).Column(10).Element(CellStyle).Text("PUBLIC HOLIDAY").FontSize(4);
                            header.Cell().Row(2).Column(11).Element(CellStyle).Text("DATE").FontSize(4);
                            //header.Cell().ColumnSpan(2).RowSpan(2).Row(1).Column(8).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("DATE").FontSize(6);
                            header.Cell().ColumnSpan(3).RowSpan(2).Row(1).Column(12).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("TIME /REMARKS").FontSize(6);

                            // you can extend already existing styles by creating additional methods
                            IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                        }

                    });  //Table Header

                    var repeated = pageSizes.DistinctBy(p => p.EmployeeNumber).ToList();
                    totalemployee = repeated.Count();
                    foreach (var page in pageSizes)
                    {
                        if (App.UserDetails.Role == "EXECUTIVE")
                        {
                            //if (page.IsRejected == true)
                            //{
                            //    continue;
                            //}
                            //else
                            //{
                            table.Cell().Element(CellStyle).Text(counter++).FontSize(6);
                            table.Cell().Element(CellStyle).Text(page.EmployeeNumber).FontSize(6);
                            table.Cell().ColumnSpan(2).Element(CellStyle).Text(page.UserName).FontSize(6);
                            if (page.RejectedBy != null)
                            {
                                table.Cell().Element(CellStyle).Text("").FontSize(6);
                                table.Cell().Element(CellStyle).Text("X").FontSize(6);
                            }
                            else
                            {
                                totalhour += page.Hours;
                                table.Cell().Element(CellStyle).Text(page.Hours).FontSize(6);
                                table.Cell().Element(CellStyle).Text("\u221A").FontSize(6);
                            }
                            //table.Cell().Element(CellStyle).Text("HOD").FontSize(6);
                            //table.Cell().Element(CellStyle2).Text("").FontSize(6);
                            table.Cell().ColumnSpan(3).Element(CellStyle).Text("HOD/ PLANT HEAD").FontSize(6);
                            table.Cell().Element(CellStyle).Text("").FontSize(6);
                            table.Cell().Element(CellStyle).Text("").FontSize(6);
                            table.Cell().Element(CellStyle).Text("").FontSize(6);
                            //DateTime converted = DateTime.ParseExact(page.DayMonthYear, "d/M/yyyy hh:mm:ss tt", null);
                            table.Cell().Element(CellStyle).Text(page.DayMonthYear).WrapAnywhere(true).FontSize(6);
                            if (page.RejectedBy != null)
                            {
                                table.Cell().ColumnSpan(3).Element(CellStyle).Text(
                                    $"Schedule is excluded by {page.RejectedBy} " +
                                    $"because of {page.RejectedReason} " +
                                    $"at {page.RejectedDate}.").FontSize(6);
                            }
                            else if (page.Remark != null)
                            {
                                table.Cell().ColumnSpan(3).Element(CellStyle).Text(page.Remark).FontSize(6);
                            }
                            else
                            {
                                table.Cell().ColumnSpan(3).Element(CellStyle).Text("Not Stated.").FontSize(6);

                            }
                            //}
                        }
                        else
                        {
                            if (page.IsRejected == true)
                            {
                                continue;
                            }
                            else
                            {
                                totalhour += page.Hours;
                                table.Cell().Element(CellStyle).Text(counter++).FontSize(6);
                                table.Cell().Element(CellStyle).Text(page.EmployeeNumber).FontSize(6);
                                table.Cell().ColumnSpan(2).Element(CellStyle).Text(page.UserName).FontSize(6);
                                table.Cell().Element(CellStyle).Text(page.Hours).FontSize(6);
                                if (page.IsRejected == true)
                                {
                                    table.Cell().ColumnSpan(2).Element(CellStyle).Text("HOD/ PLANT HEAD").FontSize(6);
                                    //table.Cell().Element(CellStyle2).Text("").FontSize(6);
                                }
                                else
                                {
                                    //table.Cell().Element(CellStyle2).Text("").FontSize(6);
                                    table.Cell().ColumnSpan(2).Element(CellStyle).Text("HOD/ PLANT HEAD").FontSize(6);
                                }
                                table.Cell().Element(CellStyle).Text("").FontSize(6);
                                table.Cell().Element(CellStyle).Text("").FontSize(6);
                                table.Cell().Element(CellStyle).Text("").FontSize(6);
                                table.Cell().Element(CellStyle).Text(page.DayMonthYear).WrapAnywhere(true).FontSize(6);
                                //if (page.RejectedBy != null)
                                //{
                                //    table.Cell().ColumnSpan(2).Element(CellStyle).Text(
                                //        $"Schedule is excluded by {page.RejectedBy} " +
                                //        $"because of {page.RejectedReason} " +
                                //        $"at {page.RejectedDate}.").FontSize(6);
                                //}
                                //else 

                                if (page.Remark != null)
                                {
                                    table.Cell().ColumnSpan(3).Element(CellStyle).Text(page.Remark).FontSize(6);
                                }
                                else
                                {
                                    table.Cell().ColumnSpan(3).Element(CellStyle).Text("Not Stated.").FontSize(6);
                                }
                            }
                        }

                    }  //Table Content


                });

                column.Item()
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
                    //IContainer CellStyle2(IContainer container) => DefaultCellStyle(container, Colors.Grey.Medium);

                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(30); //1
                        columns.RelativeColumn();//2
                        columns.RelativeColumn();//3
                        columns.RelativeColumn();//3,4

                        columns.ConstantColumn(25);//5
                        columns.ConstantColumn(25);//6                         
                        columns.ConstantColumn(25);//7                     
                        columns.ConstantColumn(30);//8                  
                        columns.ConstantColumn(30);//9               

                        columns.ConstantColumn(25);//10
                        columns.ConstantColumn(25);//11
                        columns.ConstantColumn(25);//12
                        columns.ConstantColumn(40);//13

                        columns.RelativeColumn();//14
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });
                    //Format

                    table.Header(header =>
                    {

                        table.Cell().RowSpan(10).ColumnSpan(8).Element(CellStyle).Text($"Total Employee : {totalemployee}").FontSize(10);
                        table.Cell().RowSpan(10).ColumnSpan(8).Element(CellStyle).Text($"Total Hour : {totalhour}").FontSize(10);
                    });

                    //if (App.UserDetails.Role == "EXECUTIVE")
                    //{
                    //    table.Cell().Element(CellStyle).Text("1").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("2").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("3").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("4").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("5").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("6").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("7").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("8").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("9").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("10").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("11").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("12").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("13").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("14").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("15").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("16").FontSize(6);
                    //    //}
                    //}
                    //else
                    //{
                    //    table.Cell().Element(CellStyle).Text("1").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("2").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("3").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("4").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("5").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("6").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("7").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("8").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("9").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("10").FontSize(6);
                    //    table.Cell().Element(CellStyle).Text("11").FontSize(6);
                    //}

                });

                column.Item()//Table for signature
                .ExtendVertical()
                .AlignBottom()
                .PaddingTop(10)
                .PaddingHorizontal(2)
                .MinimalBox()
                .Border(1)
                .Table(table =>
                {
                    if (App.UserDetails.Role == "EXECUTIVE")
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();

                            columns.ConstantColumn(50);
                            columns.ConstantColumn(50);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });
                    }  //Format
                    else
                    {
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
                    }

                    if (App.UserDetails.Role != "EXECUTIVE")
                    {
                        table.Cell().ColumnSpan(11).EmptyCell("");
                        table.Cell().EmptyCell("");
                        table.Cell().ColumnSpan(2).EmptyCell("Signature By :");
                        table.Cell().EmptyCell("");
                        table.Cell().ColumnSpan(3).EmptyCell("Verified By : HOD");
                        table.Cell().EmptyCell("");
                        table.Cell().ColumnSpan(3).EmptyCell("Approved By : PLANT HEAD /COO");
                        table.Cell().EmptyCell("");
                        table.Cell().ColumnSpan(11).EmptyCell("");
                        table.Cell().ColumnSpan(11).EmptyCell("");
                        table.Cell().EmptyCell("");
                        table.Cell().ColumnSpan(2).SignatureSubscriptCell("Name :");
                        table.Cell().EmptyCell("");
                        table.Cell().ColumnSpan(3).SignatureSubscriptCell("Name :");
                        table.Cell().EmptyCell("");
                        table.Cell().ColumnSpan(2).SignatureSubscriptCell("Name :");
                        table.Cell().EmptyCell("");

                        table.Cell().EmptyCell("");
                        table.Cell().ColumnSpan(2).SignatureEmptyCell("Position :");
                        table.Cell().EmptyCell("");
                        table.Cell().ColumnSpan(3).EmptyCell("");
                        table.Cell().EmptyCell("");
                        table.Cell().ColumnSpan(2).EmptyCell("");
                        table.Cell().EmptyCell("");
                        table.Cell().ColumnSpan(11).EmptyCell("");
                    } // Signature 
                });
            });


            return container;
        }

        [RelayCommand]
        public async Task DownloadReport()
        {
            if (await ConfirmGenerate())
            {
                //var imageStream = await imagePth("planet.png");
                //#if ANDROID
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
                            .Row(row =>
                            {

                                row.RelativeItem()
                                    .AlignLeft()
                                    .Padding(0)
                                    .Text($"* This Report generated by {App.UserDetails.EmployeeName} from Smart Glove Overtime Application.*")
                                    .FontSize(10);

                                row.RelativeItem()
                                    .AlignRight()
                                    .Padding(0)
                                    .Text(x =>
                                    {
                                        x.Span("Page ");
                                        x.CurrentPageNumber();
                                    });
                            });
                    });
                }).GeneratePdf(Path.Combine(FileSystem.Current.AppDataDirectory, $"REPORT_OT_{DateTime.Now.ToString("d_M_yyyy")}.pdf"));


                var filepath = Path.Combine(FileSystem.Current.AppDataDirectory, $"REPORT_OT_{DateTime.Now.ToString("d_M_yyyy")}.pdf");
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filepath),

                });
            }
        }
    }
}