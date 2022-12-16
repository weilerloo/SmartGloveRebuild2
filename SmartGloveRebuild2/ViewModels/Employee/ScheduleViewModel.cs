using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Services;
using SmartGloveRebuild2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.ViewModels;
using Microsoft.Maui.Networking;
using System.Diagnostics;
using SmartGloveRebuild2.Views.Employee;
using SmartGloveRebuild2.Models.Group;
using SmartGloveRebuild2.Models.Schedule;
using Newtonsoft.Json.Serialization;

namespace SmartGloveRebuild2.ViewModels.Employee
{
    public partial class ScheduleViewModel : BaseViewModel
    {
        #region Properties
        private readonly IScheduleServices _scheduleServices;


        int buttonGridColumn = 0;
        int buttonGridRow = 0;

        [ObservableProperty]
        DateTime now = DateTime.Now;

        [ObservableProperty]
        int month = 1, decreasemonth, increasemonth, year;

        [ObservableProperty]
        String lastcurrentmonth;

        [ObservableProperty]
        bool isRefreshing, status = true, isBooked, isAvailable, isRejected;


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

        #region ObservableCollection  
        public ObservableCollection<CalendarModel> CalendarDetails { get; set; } = new ObservableCollection<CalendarModel>();
        public ObservableCollection<CalendarModel> Items { get; set; } = new ObservableCollection<CalendarModel>();
        public ObservableCollection<CalendarModel> Datename { get; set; } = new ObservableCollection<CalendarModel>();
        public ObservableCollection<CalendarModel> Monthname { get; set; } = new ObservableCollection<CalendarModel>();

        #endregion

        public ScheduleViewModel(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
            DisplayDays();
            ColorStatus();
            Title = "Your OT Schdule";
            Items = new ObservableCollection<CalendarModel>();
            SelectedItem = new CalendarModel();

        }

        #region Navigation

        [RelayCommand]
        private async void GotoScheduleOT()
        {
            await Shell.Current.GoToAsync(nameof(ScheduleOT));
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
        public async Task DisplayDays()
        {
            var month = now.Month;  // Current Date
            year = now.Year; // 2022

            if (month == 12)
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

            }  // For December, it will print end of November with start of Decemebr
            else if (month == 1)
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
                month++;
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
                            Day = p, // start with the curretn month
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
        public async Task DecreaseMonth()
        {
            IsBusy = true;
            CalendarDetails.Clear();
            Datename.Clear();
            Monthname.Clear();
            ReduceMonth();
            now = DateTime.Now.AddMonths(month); // 1
            DisplayDays();
            ColorStatus();
            IsBusy = false;
            IsRefreshing = false;
        }

        [RelayCommand]
        public async Task IncreaseMonth()
        {
            IsBusy = true;
            CalendarDetails.Clear();
            Datename.Clear();
            Monthname.Clear();
            AddMonth();
            now = DateTime.Now.AddMonths(month);
            DisplayDays();
            ColorStatus();
            IsBusy = false;
            IsRefreshing = false;

        }

        #endregion

        #region Color

        [RelayCommand]
        public async Task ColorStatus()
        {
            if (App.UserDetails.GroupName != "Unassigned")
            {
                foreach (var cm in CalendarDetails)
                {
                    var response = await _scheduleServices.GetSchedulebyGroupandDate(new GetSchedulebyGroupandDateDTO
                    {
                        GroupName = App.UserDetails.GroupName,
                        ScheduleDate = cm.DayMonthYear,
                    });

                    if (response != null)
                    {
                        DateTime sDate = DateTime.ParseExact(cm.DayMonthYear, "dd/MM/yyyy", null);

                        var DayDifferences = (DateTime.Now - sDate.Date).Days;
                        if (DayDifferences > 7 && response.Status == true)
                        {
                            cm.Color = Color.FromArgb("#FFA500");
                            cm.IsBooked = true;

                        }
                        else if (DayDifferences < 7 && response.Status == true)
                        {
                            cm.Color = Color.FromArgb("#32CD32");
                            cm.IsAvailable = true;

                        }
                        else if (response.Status == false)
                        {
                            cm.Color = Color.FromArgb("#FFA07A");
                            cm.IsRejected = true;
                        }
                    }
                    else
                    {
                        cm.Color = Color.FromArgb("#778899");
                        cm.IsAvailable = false;

                    }
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("You are Unassigned", "Please get a group to be assigned first.", "OK");

            }
        }

        #endregion


        [RelayCommand]
        public async Task SubmitButtonSelected(CalendarModel selectedItem)
        {
            if (selectedItem != null)
            {
                foreach (var ccm in CalendarDetails)
                {
                    if (ccm.IsSelected == false &&
                        ccm.DayMonthYear == selectedItem.DayMonthYear &&
                        ccm.IsAvailable == true)
                    {
                        ccm.IsSelected = true;
                        if (ccm.IsSelected == true)
                        {
                            ccm.Color = Color.FromArgb("#006400");
                            Items.Add(new CalendarModel
                            {
                                DayMonthYear = selectedItem.DayMonthYear,
                                Day = selectedItem.Day,
                                Month = selectedItem.Month,
                                Year = selectedItem.Year,
                                IsSelected = true,
                                EmployeeNumber = App.UserDetails.EmployeeName,
                                GroupName = App.UserDetails.GroupName,
                            });
                        }
                    }
                }
            }

        }
    

    [RelayCommand]
    public async Task DeleteButtonSelected()
    {
        IsBusy = true;
        foreach (var ccm in CalendarDetails)
        {
            if (ccm.IsSelected == true)
            {
                ccm.Color = Color.FromArgb("#32CD32");
                ccm.IsSelected = false;
            }
        }
        Items.Clear();
        IsBusy = false;
        IsRefreshing = false;
    }
}

    //try
    //{
    //    foreach (var cm in SubmitOTRequest)
    //    {
    //        var response = await _scheduleServices.EmployeeAddSchedule(new EmployeeAddScheduleDTO
    //        {
    //            DayMonthYear = cm.DayMonthYear,
    //            EmployeeNumber = App.UserDetails.EmployeeNumber,
    //            ScheduleDate = cm.ScheduleDate,
    //            GroupName = App.UserDetails.GroupName,
    //            Status = true,
    //        });

    //        if (response != null)
    //        {
    //            SubmitOTRequest.Add(new CalendarModel
    //            {

    //            });
    //        }
    //        else
    //        {
    //            await Shell.Current.DisplayAlert("Hello", "Takpaye kerja", "OK");
    //        }
    //    }
    //}
    //catch (Exception ex)
    //{


    //}
}






