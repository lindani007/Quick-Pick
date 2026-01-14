using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using QuickPick_Employer.QuickPickEmployer.Models;
using QuickPick_Employer.QuickPickEmployer.Views;
using QuickPickDBApiService.Services;
using QuickPickSignlaRService.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuickPick_Employer.QuickPickEmployer.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<string> leastItems = new ObservableCollection<string>();
        List<ChartEntry> OrdersEntries = new List<ChartEntry>();
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
        private int totalItems;
        [ObservableProperty]
        private string total;
        ChartEntry[] entry = new[]
    {
        new ChartEntry(0)
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
        new ChartEntry(0)
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
        ItemService _itemService;
        OrderService _orderService;
        SlesService _saleService;
        DbBoughtItemService _dbBoughtItemService;
        SignalROrderService _signalROrderService;
        public HomeViewModel(ItemService itemService, OrderService orderService,
            SlesService saleService, SignalROrderService signalROrderService, DbBoughtItemService dbBoughtItemService)
        {
            _saleService = saleService;
            _itemService = itemService;
            _orderService = orderService;
            _signalROrderService = signalROrderService;
            _dbBoughtItemService = dbBoughtItemService;
            LoadIncomeChart();
            LoadOrders();
            LoadItemsLeft();
            SelectedChart = "Line Chart";
            StartConnection();
        }
        private async void StartConnection()

        {
           await _signalROrderService.ConnectSignlaR();
            _signalROrderService.OrderReceied -= RealTimeOrderUpade;
            _signalROrderService.OrderReceied += RealTimeOrderUpade;
        }
        partial void OnSelectedChartChanged(string? value)
        {
            // Fire-and-forget update to allow UI to clear first and then set new chart.
            _ = UpdateHoursChartAsync(value);
        }
        [RelayCommand]
        private async Task LoadIncomeChart()
        {
            var transactionList = await ReturnTransactions();
            List<Transaction> todayTransactions = new List<Transaction>();
            var hoursTotal = new List<float>();
            if (transactionList != null)
            {
                todayTransactions = transactionList.Where(t => t.TransactionDate.Date == DateTime.Today).ToList();
                for (double hour = 6; hour <= 19; hour++)
                {
                    var total = todayTransactions.Where(t => t.TransactionDate.Hour == hour).Sum(a => (float)a.TotalAmount);
                    hoursTotal.Add(total);
                }
                Total = hoursTotal.Sum().ToString("C", new CultureInfo("en-ZA"));
                var entries = hoursTotal.Select( (value, Index) => new Microcharts.ChartEntry(value)
                {
                    Color = SKColors.LimeGreen,
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
                    ValueLabelOrientation = Orientation.Vertical,
                    LabelTextSize = 15f,
                };
                Total = "R0,00";
            }
        }

        [RelayCommand]
        private async Task LoadOrders()
        {
            List<Order> orders = await ReturnOrders();
            List<Order> todayOrders = orders.Where(o => o.OrderDate.Date == DateTime.Today).ToList();
            TotalOrders = todayOrders.Count.ToString();
            if (todayOrders.Count > 0)
            {
                var hoursTotals = new List<int>();
                for (float hour = 6; hour <= 19; hour++)
                {
                    int total = todayOrders.Where(a => a.OrderDate.Hour == hour).Count();
                    hoursTotals.Add(total);
                }
                OrdersEntries = hoursTotals.Select((value, Index) => new Microcharts.ChartEntry(value)
                {
                    Label = $"{Index + 6}:00-{Index + 6 + 1}:00",
                    ValueLabel = value.ToString(),
                    Color = SKColors.LimeGreen
                }).ToList();
                OrdersChart = new LineChart()
                {
                    Entries = OrdersEntries,
                    LineMode = LineMode.Straight,
                    LineSize = 8,
                    PointMode = PointMode.Circle,
                    PointSize = 18,
                    ValueLabelOrientation = Orientation.Vertical,
                    LabelTextSize = 15f,
                };
                return;
            }
            OrdersChart = new BarChart()
            {
                Entries = entry,
                ValueLabelOrientation = Orientation.Vertical,
                LabelTextSize = 15f,
            };
            TotalOrders = "0";
        }
        [RelayCommand]
        private async Task LoadItemsLeft()
        {
            List<ChartEntry> chartEntries = new List<ChartEntry>();
            List<Item> items = await ReturnItemsList();
            if (items.Count > 0)
            {
                items = items.OrderBy(q => q.LeftQuantity).ToList();
                foreach (var item in items)
                {

                    double perc = await ReturnPecent(item);
                    if (perc > 100)
                    {
                        perc = 100;
                    }
                    if (perc < 0)
                    {
                        perc = 0;
                    }
                    if (perc < 25)
                    {
                            var entry = new Microcharts.ChartEntry(item.LeftQuantity)
                            {
                                Color = SKColors.Red,
                                Label = $"{item.ItemName}",
                                ValueLabel = item.LeftQuantity.ToString()
                            };
                            chartEntries.Add(entry);
                    }
                    else if (perc >= 25 && perc < 50)
                    {
                            var entry = new Microcharts.ChartEntry(item.LeftQuantity)
                            {
                                Color = SKColors.Orange,
                                Label = $"{item.ItemName}",
                                ValueLabel = item.LeftQuantity.ToString()
                            };
                            chartEntries.Add(entry);
                    }
                    else if (perc >= 50 && perc < 75)
                    {
                            var entry = new Microcharts.ChartEntry(item.LeftQuantity)
                            {
                                Color = SKColors.Yellow,
                                Label = $"{item.ItemName}",
                                ValueLabel = item.LeftQuantity.ToString()
                            };
                            chartEntries.Add(entry);
                    }
                    else
                    {
                            var entry = new Microcharts.ChartEntry(item.LeftQuantity)
                            {
                                Color = SKColors.LimeGreen,
                                Label = $"{item.ItemName}",
                                ValueLabel = item.LeftQuantity.ToString()
                            };
                            chartEntries.Add(entry);
                    }
                }
                DisplaLeastItems(items);
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
            }
            else
            {
                EmptyItems = "No Items To Show 😢!!!";
                TotalItems = 0;
            }
        }
        [RelayCommand]
        private async Task DisplayIncome()
        {
            if (SelectedChart == "Pie Chart")
            {
                var entries = await ReturnEntries();
                if (entries.Count > 0)
                {
                    HoursIncomeChart = new PieChart()
                    {
                        Entries = entries,
                        LabelMode = LabelMode.RightOnly,
                        LabelTextSize = 15f,
                    };
                }
            }
            else if (SelectedChart == "Bar Chart")
            {
                var entries = await ReturnEntries();
                if (entries.Count > 0)
                {
                    HoursIncomeChart = new BarChart()
                    {
                        Entries = entries,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 15f,
                    };
                }
            }
            else if (SelectedChart == "Line Chart")
            {
                var entries = await ReturnEntries();
                if (entries.Count > 0)
                {
                    HoursIncomeChart = new LineChart()
                    {
                        Entries = entries,
                        LineMode = LineMode.Straight,
                        LineSize = 8,
                        PointMode = PointMode.Circle,
                        PointSize = 18,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 15f,
                    };
                }
            }
            else if (SelectedChart == "Half Radial Gauge Chart")
            {
                var entries = await ReturnEntries();
                if (entries.Count > 0)
                {
                    HoursIncomeChart = new RadialGaugeChart()
                    {
                        Entries = entries,
                        LineSize = 8,
                        LabelTextSize = 15f,
                    };
                }
            }
        }
        private async Task UpdateHoursChartAsync(string? value)
        {
            // Clear chart on UI thread first so the control can remove the previous visuals.
            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() => HoursIncomeChart = null);

            // Give the UI a moment to process the clear. Small delay avoids blocking.
            await Task.Delay(50).ConfigureAwait(false);

            var entries = await ReturnEntries();

            Chart newChart = null;

            if (value == "Pie Chart")
            {
                newChart = (entries.Count > 0)
                    ? new PieChart { Entries = entries, LabelMode = LabelMode.RightOnly, LabelTextSize = 15f }
                    : new PieChart { Entries = entry, BackgroundColor = SKColors.Transparent, LabelTextSize = 15f };
            }
            else if (value == "Bar Chart")
            {
                foreach (var color in entries)
                {
                    color.Color = SKColors.LimeGreen;
                }
                newChart = (entries.Count > 0)
                    ? new BarChart { Entries = entries, ValueLabelOrientation = Orientation.Vertical, LabelTextSize = 15f }
                    : new BarChart { Entries = entry, ValueLabelOrientation = Orientation.Vertical, LabelTextSize = 15f };
            }
            else if (value == "Line Chart")
            {
                if (entries.Count > 0)
                {
                    foreach(var color in entries)
                    {
                        color.Color = SKColors.LimeGreen;
                    }
                    newChart = new LineChart
                    {
                        Entries = entries,
                        LineMode = LineMode.Straight,
                        LineSize = 8,
                        PointMode = PointMode.Circle,
                        PointSize = 18,
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 15f,
                        
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
                        ValueLabelOrientation = Orientation.Vertical,
                        LabelTextSize = 15f
                    };
                    Total = "R0,00";
                }
            }
            else if (value == "Half Radial Gauge Chart")
            {
                newChart = (entries.Count > 0)
                    ? new RadialGaugeChart { Entries = entries, LineSize = 8, LabelTextSize = 15f }
                    : new RadialGaugeChart { Entries = entries, LineSize = 8, LabelTextSize = 15f };
            }

            // Assign the new chart on the UI thread.
            if (newChart != null)
            {
                Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() => HoursIncomeChart = newChart);
            }
        }
        private async Task<List<ChartEntry>> ReturnEntries()
        {
            var transactionList = await ReturnTransactions();
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
                var entries = hoursTotal.Select( (value, Index) => new Microcharts.ChartEntry(value)
                {
                    Color = ChangeEntryColor((Index + 6)),
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
        private async void DisplaLeastItems(List<Item> items)
        {
            int count = 0;
            foreach (Item item in items)
            {
                double perc = (double.Parse(item.LeftQuantity.ToString()) / double.Parse(item.ItemQuantity.ToString() )) * 100;
                if (perc <= 25)
                {
                    count++;
                    LeastItems.Add(item.ItemName);
                }
            }
            TotalItems = count;
        }
        private async Task<double> ReturnPecent(Item i)
        {
            double perc = 0;
           perc = (double.Parse(i.LeftQuantity.ToString()) / double.Parse(i.ItemQuantity.ToString())) * 100;
            return perc;
        }
        private async Task<List<Item>> ReturnItemsList()
        {
            var l = await _itemService.GetItemsAsync();
            List<Item> Items = l.Select(propa => new Item
            {
                ItemId = propa.ID,
                ItemName = propa.Item_Name,
                ItemDescription = propa.Description,
                ItemPrice = propa.Price,
                ItemQuantity = propa.Quantity,
                LeftQuantity  = propa.LeftQuantity,
                AisleId = propa.Aisle_Id,
                ImageSource = propa.ImageUrl,
            }).ToList();
            return Items;
        }
        private async Task<List<Order>> ReturnOrders()
        {
            var l = await _orderService.GetOrdersAsync();
            List<Order> List = l.Select(t => new Order
            {
                OrderId = t.Id,
                OrderNumber = t.Order_Number,
                OrderDate = t.Occured_On,
                OrderedItemsQty = t.Quantity,
                Code = t.Code
            }).ToList();
            return List;
        }
        private async Task<List<Transaction>> ReturnTransactions()
        {
            var l = await _dbBoughtItemService.GetItemsAsync();
            
            List<Transaction> transactionList = l.Select(t => new Transaction
            {
                TransactionId = t.TransactionId,
                TotalAmount = t.TotalAmount,
                Quantity = t.Quantity,
                Packed_By = t.Packed_By,
                TransactionDate = t.TransactionDtae,
            }).ToList();
            return transactionList;
        }
        private SKColor ChangeEntryColor(double hour)
        {
            if (hour == 6)
            {
                return SKColors.Orange;
            }
            else if (hour == 7)
            {
                return SKColors.DarkOrange;
            }
            else if (hour == 8)
            {
                return SKColors.OrangeRed;
            }
            else if (hour == 9)
            {
                return SKColors.Yellow;
            }
            else if (hour == 10)
            {
                return SKColors.YellowGreen;
            }
            else if (hour == 11)
            {
                return SKColors.GreenYellow;
            }
            else if (hour == 12)
            {
                return SKColors.LightGoldenrodYellow;
            }
            else if (hour == 13)
            {
                return SKColors.LightYellow;
            }
            if (hour == 14)
            {
                return SKColors.LightGreen;
            }
            else if (hour == 15)
            {
                return SKColors.LightSeaGreen;
            }
            else if (hour == 16)
            {
                return SKColors.ForestGreen;
            }
            else if (hour == 17)
            {
                return SKColors.LawnGreen;
            }
            else if (hour == 18)
            {
                return SKColors.LimeGreen;
            }
            else if (hour == 19)
            {
                return SKColors.MediumSeaGreen;
            }
            else
            {
                return SKColors.LimeGreen;
            }
        }
        public async void RealTimeOrderUpade(QuickPickSignlaRService.Models.Order order)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if(string.IsNullOrEmpty(TotalOrders))
                {
                    TotalOrders = "0";
                }
                int orders = int.Parse(TotalOrders);
                TotalOrders = (orders + 1 ).ToString();
               await LoadOrders();
                await LoadIncomeChart();
                await LoadItemsLeft();
            });
        }
        private async void RealTimeItemsUpdate(List<QuickPickSignlaRService.Models.BoughtItem> items)
        {

        }
    }
}

