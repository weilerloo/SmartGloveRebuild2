using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Models.ClerkDTO;
using SmartGloveRebuild2.Models.Schedule;
using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.Views.Employee;
using System.Collections.ObjectModel;
using System.Globalization;

namespace SmartGloveRebuild2.ViewModels.Employee
{
    public partial class ScheduleViewModel : BaseViewModel
    {
        #region Properties
        private readonly IScheduleServices _scheduleServices;
        private readonly Task Init;

        [ObservableProperty]
        DateTime now = DateTime.Now;

        [ObservableProperty]
        int month = 1, decreasemonth, increasemonth, year;

        [ObservableProperty]
        String lastcurrentmonth;

        private string notesforot;
        public string Notesforot
        {
            get => notesforot;
            set
            {
                notesforot = value;
                OnPropertyChanged(nameof(Notesforot));
            }
        }

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
            Init = ColorStatus();
            Title = "Your OT Schedule";
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
                        LastCurrentMonth = "January ~ February"
                    });

                    for (int a = 0; a < 7; a++)
                    {
                        //Day of Week
                        string dayoftheweek = new DateTime(year, 1, 21 + a).ToString("ddd");
                        Datename.Add(new CalendarModel
                        {
                            Currentday = dayoftheweek, // 21, 22
                            Month = 12,
                            Year = year,
                        });
                    } //Day Name

                    for (int c = 21; c <= DateTime.DaysInMonth(year - 1, 1); c++)
                    {
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = c, // start with the current month
                            Month = 1,
                            Year = year,
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
            if (IsBusy) return;
            CalendarDetails.Clear();
            Datename.Clear();
            Monthname.Clear();
            ReduceMonth();
            now = DateTime.Now.AddMonths(month); // 1
            DisplayDays();
            IsBusy = true;
            await ColorStatus();
            IsRefreshing = false;
            IsBusy = false;
        }

        [RelayCommand]
        public async Task IncreaseMonth()
        {
            if (IsBusy) return;
            CalendarDetails.Clear();
            Datename.Clear();
            Monthname.Clear();
            AddMonth();
            now = DateTime.Now.AddMonths(month);
            DisplayDays();
            IsBusy = true;
            await ColorStatus();
            IsRefreshing = false;
            IsBusy = false;
        }

        #endregion

        #region Color

        [RelayCommand]
        public async Task ColorStatus()
        {
            IsBusy = true;
            TotalHours = 0;
            if (App.UserDetails.GroupName != "Unassigned")
            {
                foreach (var cm in CalendarDetails)
                {
                    var getEmployeeSchedule = await _scheduleServices.GetScheduleByEmployeeNumberandDate(new GetScheduleByEmployeeNumberandDateDTO
                    {
                        DayMonthYear = cm.DayMonthYear,
                        EmployeeNumber = App.UserDetails.EmployeeNumber,
                    });

                    var response = await _scheduleServices.GetSchedulebyGroupandDate(new GetSchedulebyGroupandDateDTO
                    {
                        GroupName = App.UserDetails.GroupName,
                        ScheduleDate = cm.DayMonthYear,
                    });
                    if (getEmployeeSchedule != null)
                    {
                        var convertedstring = getEmployeeSchedule.ScheduleDate.ToString("d/M/yyyy");
                        if (convertedstring == cm.DayMonthYear)
                        {
                            TotalHours = getEmployeeSchedule.Hours + TotalHours;
                        }
                        if (TotalHours >= 104)
                        {
                            Notesforot = "You are NOT Eligible for OT's.";
                        }
                        else
                        {
                            Notesforot = "You are Eligible for OT's.";
                        }
                    }
                    else
                    {
                        Notesforot = "You are Eligible for OT's.";
                    }

                    if (response != null)
                    {
                        DateTime sDate = DateTime.ParseExact(cm.DayMonthYear, "d/M/yyyy", null);
                        var DayDifferences = (DateTime.Now - sDate.Date).Days;
                        var checkIsFull = response.Find(f => f.AvailablePaxs >= f.Paxs && f.Status == true);

                        if (getEmployeeSchedule != null && getEmployeeSchedule.IsRejected == true)
                        {
                            cm.Color = Color.FromArgb("#FF0000");
                            cm.IsRejected = true;
                        }
                        else if (DayDifferences > 7 && response.Count() != 0 && getEmployeeSchedule != null)
                        {
                            foreach (var sdl in response)
                            {
                                cm.Color = Color.FromArgb("#FFA500");
                                cm.IsBooked = true;
                                //cm.Remark = sdl.Remarks;
                                cm.Hours = sdl.Hours;
                                cm.GroupName = sdl.GroupName;
                            }

                        }
                        else if (getEmployeeSchedule != null && getEmployeeSchedule.EmployeeNumber == App.UserDetails.EmployeeNumber)
                        {
                            if (response.Count > 0)
                            {
                                foreach (var sdl in response)
                                {
                                    cm.Color = Color.FromArgb("#FFA500");//orange
                                    cm.IsBooked = true;
                                    //cm.Remark = sdl.Remarks;
                                    cm.Hours = sdl.Hours;
                                    cm.GroupName = sdl.GroupName;
                                }
                            }
                            else
                            {
                                cm.Color = Color.FromArgb("#FFA500");//orange
                                cm.IsBooked = true;
                                //cm.Remark = sdl.Remarks;
                                cm.Hours = getEmployeeSchedule.Hours;
                                cm.GroupName = getEmployeeSchedule.GroupName;
                            }
                        }
                        else if (checkIsFull != null)
                        {
                            cm.Color = Color.FromArgb("#FF0000");//red
                        }
                        else if (DayDifferences < 7 && response.Count() != 0)
                        {
                            foreach (var sdl in response)
                            {
                                DateTime cmdaymonthyear = DateTime.ParseExact(cm.DayMonthYear, "d/M/yyyy", null);

                                if (sdl.Status == true && cmdaymonthyear >= DateTime.Now)
                                {
                                    cm.Hours = sdl.Hours;
                                    cm.Color = Color.FromArgb("#32CD32");//green
                                    cm.IsAvailable = true;
                                    cm.GroupName = sdl.GroupName;
                                }
                                else
                                {
                                    cm.Color = Color.FromArgb("#778899");//grey
                                    cm.IsAvailable = false;
                                }
                            }
                        }
                        else
                        {
                            cm.Color = Color.FromArgb("#778899");//grey
                            cm.IsAvailable = false;
                        }
                    }
                    else
                    {
                        cm.Color = Color.FromArgb("#778899");//grey
                        cm.IsAvailable = false;
                    }
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("You are Unassigned", "Please get a group to be assigned first.", "OK");

            }

            if (TotalHours >= 104)
            {
                Notesforot = "You are NOT Eligible for OT's.";
            }
            else
            {
                Notesforot = "You are Eligible for OT's.";
            }
            IsRefreshing = false;
            IsBusy = false;
        }

        #endregion


        [RelayCommand]
        public async void SubmitButtonSelected(CalendarModel selectedItem)
        {
            if (selectedItem != null && TotalHours <= 104)
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
                            var action = await Shell.Current.DisplayAlert($"You are Scheduling for {selectedItem.DayMonthYear}",
                                $"Schedule Date : {selectedItem.DayMonthYear} \n " +
                                $"Groups :{selectedItem.GroupName} \n" +
                                $"OT Hours is :{selectedItem.Hours} \n" +
                                //$"OT Purposes :{selectedItem.Remark} \n\n" +
                                "Do you want to schedule?", "Yes", "No");



                            if (action)
                            {
                                var tthour = TotalHours + selectedItem.Hours;
                                if(tthour > 104)
                                {
                                    ccm.IsSelected = false;
                                    await Shell.Current.DisplayAlert("Messages","Current OT Hour is exceeding 104 hours. Your schedule has disabled.","OK");
                                    return;
                                }


                                TotalHours = selectedItem.Hours + TotalHours;
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
                            else
                            {
                                ccm.IsSelected = false;
                            }
                        }
                    }

                    if (ccm.IsSelected == false &&
                        ccm.DayMonthYear == selectedItem.DayMonthYear &&
                        ccm.IsBooked == true)
                    {
                        await Shell.Current.DisplayAlert($"Scheduled Overtime {selectedItem.DayMonthYear}",
                            $"You have schedule for {selectedItem.DayMonthYear} \n" +
                            $"Groups :{selectedItem.GroupName} \n" +
                            $"OT Hours is :{selectedItem.Hours} \n"
                            //$"OT Purposes :{selectedItem.Remark} \n"

                            , "OK");
                    }
                }
            }
            else
            {
                if (TotalHours >= 104)
                {
                    Notesforot = "You are NOT Eligible for OT's.";
                }
                else
                {
                    Notesforot = "You are Eligible for OT's.";
                }
                await Shell.Current.DisplayAlert("Alert", "You have exceed the limits of OT. Current actions is disabled.", "OK");
            }
        }


        [RelayCommand]
        public async Task DeleteButtonSelected()
        {
            foreach (var ccm in CalendarDetails)
            {
                if (ccm.IsSelected == true)
                {
                    ccm.Color = Color.FromArgb("#32CD32");
                    ccm.IsSelected = false;
                }
            }
            Items.Clear();
            CalendarDetails.Clear();
            Datename.Clear();
            Monthname.Clear();
            DisplayDays();
            await ColorStatus();
        }

        [RelayCommand]
        public async Task SubmitRequest()
        {
            if (Items.Count != 0)
            {
                foreach (var rqt in Items)
                {
                    var response = await _scheduleServices.EmployeeAddSchedule(new EmployeeAddScheduleDTO
                    {
                        DayMonthYear = rqt.DayMonthYear,
                        EmployeeNumber = App.UserDetails.EmployeeNumber,
                        ScheduleDate = DateTime.ParseExact(rqt.DayMonthYear, "d/M/yyyy", null),
                        GroupName = App.UserDetails.GroupName,
                        Status = true,
                    });
                }
                foreach (var ccm in CalendarDetails)
                {
                    if (ccm.IsSelected == true)
                    {
                        ccm.Color = Color.FromArgb("#FFA500");
                    }
                }
                Items.Clear();
                await ColorStatus();
                await Shell.Current.DisplayAlert("Schedule Successfully", "Please wait the pending OT result.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Schedule Unsuccesfull", "Please select the Available Date.", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}






