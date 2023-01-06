using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.ViewModels;
using SmartGloveRebuild2.ViewModels.Employee;

namespace SmartGloveRebuild2.Views.Employee;

public partial class ScheduleOT : ContentPage
{
    public ScheduleOT(ScheduleViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}