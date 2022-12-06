using CommunityToolkit.Mvvm.ComponentModel;
using SmartGloveRebuild2.Controls;
using SmartGloveRebuild2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SmartGloveRebuild2.ViewModels.Dashboard
{
    public partial class DashboardPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private EmployeeModel employeeModel;
        
        public DashboardPageViewModel()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();

        }
    }
}
