using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Models.ClerkDTO;
using SmartGloveRebuild2.Models.Group;
using SmartGloveRebuild2.Models.GroupResponse;
using SmartGloveRebuild2.Models.Schedule;
using SmartGloveRebuild2.Models.ScheduleResponse;
using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.Views.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.ViewModels.Employee
{
    public partial class RejectViewModel : BaseViewModel
    {
        #region Properties  
        public ObservableCollection<CalendarModel> RejectedList { get; set; } = new ObservableCollection<CalendarModel>();
        public ObservableCollection<CalendarModel> ListCalendar { get; set; } = new ObservableCollection<CalendarModel>();
        public ObservableCollection<CalendarModel> Datename { get; set; } = new ObservableCollection<CalendarModel>();
        public ObservableCollection<CalendarModel> Monthname { get; set; } = new ObservableCollection<CalendarModel>();

        #endregion

        #region Obseravble Property

        [ObservableProperty]
        DateTime now = DateTime.Now;

        [ObservableProperty]
        int month = 1, decreasemonth, increasemonth, year;

        [ObservableProperty]
        String lastcurrentmonth;

        [ObservableProperty]
        bool status = true, isRefreshing;

        public CalendarModel choosenitem;
        public CalendarModel ChoosenItem
        {
            get
            {
                return choosenitem;
            }
            set
            {
                if (choosenitem != value)
                {
                    choosenitem = value;
                }
            }
        }
        #endregion

        private readonly IScheduleServices _scheduleService;
        private readonly Task Init;
        public RejectViewModel(IScheduleServices scheduleServices)
        {
            _scheduleService = scheduleServices;
            DisplayDays();
            this.Init = GetRejectedList();
            Title = "Reject List";
        }

        #region LoadCalendar
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
                        ListCalendar.Add(new CalendarModel
                        {
                            Day = b, // start with the curretn month
                            Month = 12,
                            Year = year,
                        });
                    }  // End of the November

                    for (int m = 1; m <= DateTime.DaysInMonth(year + 1, 1); m++)
                    {
                        ListCalendar.Add(new CalendarModel
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
                        ListCalendar.Add(new CalendarModel
                        {
                            Day = b, // start with the curretn month
                            Month = 11,
                            Year = year,
                        });
                    }  // End of the November

                    for (int m = 1; m <= DateTime.DaysInMonth(year, 12); m++)
                    {
                        ListCalendar.Add(new CalendarModel
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
                        LastCurrentMonth = "January ~ February"
                    });

                    for (int a = 0; a < 7; a++)
                    {
                        //Day of Week
                        string dayoftheweek = new DateTime(year, 1, 21 + a).ToString("ddd");
                        Datename.Add(new CalendarModel
                        {
                            Currentday = dayoftheweek, // 21, 22
                        });
                    } //Day Name

                    for (int c = 21; c <= DateTime.DaysInMonth(year, 1); c++)
                    {
                        ListCalendar.Add(new CalendarModel
                        {
                            Day = c, // start with the curretn month
                            Month = 1,
                            Year = year,
                        });
                    }  //End of December

                    for (int d = 1; d <= DateTime.DaysInMonth(year + 1, 2); d++)
                    {
                        ListCalendar.Add(new CalendarModel
                        {
                            Day = d,
                            Month = 2,
                            Year = year + 1,
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
                        });
                    } //Day Name

                    for (int c = 21; c <= DateTime.DaysInMonth(year - 1, 12); c++)
                    {
                        ListCalendar.Add(new CalendarModel
                        {
                            Day = c, // start with the curretn month
                            Month = 12,
                            Year = year - 1,
                        });
                    }  //End of December

                    for (int d = 1; d <= DateTime.DaysInMonth(year, 1); d++)
                    {
                        ListCalendar.Add(new CalendarModel
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
                        ListCalendar.Add(new CalendarModel
                        {
                            Day = p, // start with the curretn month
                            Month = month,
                            Year = year,
                        });
                    }

                    for (int q = 1; q <= DateTime.DaysInMonth(year, month + 1); q++)
                    {
                        if (month == 1)
                        {
                            break;
                        }
                        ListCalendar.Add(new CalendarModel
                        {
                            Day = q,
                            Month = month + 1,
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
                        });
                    } //Day Week Name

                    for (int r = 21; r <= DateTime.DaysInMonth(year, month - 1); r++)
                    {
                        if (month == 1)
                        {
                            break;
                        }
                        ListCalendar.Add(new CalendarModel
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
                        ListCalendar.Add(new CalendarModel
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
        public async Task DecreaseMonth()
        {
            if (IsBusy) return;
            Datename.Clear();
            Monthname.Clear();
            ListCalendar.Clear();
            RejectedList.Clear();
            ReduceMonth();
            now = DateTime.Now.AddMonths(month); // 1
            DisplayDays();
            await GetRejectedList();
        }



        [RelayCommand]
        public async Task IncreaseMonth()
        {
            if (IsBusy) return;
            Datename.Clear();
            Monthname.Clear();
            ListCalendar.Clear();
            RejectedList.Clear();
            AddMonth();
            now = DateTime.Now.AddMonths(month);
            DisplayDays();
            await GetRejectedList();
        }

        #endregion

        [RelayCommand]
        public async Task GetRejectedList()
        {
            if(IsBusy) return;

            IsBusy = true;
            if (RejectedList != null)
            {
                RejectedList.Clear();
            }
            foreach (var cm in ListCalendar)
            {
                var getEmployeeSchedule = await _scheduleService.GetScheduleByEmployeeNumberandDate(new GetScheduleByEmployeeNumberandDateDTO
                {
                    DayMonthYear = cm.DayMonthYear,
                    EmployeeNumber = App.UserDetails.EmployeeNumber,
                });

                if (getEmployeeSchedule != null)
                {
                    if (getEmployeeSchedule.IsRejected == true)
                    {
                        RejectedList.Add(new CalendarModel
                        {
                            Day = cm.Day,
                            Month = cm.Month,
                            Year = cm.Year,
                            Rejectedreason = getEmployeeSchedule.RejectedReason,
                            IsRejected = getEmployeeSchedule.IsRejected,
                            RejectedBy = getEmployeeSchedule.RejectedBy,
                        });
                    }
                }
            }
            IsRefreshing = false;
            IsBusy = false;
        }
    }
}
