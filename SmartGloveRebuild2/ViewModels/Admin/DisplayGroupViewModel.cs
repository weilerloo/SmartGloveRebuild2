using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.Services;
using SmartGloveRebuild2.Views.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.ViewModels.Admin
{
    public partial class DisplayGroupViewModel : BaseViewModel
    {
        [RelayCommand]
        private async void DisplayGroup()
        {
            await Shell.Current.GoToAsync(nameof(DisplayGroupPage));
        }

        //[ObservableProperty]
        //bool isRefreshing;

        //IConnectivity connectivity;

        //EmployeeServices employeeServices;
        //public ObservableCollection<EmployeeModel> employeeModels { get; } = new();

        //public DisplayGroupViewModel()
        //{
        //    DisplayEmployee();
        //}

        //public void DisplayEmployee()
        //{

        //}


        //[RelayCommand]
        //async Task GetEmployeeSchedule()
        //{
        //    if (IsBusy)
        //        return;

        //    try
        //    {
        //        if (connectivity.NetworkAccess != NetworkAccess.Internet)
        //        {
        //            await Shell.Current.DisplayAlert("No connectivity!",
        //                $"Please check internet and try again.", "OK");
        //            return;
        //        }

        //        IsBusy = true;

        //        var employees = await employeeServices.GetEmployees();

        //        if (employeeModels.Count != 0)
        //        {
        //            employeeModels.Clear();
        //        }

        //        foreach (var employee in employees)
        //        {
        //            employeeModels.Add(employee);
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Unable to get Employee Schedule: {ex.Message}");
        //        await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //        IsRefreshing = false;
        //    }

        //}
    }
}
