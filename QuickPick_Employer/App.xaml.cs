using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuickPick_Employer.QuickPickEmployer.ViewModel;
using QuickPick_Employer.QuickPickEmployer.Views;
using QuickPickBlobService;
using QuickPickDBApiService.Models; 
using QuickPickDBApiService.Services;
using QuickPickSignlaRService.Services;

namespace QuickPick_Employer
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
            var blobApiBaseUrl = new QuickPickBlobService.Model.ApiBaseUrl { BaseUrl = "https://localhost:7279" };

            //Blob Services
            ImageStorege imageStorege = new ImageStorege(blobApiBaseUrl.BaseUrl);

            //SignalR Services
            SignlaRAisleService signlaRAisleService = new SignlaRAisleService();
            SignalRItemService signalRItem = new SignalRItemService();
            SignalROrderService signalROrderService = new SignalROrderService();

            //DB Api Services
            StockService stockService = new StockService(apiBaseUrl);
            ItemService itemService = new ItemService(apiBaseUrl);
            AisleService aisleService = new AisleService(apiBaseUrl);
            DbBoughtItemService dbBoughtItemService = new DbBoughtItemService(apiBaseUrl);
            SlesService ss = new SlesService(apiBaseUrl);
            OrderService orderService = new OrderService(apiBaseUrl);

            //ViewModels
            HomeViewModel viewModel = new HomeViewModel(itemService,orderService,ss,signalROrderService,dbBoughtItemService);
            ViewModelAisle aisleViewModel = new ViewModelAisle(aisleService,signlaRAisleService,imageStorege);
            ItemViewModel itemViewModel = new ItemViewModel(itemService, aisleService, aisleViewModel, signlaRAisleService,signalRItem, stockService, imageStorege);
            SalesAnalysisViewModel s = new SalesAnalysisViewModel();
            TransactionListViewModel transactionListViewModel = new TransactionListViewModel(ss,itemService,orderService,dbBoughtItemService,itemService);
            TodayTransactionViewModel t = new TodayTransactionViewModel(ss,dbBoughtItemService, itemService, orderService, transactionListViewModel);

            //Main Page
            return new Window(new NavigationPage(new DashBoard(viewModel, itemViewModel, aisleViewModel, t, transactionListViewModel, s)));
        }
    }
}