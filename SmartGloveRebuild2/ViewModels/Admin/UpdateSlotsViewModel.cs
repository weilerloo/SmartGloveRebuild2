using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

namespace SmartGloveRebuild2.ViewModels.Admin
{
    public partial class UpdateSlotsViewModel : BaseViewModel
    {
        #region Properties  
        public static ObservableCollection<UpdateGroupModel> GroupSchedule { get; set; } = new ObservableCollection<UpdateGroupModel>();
        public ObservableCollection<UpdateSlotsModel> ListCalendar { get; set; } = new ObservableCollection<UpdateSlotsModel>();
        public ObservableCollection<UpdateSlotsModel> Datename { get; set; } = new ObservableCollection<UpdateSlotsModel>();
        public ObservableCollection<UpdateSlotsModel> Monthname { get; set; } = new ObservableCollection<UpdateSlotsModel>();

        #endregion

        #region Obseravble Property

        [ObservableProperty]
        DateTime now = DateTime.Now;

        [ObservableProperty]
        int month = 1, decreasemonth, increasemonth, year;

        [ObservableProperty]
        String lastcurrentmonth;

        [ObservableProperty]
        bool isRefreshing, status = true;

        public UpdateSlotsModel choosenitem;
        public UpdateSlotsModel ChoosenItem
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
        private readonly IGroupServices _groupServices;

