using Microsoft.Extensions.DependencyInjection;
using QuickPick.QuickPickEmployer.ViewModel;
using QuickPick.QuickPickEmployer.Views;
using QuickPick.QuieckPickCustomer.ViewModels;
using QuickPick.QuieckPickCustomer.Views;
using QuickPick.QuikApp.Pages.Customer;
using QuickPick.QuikApp.Pages.Workers;

namespace QuickPick
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            HomeViewModel viewModel = new HomeViewModel();
            ItemViewModel itemViewModel = new ItemViewModel();
            ViewModelAisle aisleViewModel = new ViewModelAisle();
            TodayTransactionViewModel t = new TodayTransactionViewModel();
            SalesAnalysisViewModel s = new SalesAnalysisViewModel();
            TransactionListViewModel transactionListViewModel = new TransactionListViewModel();
           // return new Window(new NavigationPage(new DashBoard(viewModel,itemViewModel,aisleViewModel,t,transactionListViewModel,s)));
            ChooseAiselViewModel a = new ChooseAiselViewModel();

            return new Window(new NavigationPage(new Welcome(a)));
        }
    }
}