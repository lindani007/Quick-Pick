using Microcharts;
using Microcharts.Maui;
using SkiaSharp;
namespace QuickPick.QuikApp.Pages.Employer;

public partial class FlyoutDashboard : FlyoutPage
{
	ChartEntry[] incomeEntry = new[]
	{
		new ChartEntry(2000)
		{
			Label = "6h00-7h00",
			ValueLabel = "2000",
			Color = SKColors.Red,
        },
        new ChartEntry(3000)
        {
            Label = "7h00-8h00",
            ValueLabel = "3000",
            Color = SKColors.Red,
        },
        new ChartEntry(2000)
        {
            Label = "8h00-9h00",
            ValueLabel = "2000",
            Color = SKColors.Red,
        },
        new ChartEntry(4000)
        {
            Label = "9h00-10h00",
            ValueLabel = "4000",
            Color = SKColors.Red,
        },
        new ChartEntry(3500)
        {
            Label = "10h00-11h00",
            ValueLabel = "3500",
            Color = SKColors.Red,
        },
        new ChartEntry(5000)
        {
            Label = "11h00-12h00",
            ValueLabel = "5000",
            Color = SKColors.Orange,
        },
        new ChartEntry(1000)
        {
            Label = "12h00-13h00",
            ValueLabel = "1000",
            Color = SKColors.Red,
        },
        new ChartEntry(6000)
        {
            Label = "13h00-14h00",
            ValueLabel = "6000",
            Color = SKColors.Blue,
        },
        new ChartEntry(7000)
        {
            Label = "14h00-15h00",
            ValueLabel = "7000",
            Color = SKColors.Blue,
        },
        new ChartEntry(8000)
        {
            Label = "15h00-16h00",
            ValueLabel = "8000",
            Color = SKColors.Blue,
        },
        new ChartEntry(9000)
        {
            Label = "16h00-17h00",
            ValueLabel = "9000",
            Color = SKColors.Green,
        },
        new ChartEntry(10000)
        {
            Label = "17h00-18h00",
            ValueLabel = "10000",
            Color = SKColors.Green,
        },
         new ChartEntry(7000)
        {
            Label = "18h00-19h00",
            ValueLabel = "7000",
            Color = SKColors.Blue,
        },

    };
    ChartEntry[] profitEntry = new[]
    {
        new ChartEntry(2000)
        {
            Label = "Jan",
            ValueLabel = "2000",
            Color = SKColors.Red,
        },
        new ChartEntry(3000)
        {
            Label = "Feb",
            ValueLabel = "3000",
            Color = SKColors.Red,
        },
        new ChartEntry(2000)
        {
            Label = "Mar",
            ValueLabel = "2000",
            Color = SKColors.Red,
        },
        new ChartEntry(4000)
        {
            Label = "Apr",
            ValueLabel = "4000",
            Color = SKColors.Red,
        },
        new ChartEntry(3500)
        {
            Label = "May",
            ValueLabel = "3500",
            Color = SKColors.Red,
        },
        new ChartEntry(5000)
        {
            Label = "Jun",
            ValueLabel = "5000",
            Color = SKColors.Orange,
        },
        new ChartEntry(1000)
        {
            Label = "Jul",
            ValueLabel = "1000",
            Color = SKColors.Red,
        },
        new ChartEntry(6000)
        {
            Label = "Aug",
            ValueLabel = "6000",
            Color = SKColors.Blue,
        },
        new ChartEntry(7000)
        {
            Label = "Sep",
            ValueLabel = "7000",
            Color = SKColors.Blue,
        },
        new ChartEntry(8000)
        {
            Label = "Oct",
            ValueLabel = "8000",
            Color = SKColors.Blue,
        },
        new ChartEntry(9000)
        {
            Label = "Nov",
            ValueLabel = "9000",
            Color = SKColors.Green,
        },
        new ChartEntry(10000)
        {
            Label = "Dec",
            ValueLabel = "10000",
            Color = SKColors.Green,
        },

    };
    ChartEntry[] costEntry = new[]
    {
        new ChartEntry(2000)
        {
            Label = "Jan",
            ValueLabel = "2000",
            Color = SKColors.Red,
        },
        new ChartEntry(3000)
        {
            Label = "Feb",
            ValueLabel = "3000",
            Color = SKColors.Red,
        },
        new ChartEntry(2000)
        {
            Label = "Mar",
            ValueLabel = "2000",
            Color = SKColors.Red,
        },
        new ChartEntry(4000)
        {
            Label = "Apr",
            ValueLabel = "4000",
            Color = SKColors.Red,
        },
        new ChartEntry(3500)
        {
            Label = "May",
            ValueLabel = "3500",
            Color = SKColors.Red,
        },
        new ChartEntry(5000)
        {
            Label = "Jun",
            ValueLabel = "5000",
            Color = SKColors.Orange,
        },
        new ChartEntry(1000)
        {
            Label = "Jul",
            ValueLabel = "1000",
            Color = SKColors.Red,
        },
        new ChartEntry(6000)
        {
            Label = "Aug",
            ValueLabel = "6000",
            Color = SKColors.Blue,
        },
        new ChartEntry(7000)
        {
            Label = "Sep",
            ValueLabel = "7000",
            Color = SKColors.Blue,
        },
        new ChartEntry(8000)
        {
            Label = "Oct",
            ValueLabel = "8000",
            Color = SKColors.Blue,
        },
        new ChartEntry(9000)
        {
            Label = "Nov",
            ValueLabel = "9000",
            Color = SKColors.Green,
        },
        new ChartEntry(10000)
        {
            Label = "Dec",
            ValueLabel = "10000",
            Color = SKColors.Green,
        },

    };
    public FlyoutDashboard()
    {
        InitializeComponent();
        incomeChartView.Chart = new LineChart() { Entries = incomeEntry, LineMode = LineMode.Straight, ValueLabelOrientation = Orientation.Vertical, PointMode = PointMode.Circle, PointSize = 15f };
        ordersChartView.Chart = new LineChart() { Entries = incomeEntry, LineMode = LineMode.Straight, ValueLabelOrientation = Orientation.Vertical, PointMode = PointMode.Circle, PointSize = 15f, LabelTextSize = 10 };
        itemChartView.Chart = new BarChart() { Entries = costEntry, ValueLabelOrientation = Orientation.Vertical };
    }
}