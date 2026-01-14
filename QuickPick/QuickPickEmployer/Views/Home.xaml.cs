using Microcharts;
using QuickPick.QuickPickEmployer.ViewModel;
using SkiaSharp;

namespace QuickPick.QuickPickEmployer.Views;

public partial class Home : ContentPage
{
    public Home(HomeViewModel homeViewModel)
	{
		InitializeComponent();
        BindingContext = homeViewModel;
    }
}