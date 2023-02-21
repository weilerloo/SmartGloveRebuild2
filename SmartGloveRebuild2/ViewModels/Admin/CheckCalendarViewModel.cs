using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveOvertime.Handlers;
using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Models.ClerkDTO;
using SmartGloveRebuild2.Models.Group;
using SmartGloveRebuild2.Models.Schedule;
using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.Views.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.ViewModels.Admin
{
    public partial class CheckCalendarViewModel : BaseViewModel
    {

        #region Properties
        private readonly IScheduleServices _scheduleServices;
        private readonly IGroupServices _groupServices;

        [ObservableProperty]
        DateTime now = DateTime.Now;

        [ObservableProperty]
        int month = 1, decreasemonth, increasemonth, year;

        [ObservableProperty]
        String lastcurrentmonth;

        private double totalhours;
        public double TotalHours
        {
            get => totalhours;
            set
            {
                totalhours = value;
                OnPropertyChanged(nameof(TotalHours));
            }
        }

        [ObservableProperty]
        bool isRefreshing, status = true, isBooked, isAvailable, isRejected;

        private GroupList selectedgroupname;
        public GroupList SelectedGroupname
        {
            get => selectedgroupname;
            set
            {
                selectedgroupname = value;
                OnPropertyChanged();
                ColorStatus();
            }
        }

        public CalendarModel selectedItem;
        public CalendarModel SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                }
            }
        }


        #endregion

        #region ObservableCollection and List
        public static ObservableCollection<GroupList> RejectList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<CalendarModel> CalendarDetails { get; set; } = new ObservableCollection<CalendarModel>();
        public ObservableCollection<CalendarModel> Items { get; set; } = new ObservableCollection<CalendarModel>();
        public ObservableCollection<CalendarModel> Datename { get; set; } = new ObservableCollection<CalendarModel>();
        public ObservableCollection<CalendarModel> Monthname { get; set; } = new ObservableCollection<CalendarModel>();
        public ObservableCollection<GroupList> GroupNameList { get; set; } = new ObservableCollection<GroupList>();
        public ObservableCollection<GroupList> GroupTitleList { get; set; } = new ObservableCollection<GroupList>();
        #endregion

        public CheckCalendarViewModel(IScheduleServices scheduleServices, IGroupServices groupServices)
        {
            _scheduleServices = scheduleServices;
            _groupServices = groupServices;
            DisplayDays();
            DisplayGroupMember();
            ColorStatus();
            Title = "Check Calendar";
            Items = new ObservableCollection<CalendarModel>();
            SelectedItem = new CalendarModel();

        }

        #region Navigation

        [RelayCommand]
        private async void CheckCalendar()
        {
            await Shell.Current.GoToAsync(nameof(CheckCalendarPage));
        }
        #endregion

        #region Calendar
        public int AddMonth()
        {
            if (decreasemonth < 0)
            {
                status = false;
            }

            if (status == false)
            {
                month = decreasemonth + 1;
                decreasemonth++;
                status = true;
                return month;
            }
            else
            {
                month = increasemonth + 1;
                increasemonth++;
            }
            return month;
        }
        public int ReduceMonth()
        {

            if (increasemonth > 0)
            {
                status = false;
            }

            if (status == false)
            {
                month = increasemonth - 1;
                increasemonth--;
                status = true;
                return month;
            }
            else
            {
                month = decreasemonth - 1;  // month = 2, increasemonth = 2; 
                decreasemonth--;
            }
            return month;
        }

        [RelayCommand]
        public void DisplayDays()
        {
            var month = now.Month;  // Current Date
            year = now.Year; // 2022

            if (month == 12)
            {
                if (DateTime.Now.Day >= 21)
                {
                    Monthname.Add(new CalendarModel
                    {
                        Year = year,  // 2022
                        LastCurrentMonth = "December ~ January" // 11~12
                    });

                    for (int a = 0; a < 7; a++)
                    {
                        //Day of Week
                        string dayoftheweek = new DateTime(year, 12, 21 + a).ToString("ddd");
                        Datename.Add(new CalendarModel
                        {
                            Currentday = dayoftheweek, // 21, 22
                        });
                    }  //Day Name

                    for (int b = 21; b <= DateTime.DaysInMonth(year, 12); b++)
                    {
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = b, // start with the curretn month
                            Month = 12,
                            Year = year,
                        });
                    }  // End of the November

                    for (int m = 1; m <= DateTime.DaysInMonth(year + 1, 1); m++)
                    {
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = m,
                            Month = 1,
                            Year = year + 1,
                        });

                        if (m == 20)
                        {
                            break;
                        }
                    }  // Start of the December
                }
                else
                {
                    Monthname.Add(new CalendarModel
                    {
                        Year = year,  // 2022
                        LastCurrentMonth = "November ~ December" // 11~12
                    });

                    for (int a = 0; a < 7; a++)
                    {
                        //Day of Week
                        string dayoftheweek = new DateTime(year, 11, 21 + a).ToString("ddd");
                        Datename.Add(new CalendarModel
                        {
                            Currentday = dayoftheweek, // 21, 22
                        });
                    }  //Day Name

                    for (int b = 21; b <= DateTime.DaysInMonth(year, 11); b++)
                    {
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = b, // start with the curretn month
                            Month = 11,
                            Year = year,
                        });
                    }  // End of the November

                    for (int m = 1; m <= DateTime.DaysInMonth(year, 12); m++)
                    {
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = m,
                            Month = 12,
                            Year = year,
                        });

                        if (m == 20)
                        {
                            break;
                        }
                    }  // Start of the December
                }

            }  // For December, it will print end of November with start of Decemebr
            else if (month == 1)
            {
                if (DateTime.Now.Day >= 21)
                {
                    Monthname.Add(new CalendarModel
                    {
                        Year = year,
                        LastCurrentMonth = "Januray ~ February"
                    });

                    for (int a = 0; a < 7; a++)
                    {
                        //Day of Week
                        string dayoftheweek = new DateTime(year - 1, 1, 21 + a).ToString("ddd");
                        Datename.Add(new CalendarModel
                        {
                            Currentday = dayoftheweek, // 21, 22
                            Month = 12,
                            Year = year - 1,
                        });
                    } //Day Name

                    for (int c = 21; c <= DateTime.DaysInMonth(year - 1, 1); c++)
                    {
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = c, // start with the curretn month
                            Month = 1,
                            Year = year - 1,
                        });
                    }  //End of December

                    for (int d = 1; d <= DateTime.DaysInMonth(year, 2); d++)
                    {
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = d,
                            Month = 2,
                            Year = year,
                        });

                        if (d == 20)
                        {
                            break;
                        }
                    }  //Start of January
                }
                else
                {
                    Monthname.Add(new CalendarModel
                    {
                        Year = year,
                        LastCurrentMonth = "December ~ January"
                    });

                    for (int a = 0; a < 7; a++)
                    {
                        //Day of Week
                        string dayoftheweek = new DateTime(year - 1, 12, 21 + a).ToString("ddd");
                        Datename.Add(new CalendarModel
                        {
                            Currentday = dayoftheweek, // 21, 22
                            Month = 12,
                            Year = year - 1,
                        });
                    } //Day Name

                    for (int c = 21; c <= DateTime.DaysInMonth(year - 1, 12); c++)
                    {
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = c, // start with the curretn month
                            Month = 12,
                            Year = year - 1,
                        });
                    }  //End of December

                    for (int d = 1; d <= DateTime.DaysInMonth(year, 1); d++)
                    {
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = d,
                            Month = 1,
                            Year = year,
                        });

                        if (d == 20)
                        {
                            break;
                        }
                    }  //Start of January
                }

            } // For January, it will print end of December with start of January
            else
            {
                if (DateTime.Now.Day >= 21)
                {
                    lastcurrentmonth = DateTimeFormatInfo.CurrentInfo.GetMonthName(month) + " ~ " + DateTimeFormatInfo.CurrentInfo.GetMonthName(month + 1);
                    Monthname.Add(new CalendarModel
                    {
                        Year = year,
                        LastCurrentMonth = lastcurrentmonth
                    });


                    for (int g = 0; g < 7; g++)
                    {
                        if (month == 1)
                        {
                            break;
                        }
                        //Day of Week
                        string dayoftheweek = new DateTime(year, month, 21 + g).ToString("ddd");
                        Datename.Add(new CalendarModel
                        {
                            Currentday = dayoftheweek, // 21, 22
                        });
                    } //Day Week Name

                    for (int p = 21; p <= DateTime.DaysInMonth(year, month); p++) //Start with Current Month 21 ~ 31
                    {
                        if (month == 1)
                        {
                            break;
                        }
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = p,
                            Month = month,
                            Year = year,// start with the curretn month
                        });
                    }

                    for (int q = 1; q <= DateTime.DaysInMonth(year, month + 1); q++)
                    {
                        if (month == 1)
                        {
                            break;
                        }
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = q,
                            Month = month,
                            Year = year,
                        });

                        if (q == 20)
                        {
                            break;
                        }
                    }

                }
                else
                {

                    lastcurrentmonth = DateTimeFormatInfo.CurrentInfo.GetMonthName(month - 1) + " ~ " + DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                    Monthname.Add(new CalendarModel
                    {
                        Year = year,
                        LastCurrentMonth = lastcurrentmonth
                    });
                    for (int e = 0; e < 7; e++)
                    {
                        if (month == 1)
                        {
                            break;
                        }
                        //Day of Week
                        string dayoftheweek = new DateTime(year, month - 1, 21 + e).ToString("ddd");
                        Datename.Add(new CalendarModel
                        {
                            Currentday = dayoftheweek, // 21, 22
                            Month = month - 1,
                            Year = year,
                        });
                    } //Day Week Name

                    for (int r = 21; r <= DateTime.DaysInMonth(year, month - 1); r++)
                    {
                        if (month == 1)
                        {
                            break;
                        }
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = r, // 21,22,23,24,25,26,27,28,29,30 start with the last month
                            Month = month - 1,
                            Year = year,
                        });
                    } //Start with Last Month 21 ~ 31 

                    for (int t = 1; t <= DateTime.DaysInMonth(year, month); t++)
                    {
                        if (month == 1)
                        {
                            break;
                        }
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = t,
                            Month = month,
                            Year = year,
                        });

                        if (t == 20)
                        {
                            break;
                        }
                    }
                }
            }
        }

        [RelayCommand]
        public void DecreaseMonth()
        {
            if (IsBusy) return;
            CalendarDetails.Clear();
            Datename.Clear();
            Monthname.Clear();
            ReduceMonth();
            now = DateTime.Now.AddMonths(month); // 1
            DisplayDays();
            ColorStatus();
        }

        [RelayCommand]
        public void IncreaseMonth()
        {
            if (IsBusy) return;
            CalendarDetails.Clear();
            Datename.Clear();
            Monthname.Clear();
            AddMonth();
            now = DateTime.Now.AddMonths(month);
            DisplayDays();
            ColorStatus();
        }

        #endregion

        #region Color
        public async void ColorStatus()
        {
            if (IsBusy)
                return;
            IsBusy = true;
#if ANDROID
            PopupPages p = new PopupPages();
            Application.Current.MainPage.ShowPopup(p);
            if (SelectedGroupname != null)
            {
                if (GroupNameList.Count > 0)
                {
                    GroupNameList.Clear();
                }
                foreach (var cm in CalendarDetails)
                {
                    var GetSchedulebyGroupandDateresponse = await _scheduleServices.GetSchedulebyGroupandDate(new GetSchedulebyGroupandDateDTO
                    {
                        GroupName = SelectedGroupname.GroupName,
                        ScheduleDate = cm.DayMonthYear,
                    });

                    if (GetSchedulebyGroupandDateresponse.Count > 0)
                    {
                        foreach (var getschdule in GetSchedulebyGroupandDateresponse)
                        {
                            if (getschdule.Hours == 0 && getschdule.Paxs == 0)
                            {
                                cm.IsSelected = true;
                                cm.GroupName = selectedgroupname.GroupName;
                                cm.Color = Color.FromArgb("#778899");//grey
                                cm.IsAvailable = false;
                            }
                            else
                            {
                                var checkIsFull = GetSchedulebyGroupandDateresponse.Find(f => f.AvailablePaxs >= f.Paxs);
                                var checkIsNotFull = GetSchedulebyGroupandDateresponse.Find(f => (f.AvailablePaxs / f.Paxs) * 100 <= 80.00);
                                var checkIsOFF = GetSchedulebyGroupandDateresponse.Find(f => f.Status == false);

                                if (checkIsFull != null)
                                {
                                    cm.IsSelected = false;
                                    cm.IsAvailable = true;
                                    cm.Color = Color.FromArgb("#0000FF"); //BLUE
                                    cm.GroupName = selectedgroupname.GroupName;
                                }
                                else if (checkIsOFF != null)
                                {
                                    cm.IsSelected = true;
                                    cm.Color = Color.FromArgb("#778899");
                                    cm.GroupName = selectedgroupname.GroupName;
                                }
                                else if (checkIsNotFull != null)
                                {
                                    cm.IsSelected = false;
                                    cm.IsAvailable = true; //not relevant
                                    cm.Color = Color.FromArgb("#A52A2A");//chocolate
                                    cm.GroupName = selectedgroupname.GroupName;
                                }
                            }
                        }
                    }
                    else
                    {
                        cm.IsSelected = true;
                        cm.GroupName = selectedgroupname.GroupName;
                        cm.Color = Color.FromArgb("#778899");
                        cm.IsAvailable = false;
                    }
                }
            }
            p.Close();
#elif WINDOWS
            if (SelectedGroupname != null)
            {
                if (GroupNameList.Count > 0)
                {
                    GroupNameList.Clear();
                }
                foreach (var cm in CalendarDetails)
                {
                    var GetSchedulebyGroupandDateresponse = await _scheduleServices.GetSchedulebyGroupandDate(new GetSchedulebyGroupandDateDTO
                    {
                        GroupName = SelectedGroupname.GroupName,
                        ScheduleDate = cm.DayMonthYear,
                    });

                    if (GetSchedulebyGroupandDateresponse.Count > 0)
                    {
                        foreach (var getschdule in GetSchedulebyGroupandDateresponse)
                        {
                            if (getschdule.Hours == 0 && getschdule.Paxs == 0)
                            {
                                cm.IsSelected = true;
                                cm.GroupName = selectedgroupname.GroupName;
                                cm.Color = Color.FromArgb("#778899");//grey
                                cm.IsAvailable = false;
                            }
                            else
                            {
                                var checkIsFull = GetSchedulebyGroupandDateresponse.Find(f => f.AvailablePaxs >= f.Paxs);
                                var checkIsNotFull = GetSchedulebyGroupandDateresponse.Find(f => (f.AvailablePaxs / f.Paxs) * 100 <= 80.00);
                                var checkIsOFF = GetSchedulebyGroupandDateresponse.Find(f => f.Status == false);

                                if (checkIsFull != null)
                                {
                                    cm.IsSelected = false;
                                    cm.IsAvailable = true;
                                    cm.Color = Color.FromArgb("#0000FF"); //BLUE
                                    cm.GroupName = selectedgroupname.GroupName;
                                }
                                else if (checkIsOFF != null)
                                {
                                    cm.IsSelected = true;
                                    cm.Color = Color.FromArgb("#778899");
                                    cm.GroupName = selectedgroupname.GroupName;
                                }
                                else if (checkIsNotFull != null)
                                {
                                    cm.IsSelected = false;
                                    cm.IsAvailable = true; //not relevant
                                    cm.Color = Color.FromArgb("#A52A2A");//chocolate
                                    cm.GroupName = selectedgroupname.GroupName;
                                }
                            }
                        }
                    }
                    else
                    {
                        cm.IsSelected = true;
                        cm.GroupName = selectedgroupname.GroupName;
                        cm.Color = Color.FromArgb("#778899");
                        cm.IsAvailable = false;
                    }
                }
            }
#endif
            IsRefreshing = false;
            IsBusy = false;
        }
        #endregion


        #region PickerList
        [RelayCommand]
        public async void DisplayGroupMember()
        {
            if (IsBusy)
                return;
            IsBusy = true;
#if ANDROID
            PopupPages p = new PopupPages();
            Application.Current.MainPage.ShowPopup(p);
            var response = await _groupServices.DisplayGroup();
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

            ColorStatus();
            p.Close();
#elif WINDOWS
            var response = await _groupServices.DisplayGroup();
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

            ColorStatus();
#endif
            IsRefreshing = false;
            IsBusy = false;
        }
        #endregion

        #region SelectADate

        [RelayCommand]
        public async Task ButtonSelected(CalendarModel selectedItem)
        {
            if (IsBusy)
                return;
            IsBusy = true;
#if ANDROID
            PopupPages p = new PopupPages();
            Application.Current.MainPage.ShowPopup(p);
            if (RejectList != null)
            {
                RejectList.Clear();
            }
            if (selectedItem == null || selectedItem.IsSelected == true)
            {
                p.Close();
                IsBusy = false;
                await Shell.Current.DisplayAlert("Messages", "Please choose the Colored Days to Exclude.", "OK");
                return;
            }
            else if (selectedItem.Color == null)
            {
                p.Close();
                IsBusy = false;
                await Shell.Current.DisplayAlert("Messages", "Please choose the Groups First.", "OK");
                return;
            }

            var getUsersname = await _scheduleServices.GetScheduleLogsByGroupandDate(new GetSchedulebyGroupandDateDTO
            {
                GroupName = selectedItem.GroupName,
                ScheduleDate = selectedItem.DayMonthYear,
            });

            var getEmployeeName = await _groupServices.DisplayGroupFromUsers();

            if (getUsersname != null && getEmployeeName != null)
            {

                foreach (var i in getUsersname)
                {
                    foreach (var o in getEmployeeName)
                    {
                        if (o.UserName == i.EmployeeNumber)
                        {
                            RejectList.Add(new GroupList
                            {
                                IsRejected = i.IsRejected,
                                DayMonthYear = selectedItem.DayMonthYear,
                                UserName = i.EmployeeNumber,
                                EmployeeName = o.EmployeeName,
                                GroupName = i.GroupName,
                            });
                        }
                    }
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Messages", "Group Schedule not found with date.", "OK");
                p.Close();
                IsBusy = false;
            }

            await Shell.Current.GoToAsync(nameof(ExclusionListPage), true, new Dictionary<string, object>
            {
            {"CalendarModel", selectedItem
    }
});
            p.Close();

#elif WINDOWS
            if (RejectList != null)
            {
                RejectList.Clear();
            }
            if (selectedItem == null || selectedItem.IsSelected == true)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Messages", "Please choose the Colored Days to Exclude.", "OK");
                return;
            }
            else if (selectedItem.Color == null)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Messages", "Please choose the Groups First.", "OK");
                return;
            }

            var getUsersname = await _scheduleServices.GetScheduleLogsByGroupandDate(new GetSchedulebyGroupandDateDTO
            {
                GroupName = selectedItem.GroupName,
                ScheduleDate = selectedItem.DayMonthYear,
            });

            var getEmployeeName = await _groupServices.DisplayGroupFromUsers();

            if (getUsersname != null && getEmployeeName != null)
            {

                foreach (var i in getUsersname)
                {
                    foreach (var o in getEmployeeName)
                    {
                        if (o.UserName == i.EmployeeNumber)
                        {
                            RejectList.Add(new GroupList
                            {
                                IsRejected = i.IsRejected,
                                DayMonthYear = selectedItem.DayMonthYear,
                                UserName = i.EmployeeNumber,
                                EmployeeName = o.EmployeeName,
                                GroupName = i.GroupName,
                            });
                        }
                    }
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Messages", "Group Schedule not found with date.", "OK");
                IsBusy = false;
            }

            await Shell.Current.GoToAsync(nameof(ExclusionListPage), true, new Dictionary<string, object>
            {
            {"CalendarModel", selectedItem
    }
});
#endif
            IsBusy = false;
        }

    }
#endregion
}

