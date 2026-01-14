using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;

using QuickPick_Employer.QuickPickEmployer.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace QuickPick_Employer.QuickPickEmployer.ViewModel
{
    public partial class SalesAnalysisViewModel : ObservableObject
    {
        [ObservableProperty]
        private Chart incomeChart;
        [ObservableProperty]
        private string selectedChart;
        public SalesAnalysisViewModel()
        {
            LoadIncome();
        }
        partial void OnSelectedChartChanged(string value)
        {
            _ = UpdateIncomeChartAsync(value);
        }
        private async Task LoadIncome()
        {
            List<Transaction> transactions =  new List<Transaction>();
            if(transactions.Count > 0)
            {
                List<float> totals = new List<float>();
                float total = 0;
                for (int i = 1; i <= 12; i++)
                {
                    total = transactions.Where(x => x.TransactionDate.Month == i).Sum(s => (float)s.TotalAmount);
                    totals.Add(total);
                }
                List<ChartEntry> entries = new List<ChartEntry>();
                int index = 1;
                foreach (var i in totals)
                {
                    if (index == 1)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C",new CultureInfo("en-ZA")),
                            Label = "Jan",
                            Color = SKColors.Blue
                        };
                        entries.Add(entry);
                    }
                    else if (index == 2)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Feb",
                            Color = SKColors.Blue
                        };
                        entries.Add(entry);
                    }
                    else if (index == 3)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Mar",
                            Color = SKColors.Blue
                        };
                    }
                    else if (index == 4)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "April",
                            Color = SKColors.Blue
                        };
                    }
                    else if (index == 5)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "May",
                            Color = SKColors.Blue
                        };
                    }
                    else if (index == 6)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Jun",
                            Color = SKColors.Blue
                        };
                    }
                    else if (index == 7)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Jul",
                            Color = SKColors.Blue
                        };
                    }
                    else if (index == 8)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Aug",
                            Color = SKColors.Blue
                        };
                    }
                    else if (index == 9)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Sep",
                            Color = SKColors.Blue
                        };
                    }
                    else if (index == 10)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Oct",
                            Color = SKColors.Blue
                        };
                    }
                    else if (index == 11)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Nov",
                            Color = SKColors.Blue
                        };
                    }
                    else if (index == 12)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Dec",
                            Color = SKColors.Blue
                        };
                    }
                    index++;
                }
                IncomeChart = new LineChart()
                {
                    Entries = entries,
                    LineMode = LineMode.Straight,
                    LineSize = 8,
                    PointMode = PointMode.Circle,
                    PointSize = 18,
                    BackgroundColor = SKColors.White,
                    ValueLabelOrientation = Orientation.Vertical,
                    LabelTextSize = 15f,
                };
            }
            else
            {
                ChartEntry[] entry = new[]
                {
                    new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Jan",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Feb",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Mar",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Apl",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "May",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Jun",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Jul",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Sep",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Aug",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "oct",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Nov",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Dec",
                    Color = SKColors.Blue,
                },
                };
                IncomeChart = new LineChart()
                {
                    Entries = entry,
                    LineMode = LineMode.Straight,
                    LineSize = 8,
                    PointMode = PointMode.Circle,
                    PointSize = 18,
                    BackgroundColor = SKColors.White,
                    ValueLabelOrientation = Orientation.Vertical,
                    LabelTextSize = 15f,
                };
            }
        }
        private async Task<List<ChartEntry>> ReturnEntries()
        {
            List<Transaction> transactions =  new List<Transaction>();
            if (transactions.Count > 0)
            {
                List<float> totals = new List<float>();
                float total = 0;
                for (int i = 1; i <= 12; i++)
                {
                    total = transactions.Where(x => x.TransactionDate.Month == i).Sum(s => (float)s.TotalAmount);
                    totals.Add(total);
                }
                List<ChartEntry> entries = new List<ChartEntry>();
                int index = 1;
                foreach (var i in totals)
                {
                    if (index == 1)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Jan",
                            Color = SKColors.Blue
                        };
                        entries.Add(entry);
                    }
                    else if (index == 2)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Feb",
                            Color = SKColors.Green
                        };
                        entries.Add(entry);
                    }
                    else if (index == 3)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Mar",
                            Color = SKColors.Red
                        };
                    }
                    else if (index == 4)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "April",
                            Color = SKColors.Orange
                        };
                    }
                    else if (index == 5)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "May",
                            Color = SKColors.GreenYellow
                        };
                    }
                    else if (index == 6)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Jun",
                            Color = SKColors.AliceBlue
                        };
                    }
                    else if (index == 7)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Jul",
                            Color = SKColors.Pink
                        };
                    }
                    else if (index == 8)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Aug",
                            Color = SKColors.Orange
                        };
                    }
                    else if (index == 9)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Sep",
                            Color = SKColors.Yellow
                        };
                    }
                    else if (index == 10)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Oct",
                            Color = SKColors.Navy
                        };
                    }
                    else if (index == 11)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Nov",
                            Color = SKColors.Purple
                        };
                    }
                    else if (index == 12)
                    {
                        var entry = new ChartEntry(i)
                        {
                            ValueLabel = i.ToString("C", new CultureInfo("en-ZA")),
                            Label = "Dec",
                            Color = SKColors.White
                        };
                    }
                    index++;
                }
                return entries;
            }
            else
            {
                List<ChartEntry> entry = new List<ChartEntry>
                {
                    new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Jan",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Feb",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Mar",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Apl",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "May",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Jun",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Jul",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Sep",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Aug",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "oct",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Nov",
                    Color = SKColors.Blue,
                },
                new ChartEntry(0)
                {

                    ValueLabel = "R0,00",
                    Label = "Dec",
                    Color = SKColors.Blue,
                },
                };
                return entry;
            }
        }
        private async Task UpdateIncomeChartAsync(string? value)
        {
            // Clear chart on UI thread first so the control can remove the previous visuals.
            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() => IncomeChart = null);

            // Give the UI a moment to process the clear. Small delay avoids blocking.
            await Task.Delay(50).ConfigureAwait(false);

            var entries = await ReturnEntries();

            Chart newChart = null;

            if (value == "Pie Chart")
            {
                newChart = new PieChart { Entries = entries, BackgroundColor = SKColors.Transparent, LabelMode = LabelMode.RightOnly, LabelTextSize = 15f };
            }
            else if (value == "Bar Chart")
            {
                newChart = new BarChart { Entries = entries, ValueLabelOrientation = Orientation.Vertical, LabelTextSize = 15f };
            }
            else if (value == "Line Chart")
            {
                    newChart = new LineChart
                    {
                        Entries = entries,
                        LineMode = LineMode.Straight,
                        LineSize = 8,
                        PointMode = PointMode.Circle,
                        PointSize = 18,
                        BackgroundColor = SKColors.Transparent,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 15f
                    };
            }
            else if (value == "Half Radial Gauge Chart")
            {
                newChart = new RadialGaugeChart { Entries = entries, LineSize = 8, LabelTextSize = 15f };
            }

            // Assign the new chart on the UI thread.
            if (newChart != null)
            {
                Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() => IncomeChart = newChart);
            }
        }
    }
}
