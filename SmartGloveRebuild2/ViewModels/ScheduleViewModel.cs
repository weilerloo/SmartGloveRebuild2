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

namespace SmartGloveRebuild2.ViewModels
{
    public partial class ScheduleViewModel : BaseViewModel
    {
        int buttonGridColumn = 0;
        int buttonGridRow = 0;

        [ObservableProperty]
        DateTime now = DateTime.Now;

        [ObservableProperty]
        int month = 1, decreasemonth = 1, year;

        [ObservableProperty]
        String lastcurrentmonth;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        CalendarModel selectedDate;

        #region Properties  
        public ObservableCollection<CalendarModel> CalendarDetails { get; set; } = new ObservableCollection<CalendarModel>();
        public ObservableCollection<CalendarModel> Datename { get; set; } = new ObservableCollection<CalendarModel>();
        public ObservableCollection<CalendarModel> Monthname { get; set; } = new ObservableCollection<CalendarModel>();

        #endregion

        public Command TriggerSceneCommand { get; set; }
        private void TriggerScene(CalendarModel newSelectedItem)
        {
            SelectedDate = newSelectedItem;
        }
        public ScheduleViewModel()
        {
            DisplayDays();
            Title = "Your OT Schdule";
        }

        [RelayCommand]
        private async void GotoScheduleOT()
        {
            await Shell.Current.GoToAsync(nameof(ScheduleOT));
        }



        [RelayCommand]
        public void DisplayDays()
        {
            var month = now.Month;
            year = now.Year;

            if (month == 12)
            {
                Monthname.Add(new CalendarModel
                {
                    Year = year,
                    LastCurrentMonth = "November ~ December"
                });

                for (int a = 0; a < 7; a++)
                {
                    //Day of Week
                    string dayoftheweek = new DateTime(year, 11, 21 + a).ToString("ddd");
                    Datename.Add(new CalendarModel
                    {
                        Currentday = dayoftheweek, // 21, 22
                    });
                }

                for (int b = 21; b <= DateTime.DaysInMonth(year, 11); b++)
                {
                    CalendarDetails.Add(new CalendarModel
                    {
                        Day = b, // start with the curretn month
                        CurrentGridColumn = buttonGridColumn, //1, 2
                        CurrentGridRow = buttonGridRow //0
                    });
                }

                for (int m = 1; m <= DateTime.DaysInMonth(year, 12); m++)
                {
                    CalendarDetails.Add(new CalendarModel
                    {
                        Day = m,
                        CurrentGridColumn = buttonGridColumn,
                        CurrentGridRow = buttonGridRow,
                    });

                    if (m == 20)
                    {
                        break;
                    }
                }

                //month = 1;
            }
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
                    string dayoftheweek = new DateTime(year, 12, 21 + a).ToString("ddd");
                    Datename.Add(new CalendarModel
                    {
                        Currentday = dayoftheweek, // 21, 22
                    });
                }

                for (int c = 21; c <= DateTime.DaysInMonth(year, 12); c++)
                {
                    CalendarDetails.Add(new CalendarModel
                    {
                        Day = c, // start with the curretn month
                        CurrentGridColumn = buttonGridColumn, //1, 2
                        CurrentGridRow = buttonGridRow //0
                    });
                }

                for (int d = 1; d <= DateTime.DaysInMonth(year + 1, 1); d++)
                {
                    CalendarDetails.Add(new CalendarModel
                    {
                        Day = d,
                        CurrentGridColumn = buttonGridColumn,
                        CurrentGridRow = buttonGridRow,
                    });

                    if (d == 20)
                    {
                        break;
                    }
                }
                month++;
            }
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
                            CurrentGridColumn = buttonGridColumn, //1, 2
                            CurrentGridRow = buttonGridRow //0
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
                            CurrentGridColumn = buttonGridColumn,
                            CurrentGridRow = buttonGridRow,
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
                        CalendarDetails.Add(new CalendarModel
                        {
                            Day = r, // 21,22,23,24,25,26,27,28,29,30 start with the last month
                            CurrentGridColumn = buttonGridColumn, //1, 2
                            CurrentGridRow = buttonGridRow //0
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
                            CurrentGridColumn = buttonGridColumn,
                            CurrentGridRow = buttonGridRow,
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
            IsBusy = true;
            CalendarDetails.Clear();
            Datename.Clear();
            Monthname.Clear();
            decreasemonth = month - 2;  //3
            if(decreasemonth < 0)
            {
                decreasemonth++;
            }
            now = DateTime.Now.AddMonths(decreasemonth); //3
            month = decreasemonth; // 3
            DisplayDays();
            IsBusy = false;
            IsRefreshing = false;
        }



        [RelayCommand]
        public void IncreaseMonth()
        {
            IsBusy = true;
            CalendarDetails.Clear();
            Datename.Clear();
            Monthname.Clear();
            now = DateTime.Now.AddMonths(month++);  //4
            DisplayDays();
            IsBusy = false;
            IsRefreshing = false;

        }
    }

}






