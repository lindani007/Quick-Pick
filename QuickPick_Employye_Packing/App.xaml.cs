using Microsoft.Extensions.DependencyInjection;
using QuickPick_Employye_Packing;
using QuickPick_Employye_Packing.QuickPick_Empleyee_Packing_Folder.ViewModels;
using QuickPick_Employye_Packing.QuickPick_Empleyee_Packing_Folder.Views;
using QuickPickDBApiService.Models;
using QuickPickDBApiService.Services;
using QuickPickSignlaRService.Services;
namespace QuickPick_Employye_Packing
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var apibaseurl = new ApiBaseUrl()
            {
                BaseUrl = "https://localhost:7148"
            };
            OrderService orderService = new OrderService(apibaseurl);
            LoginService loginService = new LoginService(apibaseurl);
            DbBoughtItemService dbBoughtItemService = new DbBoughtItemService(apibaseurl);
            SignalROrderService signalROrderService = new SignalROrderService();
            SignalROrderdItemService signalROrderdItemService = new SignalROrderdItemService();
            OrdersViewModel ordersViewModel = new OrdersViewModel(signalROrderdItemService,signalROrderService,orderService,dbBoughtItemService,loginService);
            return new Window(new NavigationPage(new Login(ordersViewModel)));
        }
    }
}