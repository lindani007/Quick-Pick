using Microsoft.Extensions.DependencyInjection;
using Order_Distribution.OrderDistribution.Views;
using Order_Distribution.OrderDistribution.ViwewModels;
using QuickPickDBApiService.Models;
using QuickPickDBApiService.Services;
using QuickPickSignlaRService.Services;
namespace Order_Distribution
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var apiBaseUrl = new ApiBaseUrl() { BaseUrl = "https://localhost:7148" };
            OrderService orderService = new OrderService(apiBaseUrl);
            SignalROrderService signalROrderService = new SignalROrderService();
            OrdersViewModel ordersViewModel = new OrdersViewModel(orderService, signalROrderService);
            return new Window(new NavigationPage(new ReadyOrders( ordersViewModel)));
        }
    }
}