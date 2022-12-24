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

    //private void MinusButton_Clicked(object sender, EventArgs e)
    //{
    //    if (sender is Button button)
    //    {
    //        if (viewModel.IsBusy == true)
    //        {
    //            button.IsEnabled = false;
    //        }
    //        else
    //        {
    //            button.IsEnabled = true;
    //        }
    //    }
    //}

    //private void AddButton_Clicked(object sender, EventArgs e)
    //{
    //    if(sender is Button button)
    //    {
    //        if(ScheduleViewModel.IsBusy == true)
    //        {
    //            button.IsEnabled = false;
    //        }
    //        else
    //        {
    //            button.IsEnabled = true;
    //        }
    //    }
    //}

    //private void SelectItemsButton_Clicked(object sender, EventArgs e)
    //{
    //    var button = sender as Button;
    //    var selectedDate = button.BindingContext as selectedItem;
    //    var vm = BindingContext as ScheduleViewModel;
    //    vm.SubmitButtonSelectedCommand.Execute(selectedDate);
    //}


}