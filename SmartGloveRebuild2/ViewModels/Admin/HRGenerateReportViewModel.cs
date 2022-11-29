using CommunityToolkit.Mvvm.Input;
using SmartGloveRebuild2.Views.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.ViewModels.Admin
{
    public partial class HRGenerateReportViewModel : BaseViewModel
    {
        [RelayCommand]
        private async void HRGenerateReport()
        {
            await Shell.Current.GoToAsync(nameof(GenerateReportPage));
        }
    }
}
