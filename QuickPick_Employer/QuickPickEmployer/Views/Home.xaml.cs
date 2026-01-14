using Microcharts;
using QuickPick_Employer.QuickPickEmployer.ViewModel;
using SkiaSharp;

namespace QuickPick_Employer.QuickPickEmployer.Views;

public partial class Home : ContentPage
{
    HomeViewModel _homeViewModel;
    public Home(HomeViewModel homeViewModel)
	{
		InitializeComponent();
        BindingContext = homeViewModel;
        _homeViewModel = homeViewModel;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        _homeViewModel.LoadIncomeChartCommand.Execute(null);
        _homeViewModel.LoadItemsLeftCommand.Execute(null);
        _homeViewModel.LoadOrdersCommand.Execute(null);
    }
}