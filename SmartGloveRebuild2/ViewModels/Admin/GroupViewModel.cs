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
using SmartGloveRebuild2.Models.Group;
using SmartGloveRebuild2.Models.ClerkDTO;
using SmartGloveRebuild2.Views.Dashboard;
using System.Text.RegularExpressions;

namespace SmartGloveRebuild2.ViewModels.Admin
{

    public partial class GroupViewModel : BaseViewModel
    {
        //public static ObservableCollection<UserList> Groups { get; set; } = new ObservableCollection<UserList>();
        //public static List<UserList> listofEmployee { get; private set; } = new List<UserList>();
        //[ObservableProperty]
        //string groupentered;

        //private readonly IScheduleServices _scheduleServices;
        //public GroupViewModel(IScheduleServices scheduleServices)
        //{
        //    _scheduleServices = scheduleServices;

        //}
        //[RelayCommand]
        //public async Task EMPGroup()
        //{
        //    Groups.Clear();
        //    var getGroup = await _scheduleServices.GetAllEmployee();
        //    if (getGroup != null)
        //    {

        //        foreach (var newG in getGroup)  // From new group
        //        {
        //            Groups.Add(new UserList
        //            {
        //                GroupName = newG.GroupName,
        //                EmployeeNumber = newG.EmployeeNumber,
        //                TotalHour = newG.TotalHour,
        //            });
        //        }
        //    }
        //}

        //[RelayCommand]
        //public async Task GetsEmployees()
        //{
        //    var getGroup = await _scheduleServices.GetAllEmployee();
        //    foreach (var Group in Groups)
        //    {
        //        listofEmployee.Add(new UserList
        //        {
        //            GroupName = Group.GroupName,
        //            EmployeeNumber = Group.EmployeeNumber,
        //            TotalHour = Group.TotalHour,
        //        });
        //    }
        //}
        //[RelayCommand]
        //public async void GetsEmployees()
        //{
        //    Groups.Clear();
        //    var employeeList = await _scheduleServices.GetAllEmployee();
        //    if (employeeList?.Count > 0)
        //    {
        //        employeeList = employeeList.OrderBy(f => f.GroupName).ToList();
        //        foreach (var Group in employeeList)
        //            listofEmployee.Add(new UserList
        //            {
        //                GroupName = Group.GroupName,
        //                EmployeeNumber = Group.EmployeeNumber,
        //                TotalHour = Group.TotalHour,
        //            });
        //        listofEmployee.Clear();
        //        listofEmployee.AddRange(employeeList);
        //    }
        //    await Shell.Current.GoToAsync(nameof(DisplayGroupPage));
        //}
            //[RelayCommand]
            //public async void DisplayAction(UserList userList)
            //{
            //    var response = await AppShell.Current.DisplayActionSheet("Select Option", "OK", null, "Edit", "Delete");
            //    if (response == "Edit")
            //    {
            //        var navParam = new Dictionary<string, object>();
            //        navParam.Add("EmployeeDetail", userList);
            //        await AppShell.Current.GoToAsync(nameof(DisplayGroupPage), navParam);
            //    }
            //    else if (response == "Delete")
            //    {
            //        //var delResponse = await _scheduleServices.DeleteStudent(userList);
            //        //if (delResponse > 0)
            //        //{
            //        //    GetStudentList();
            //        //}
            //    }
            //}
        }

}



//[RelayCommand]
//public async void CreateGroup()
//{
//    int response = -1;
//    if (GroupDetail.GroupName > 0)
//    {
//    response = await _groupService.AddGroup(new Models.CreateGroupDTO
//    {
//        GroupName = GroupDetail.GroupName,
//    });
//}
//    else
//    {
//    return;
//    }



//    if (response > 0)
//    {
//        await Shell.Current.DisplayAlert("Student Info Saved", "Record Saved", "OK");
//    }
//    else
//    {
//        await Shell.Current.DisplayAlert("Heads Up!", "Something went wrong while adding record", "OK");
//    }