        public UpdateSlotsViewModel(IScheduleServices scheduleServices, IGroupServices groupServices)
        {
            _scheduleService = scheduleServices;
            _groupServices = groupServices;
            DisplayDays();
            Title = "Update Slots";
            GroupSchedule = new ObservableCollection<UpdateGroupModel>();
            ChoosenItem = new UpdateSlotsModel();
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
                Monthname.Add(new UpdateSlotsModel
                {
                    Year = year,  // 2022
                    LastCurrentMonth = "November ~ December" // 11~12
                });

                for (int a = 0; a < 7; a++)
                {
                    //Day of Week
                    string dayoftheweek = new DateTime(year, 11, 21 + a).ToString("ddd");
                    Datename.Add(new UpdateSlotsModel
                    {
                        Currentday = dayoftheweek, // 21, 22
                    });
                }  //Day Name

                for (int b = 21; b <= DateTime.DaysInMonth(year, 11); b++)
                {
                    ListCalendar.Add(new UpdateSlotsModel
                    {
                        Day = b, // start with the curretn month
                        Currentmonth = 11,
                        Currentyear = year,
                    });
                }  // End of the November

                for (int m = 1; m <= DateTime.DaysInMonth(year, 12); m++)
                {
                    ListCalendar.Add(new UpdateSlotsModel
                    {
                        Day = m,
                        Currentmonth = 12,
                        Currentyear = year,
                    });

                    if (m == 20)
                    {
                        break;
                    }
                }  // Start of the December

            }  // For December, it will print end of November with start of Decemebr
            else if (month == 1)
            {
                Monthname.Add(new UpdateSlotsModel
                {
                    Year = year,
                    LastCurrentMonth = "December ~ January"
                });

                for (int a = 0; a < 7; a++)
                {
                    //Day of Week
                    string dayoftheweek = new DateTime(year, 12, 21 + a).ToString("ddd");
                    Datename.Add(new UpdateSlotsModel
                    {
                        Currentday = dayoftheweek, // 21, 22
                    });
                } //Day Name

                for (int c = 21; c <= DateTime.DaysInMonth(year, 12); c++)
                {
                    ListCalendar.Add(new UpdateSlotsModel
                    {
                        Day = c, // start with the curretn month
                        Currentmonth = 12,
                        Currentyear = year,
                    });
                }  //End of December

                for (int d = 1; d <= DateTime.DaysInMonth(year + 1, 1); d++)
                {
                    ListCalendar.Add(new UpdateSlotsModel
                    {
                        Day = d,
                        Currentmonth = 1,
                        Currentyear = year + 1,
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
                    Monthname.Add(new UpdateSlotsModel
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
                        Datename.Add(new UpdateSlotsModel
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
                        ListCalendar.Add(new UpdateSlotsModel
                        {
                            Day = p, // start with the curretn month
                            Currentmonth = month,
                            Currentyear = year,
                        });
                    }

                    for (int q = 1; q <= DateTime.DaysInMonth(year, month + 1); q++)
                    {
                        if (month == 1)
                        {
                            break;
                        }
                        ListCalendar.Add(new UpdateSlotsModel
                        {
                            Day = q,
                            Currentmonth = month + 1,
                            Currentyear = year,
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
                    Monthname.Add(new UpdateSlotsModel
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
                        Datename.Add(new UpdateSlotsModel
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
                        ListCalendar.Add(new UpdateSlotsModel
                        {
                            Day = r, // 21,22,23,24,25,26,27,28,29,30 start with the last month
                            Currentmonth = month - 1,
                            Currentyear = year,
                        });
                    } //Start with Last Month 21 ~ 31 

                    for (int t = 1; t <= DateTime.DaysInMonth(year, month); t++)
                    {
                        if (month == 1)
                        {
                            break;
                        }
                        ListCalendar.Add(new UpdateSlotsModel
                        {
                            Day = t,
                            Currentmonth = month,
                            Currentyear = year,
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
            ListCalendar.Clear();
            Datename.Clear();
            Monthname.Clear();
            ReduceMonth();
            now = DateTime.Now.AddMonths(month); // 1
            DisplayDays();
            IsBusy = false;
            IsRefreshing = false;
        }



        [RelayCommand]
        public void IncreaseMonth()
        {
            IsBusy = true;
            ListCalendar.Clear();
            Datename.Clear();
            Monthname.Clear();
            AddMonth();
            now = DateTime.Now.AddMonths(month);
            DisplayDays();
            IsBusy = false;
            IsRefreshing = false;

        }

        #endregion

        #region GoToCommand
        [RelayCommand]
        private async Task UpdateSlots()
        {
            await Shell.Current.GoToAsync(nameof(UpdateSlotsPage));
        }

        [RelayCommand]
        public async Task SelectedItem(UpdateSlotsModel updateSlotsModel)
        {
            bool success = false;
            GroupSchedule.Clear();
            updateSlotsModel.DayMonthYear = updateSlotsModel.Day.ToString() +
                "/" + updateSlotsModel.Currentmonth.ToString() +
                "/" + updateSlotsModel.Currentyear.ToString();
            var response = await _scheduleService.GetSchedulebyGroupandDate(new GetSchedulebyGroupandDateDTO
            {
                ScheduleDate = updateSlotsModel.DayMonthYear,
                GroupName = App.UserDetails.GroupName,
            });

            var getGroup = await _groupServices.DisplayGroup();
            if (updateSlotsModel != null)
            {
                if (response != null && getGroup != null)
                {
                    foreach (var Group in response)// 14/12/2022, Biomas, Sigma, GX2
                    {
                        foreach (var newG in getGroup)  // From new group
                        {
                            string ConvertedDayMonthYear = Group.DayMonthYear.Substring(0, 10);
                            if (updateSlotsModel.DayMonthYear == ConvertedDayMonthYear && Group.GroupName == newG.GroupName)
                            {
                                GroupSchedule.Add(new UpdateGroupModel
                                {
                                    GroupName = Group.GroupName,
                                    Status = Group.Status,
                                    DayMonthYear = Group.DayMonthYear.Substring(0, 10),
                                    Hours = Group.Hours,
                                    Paxs = Group.Paxs,
                                });
                            }
                            else
                            {
                                GroupSchedule.Add(new UpdateGroupModel
                                {
                                    GroupName = newG.GroupName,
                                    Status = false,
                                    DayMonthYear = Group.DayMonthYear.Substring(0, 10),
                                    Hours = 0,
                                    Paxs = 0,
                                });
                            }
                        }
                        success = true;
                        break;
                    }
                }
            }

            if(success == false)
            {
                foreach (var newG in getGroup)  // From new group
                {

                    GroupSchedule.Add(new UpdateGroupModel
                    {
                        GroupName = newG.GroupName,
                        Status = false,
                        DayMonthYear = updateSlotsModel.DayMonthYear,
                        Hours = 0,
                        Paxs = 0,
                    });
                }
            }

            if (updateSlotsModel == null)
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(UpdateSlotsDetails), true, new Dictionary<string, object>
            {
            {"UpdateSlotsModel", updateSlotsModel
    }

});
        }

        #endregion

        #region GetGroupList

        public void ConcatenateLabelDate()
        {
            //foreach (var date in ListCalendar)
            //{
            //    string day = date.Day.ToString();
            //    string month = date.Currentmonth.ToString();
            //    string year = date.Currentyear.ToString();
            //    GroupSchedule.Add(new ListCalendar
            //    {
            //        DayMonthYear = day + "/" + month + "/" + year
            //    });
            //}
        }


        [RelayCommand]
        public async Task ClickSelectedDate(UpdateSlotsModel choosenItem)
        {



        }

        //public void ConcatenateLabelDate()
        //{
        //    if (UpdateSlotsModel.Day.ToString().Length > 1)
        //    {
        //        string day = UpdateSlotsModel.Day.ToString();
        //        string month = UpdateSlotsModel.Currentmonth.ToString();
        //        string year = UpdateSlotsModel.Currentyear.ToString();
        //        daymonthyear = day + "%2F" + month + "%2F" + year;
        //    }
        //    else
        //    {
        //        string day = UpdateSlotsModel.Day.ToString();
        //        string month = UpdateSlotsModel.Currentmonth.ToString();
        //        string year = UpdateSlotsModel.Currentyear.ToString();
        //        daymonthyear = day + "%2F" + month + "%2F" + year;
        //    }

        //}
        //[RelayCommand]
        //public async void GetGroupSchdule()
        //{
        //    try
        //    {
        //        if (App.UserDetails.Role == "CLERK")
        //        {

        //            var response = await _scheduleServices.GetSchedulebyDate(new GetSchedulebyDateDTO
        //            {
        //                ScheduleDate = daymonthyear,
        //            });

        //            if (response != null)
        //            {
        //                GroupSchedule.Add(new GroupScheduleModel
        //                {
        //                    GroupName = response.GroupName,
        //                    Hours = response.Hours,
        //                    Paxs = response.Paxs,
        //                    Status = response.Status,
        //                });
        //            }
        //            else
        //            {
        //                await AppShell.Current.DisplayAlert("Hello", "Takpaye kerja", "OK");
        //            }
        //        }
        //        else
        //        {
        //            await AppShell.Current.DisplayAlert("Error!", "No Group or Schedule Found!", "OK");

        //        }
        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}
        #endregion

    }
}