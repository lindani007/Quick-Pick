using Microsoft.Extensions.DependencyInjection;
using QuickPick_Customer.QuieckPickCustomer.ViewModels;
using QuickPick_Customer.QuieckPickCustomer.Views;
using QuickPickDBApiService.Models;
using QuickPickDBApiService.Services;
using QuickPickSignlaRService.Services;

namespace QuickPick_Customer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var apiBaseUrl = new ApiBaseUrl { BaseUrl = "https://localhost:7148" };
            SignalROrderdItemService signalROrderdItemService = new SignalROrderdItemService();
            SignlaRAisleService signlaRAisleService = new SignlaRAisleService();
            SignalRItemService signalRItemService = new SignalRItemService();
            SignalROrderService signalROrderService = new SignalROrderService();
            DbBoughtItemService dbBoughtItemService = new DbBoughtItemService(apiBaseUrl);
            ItemService itemService = new ItemService(apiBaseUrl);
            AisleService aisleService = new AisleService(apiBaseUrl);
            SlesService ss = new SlesService(apiBaseUrl);
            OrderService orderService = new OrderService(apiBaseUrl);
            ChooseAiselViewModel a = new ChooseAiselViewModel(itemService,aisleService,ss,orderService, signalRItemService, signlaRAisleService,
                signalROrderService,signalROrderdItemService,dbBoughtItemService);

            return new Window(new NavigationPage(new Welcome(a)));
        }
    }
}