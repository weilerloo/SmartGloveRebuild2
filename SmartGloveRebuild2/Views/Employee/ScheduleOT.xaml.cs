using SmartGloveRebuild2.Models;
using SmartGloveRebuild2.ViewModels;
using SmartGloveRebuild2.ViewModels.Employee;

namespace SmartGloveRebuild2.Views.Employee;

public partial class ScheduleOT : ContentPage
{
    Button DateButton;
    ScheduleViewModel temp;
    public ScheduleOT(ScheduleViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
        temp = viewModel;

    }

    //private void SelectItemsButton_Clicked(object sender, EventArgs e)
    //{
    //    var button = sender as Button;
    //    var selectedDate = button.BindingContext as selectedItem;
    //    var vm = BindingContext as ScheduleViewModel;
    //    vm.SubmitButtonSelectedCommand.Execute(selectedDate);
    //}


}