using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using QuickPick.QuickPickEmployer.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuickPick.QuickPickEmployer.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private Chart hoursIncomeChart;
        [ObservableProperty]
        private Chart ordersChart;
        [ObservableProperty]
        private Chart itemsChart;
        [ObservableProperty]
        private string emptyItems;
        [ObservableProperty]
        private string? selectedChart;
        [ObservableProperty]
        private string? totalOrders;
        [ObservableProperty]
        private string? totalItems;
        [ObservableProperty]
        private string total;
        ChartEntry[] entry = new[]
    {
        new ChartEntry(5000)
        {
            Label = "6h00-7h00",
            ValueLabel = "0",
            Color = SKColors.Red,
        },
        new ChartEntry(0)
        {
            Label = "7h00-8h00",
            ValueLabel = "0",
            Color = SKColors.Red,
        },
        new ChartEntry(0)
        {
            Label = "8h00-9h00",
            ValueLabel = "0",
            Color = SKColors.Red,
        },
        new ChartEntry(0)
        {
            Label = "9h00-10h00",
            ValueLabel = "0",
            Color = SKColors.Red,
        },
        new ChartEntry(10000)
        {
            Label = "10h00-11h00",
            ValueLabel = "0",
            Color = SKColors.Red,
        },
        new ChartEntry(0)
        {
            Label = "11h00-12h00",
            ValueLabel = "0",
            Color = SKColors.Red,
        },
        new ChartEntry(0)
        {
            Label = "12h00-13h00",
            ValueLabel = "0",
            Color = SKColors.Red,
        },
        new ChartEntry(0)
        {
            Label = "13h00-14h00",
            ValueLabel = "0",
            Color = SKColors.Red,
        },
        new ChartEntry(0)
        {
            Label = "14h00-15h00",
            ValueLabel = "0",
            Color = SKColors.Red,
        },
        new ChartEntry(0)
        {
            Label = "15h00-16h00",
            ValueLabel = "0",
            Color = SKColors.Red,
        },
        new ChartEntry(0)
        {
            Label = "16h00-17h00",
            ValueLabel = "0",
            Color = SKColors.Red,
        },
        new ChartEntry(0)
        {
            Label = "17h00-18h00",
            ValueLabel = "0",
            Color = SKColors.Red,
        },
        new ChartEntry(0)
        {
            Label = "18h00-19h00",
            ValueLabel = "0",
            Color = SKColors.Red,
        },

    };
        public HomeViewModel()
        {
            LoadIncomeChart();
            LoadOrders();
            LoadItemsLeft();
            selectedChart = "Line Chart";
        }
        partial void OnSelectedChartChanged(string? value)
        {
            // Fire-and-forget update to allow UI to clear first and then set new chart.
            _ = UpdateHoursChartAsync(value);
        }

        private async Task UpdateHoursChartAsync(string? value)
        {
            // Clear chart on UI thread first so the control can remove the previous visuals.
            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() => HoursIncomeChart = null);

            // Give the UI a moment to process the clear. Small delay avoids blocking.
            await Task.Delay(50).ConfigureAwait(false);

            var entries = ReturnEntries();

            Chart newChart = null;

            if (value == "Pie Chart")
            {
                newChart = (entries.Count > 0)
                    ? new PieChart { Entries = entries, BackgroundColor = SKColors.Transparent, LabelMode = LabelMode.RightOnly, LabelTextSize = 15f }
                    : new PieChart { Entries = entry, BackgroundColor = SKColors.Transparent, LabelTextSize = 15f };
            }
            else if (value == "Bar Chart")
            {
                newChart = (entries.Count > 0)
                    ? new BarChart { Entries = entries, BackgroundColor = SKColors.Transparent, ValueLabelOrientation = Orientation.Vertical, LabelTextSize = 15f }
                    : new BarChart { Entries = entry, BackgroundColor = SKColors.Transparent, ValueLabelOrientation = Orientation.Vertical, LabelTextSize = 15f };
            }
            else if (value == "Line Chart")
            {
                if (entries.Count > 0)
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
                else
                {
                    newChart = new LineChart
                    {
                        Entries = entry,
                        LineMode = LineMode.Straight,
                        LineSize = 8,
                        PointMode = PointMode.Circle,
                        PointSize = 18,
                        BackgroundColor = SKColors.Transparent,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 15f
                    };
                    Total = "R0,00";
                }
            }
            else if (value == "Half Radial Gauge Chart")
            {
                newChart = (entries.Count > 0)
                    ? new RadialGaugeChart { Entries = entries, LineSize = 8, BackgroundColor = SKColors.Transparent, LabelTextSize = 15f }
                    : new RadialGaugeChart { Entries = entries, LineSize = 8, BackgroundColor = SKColors.Transparent, LabelTextSize = 15f };
            }

            // Assign the new chart on the UI thread.
            if (newChart != null)
            {
                Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() => HoursIncomeChart = newChart);
            }
        }
        [RelayCommand]
        private void LoadIncomeChart()
        {
                List<Transaction> lst = new List<Transaction>();
                var transactionList = JsonSerializer.Deserialize<List<Transaction>>("");
                List<Transaction> todayTransactions = new List<Transaction>();
                var hoursTotal = new List<float>();
                if (transactionList != null)
                {
                    todayTransactions = transactionList.Where(t => t.TransactionDate.Date == DateTime.Today).ToList();
                    for (double hour = 6; hour < 19; hour++)
                    {
                        var total = todayTransactions.Where(t => t.TransactionDate.Hour == hour).Sum(a => (float)a.TotalAmount);
                        hoursTotal.Add(total);
                    }
                    Total = hoursTotal.Sum().ToString("C", new CultureInfo("en-ZA"));
                    var entries = hoursTotal.Select((value, Index) => new Microcharts.ChartEntry(value)
                    {
                      Color = SKColors.Blue,
                        Label = $"{Index + 6}:00-{Index + 6 + 1}:00",
                        ValueLabel = value.ToString("C", new CultureInfo("en-ZA")),
                    }).ToList();
                    HoursIncomeChart = new LineChart()
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
                    HoursIncomeChart = new LineChart()
                    {
                        Entries = entry,
                        LineMode = LineMode.Straight,
                        LineSize = 8,
                        PointMode = PointMode.Circle,
                        PointSize = 18,
                        BackgroundColor = SKColors.Transparent,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 15f,
                    };
                    Total = "R0,00";
                }
                HoursIncomeChart = new LineChart()
                {
                    Entries = entry,
                    LineMode = LineMode.Straight,
                    LineSize = 8,
                    PointMode = PointMode.Circle,
                    PointSize = 18,
                    BackgroundColor = SKColors.Transparent,
                    ValueLabelOrientation = Orientation.Vertical,
                    LabelTextSize = 15f,
                };
                Total = "R0,00";
        }

        [RelayCommand]
        private async Task LoadOrders()
        {
                List<Order> orders = JsonSerializer.Deserialize<List<Order>>(" ") ?? new List<Order>();
                List<Order> todayOrders = orders.Where(o => o.OrderDate.Date == DateTime.Today).ToList();
                if (todayOrders.Count > 1)
                {
                    var hoursTotals = new List<int>();
                    for (float hour = 6; hour < 19; hour++)
                    {
                        int total = todayOrders.Where(a => a.OrderDate.Hour == hour).Count();
                        hoursTotals.Add(total);
                    }
                    TotalOrders = hoursTotals.Count().ToString();
                    var entries = hoursTotals.Select((value, Index) => new Microcharts.ChartEntry(value)
                    {
                        Label = $"{Index + 6}:00-{Index + 6 + 1}:00",
                        ValueLabel = value.ToString("C", new CultureInfo("en-ZA"))
                    }).ToList();
                    OrdersChart = new LineChart()
                    {
                        Entries = entries,
                        LineMode = LineMode.Straight,
                        LineSize = 8,
                        PointMode = PointMode.Circle,
                        PointSize = 18,
                        BackgroundColor = SKColors.Transparent,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 15f,
                    };
                    return;
                }
                    OrdersChart = new BarChart()
                    {
                        Entries = entry,
                        BackgroundColor = SKColors.Transparent,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 15f,
                    };
                    TotalOrders = "0";
        }
        [RelayCommand]
        private async Task LoadItemsLeft()
        {
                List<Item> items = JsonSerializer.Deserialize<List<Item>>(" ") ?? new List<Item>();
                if (items.Count > 0)
                {
                    items = items.OrderBy(q => q.ItemQuantity).ToList();
                    List<ChartEntry> chartEntries = new List<ChartEntry>();
                    foreach (var item in items)
                    {
                        double perc = await ReturnPecent(item);
                        if(perc > 100)
                        {
                            perc = 100;
                        }
                        if(perc < 0)
                        {
                            perc = 0;
                        }
                        if(perc < 25)
                        {
                            var entry = new Microcharts.ChartEntry(item.ItemQuantity)
                            {
                                Color = SKColors.Red,
                                Label = $"{item.ItemName}",
                                ValueLabel = item.ItemQuantity.ToString()
                            };
                            chartEntries.Add(entry);
                        }
                        else if(perc >= 25 && perc < 50)
                        {
                            var entry = new Microcharts.ChartEntry(item.ItemQuantity)
                            {
                                Color = SKColors.Orange,
                                Label = $"{item.ItemName}",
                                ValueLabel = item.ItemQuantity.ToString()
                            };
                            chartEntries.Add(entry);
                        }
                        else if(perc >= 50 && perc < 75)
                        {
                            var entry = new Microcharts.ChartEntry(item.ItemQuantity)
                            {
                                Color = SKColors.Yellow,
                                Label = $"{item.ItemName}",
                                ValueLabel = item.ItemQuantity.ToString()
                            };
                            chartEntries.Add(entry);
                        }
                        else
                        {
                            var entry = new Microcharts.ChartEntry(item.ItemQuantity)
                            {
                                Color = SKColors.Green,
                                Label = $"{item.ItemName}",
                                ValueLabel = item.ItemQuantity.ToString()
                            };
                            chartEntries.Add(entry);
                        }
                    }
                    ItemsChart = new LineChart()
                    {
                        Entries = chartEntries,
                        LineMode = LineMode.Straight,
                        LineSize = 8,
                        PointMode = PointMode.Circle,
                        PointSize = 18,
                        BackgroundColor = SKColors.White,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 15f,
                    };
                    DisplaLeastItems(items);
                }
                else
                {
                    EmptyItems = "No Items To Show 😢!!!";
                    TotalItems = "0";
                }
        }
        [RelayCommand]
        private async Task DisplayIncome()
        {
            if (SelectedChart == "Pie Chart")
            {
                var entries = ReturnEntries();
                if (entries.Count > 0)
                {
                    HoursIncomeChart = new PieChart()
                    {
                        Entries = entries,
                        BackgroundColor = SKColors.Transparent,
                        LabelMode = LabelMode.RightOnly,
                        LabelTextSize = 15f,
                    };
                }
            }
            else if (SelectedChart == "Bar Chart")
            {
                var entries = ReturnEntries();
                if (entries.Count > 0)
                {
                    HoursIncomeChart = new BarChart()
                    {
                        Entries = entries,
                        BackgroundColor = SKColors.Transparent,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 15f,
                    };
                }
            }
            else if (SelectedChart == "Line Chart")
            {
                var entries = ReturnEntries();
                if (entries.Count > 0)
                {
                    HoursIncomeChart = new LineChart()
                    {
                        Entries = entries,
                        LineMode = LineMode.Straight,
                        LineSize = 8,
                        PointMode = PointMode.Circle,
                        PointSize = 18,
                        BackgroundColor = SKColors.Transparent,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 15f,
                    };
                }
            }
            else if (SelectedChart == "Half Radial Gauge Chart")
            {
                var entries = ReturnEntries();
                if (entries.Count > 0)
                {
                    HoursIncomeChart = new RadialGaugeChart()
                    {
                        Entries = entries,
                        LineSize = 8,
                        BackgroundColor = SKColors.Transparent,
                        LabelTextSize = 15f,
                    };
                }
            }
        }
        private List<ChartEntry> ReturnEntries()
        {
            var transactionList = JsonSerializer.Deserialize<List<Transaction>>(" ");
            List<Transaction> todayTransactions = new List<Transaction>();
            var hoursTotal = new List<float>();
            if (transactionList != null)
            {
                todayTransactions = transactionList.Where(t => t.TransactionDate.Date == DateTime.Today).ToList();
                for (double hour = 6; hour < 19; hour++)
                {
                    var total = todayTransactions.Where(t => t.TransactionDate.Hour == hour).Sum(a => (float)a.TotalAmount);
                    hoursTotal.Add(total);
                }
                var entries = hoursTotal.Select((value, Index) => new Microcharts.ChartEntry(value)
                {
                    Label = $"{Index + 6}:00-{Index + 6 + 1}:00",
                    ValueLabel = value.ToString("C", new CultureInfo("en-ZA")),
                }).ToList();
                return entries;
            }
            else
            {
                return new List<ChartEntry>();
            }
        }
        private async void DisplaLeastItems(List<Item> list)
        {
            List<Item> items = JsonSerializer.Deserialize<List<Item>>(" ") ?? new List<Item>();
            int count = 0;
            foreach (Item item in items)
            {
                foreach (Item i in list)
                {
                    if (i.ItemName == item.ItemName)
                    {
                        double perc = (double)(i.ItemQuantity / item.ItemQuantity) * 100;
                        if (perc < 25)
                        {
                            count++;
                            EmptyItems += $"{i.ItemName}\n";
                        }
                    }
                }
            }
            TotalItems = count.ToString();
        }
        private async Task<double> ReturnPecent(Item i)
        {
            double perc = 0;
            List<Item> items = JsonSerializer.Deserialize<List<Item>>(" ") ?? new List<Item>();
            int count = 0;
            foreach (Item item in items)
            {
                if (item.ItemName == i.ItemName)
                {
                    perc = (double)(i.ItemQuantity / item.ItemQuantity) * 100;
                    
                }
            }
            return perc;
        }
        
    }
}

