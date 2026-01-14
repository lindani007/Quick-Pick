using QuickPick_Employer.QuickPickEmployer.ViewModel;

namespace QuickPick_Employer.QuickPickEmployer.Views;

public partial class DashBoard : FlyoutPage
{
    HomeViewModel _viewModel;
    ItemViewModel _itemViewModel;
    ViewModelAisle _aisleViewModel;
    TodayTransactionViewModel _TtransactionViewModel;
    TransactionListViewModel _transactionsvm;
    SalesAnalysisViewModel _salesAnalysisViewModel;
    public DashBoard(HomeViewModel homeViewModel, ItemViewModel itemViewModel, 
        ViewModelAisle aisleViewModel, TodayTransactionViewModel transactionViewModel, 
        TransactionListViewModel transactionsvm, SalesAnalysisViewModel s)
    {
        InitializeComponent();
        _viewModel = homeViewModel;
        _itemViewModel = itemViewModel;
        Detail = new NavigationPage(new Home(_viewModel));
        _aisleViewModel = aisleViewModel;
        _TtransactionViewModel = transactionViewModel;
        _transactionsvm = transactionsvm;
        _salesAnalysisViewModel = s;
    }
    private void HomeClicked(object sender, EventArgs e)
    {
        Detail = new NavigationPage(new Home(_viewModel));
        IsPresented = false;
    }
    private void AddAislesClicked(object sender, EventArgs e)
    {
        Detail = new NavigationPage(new AddAisles(_aisleViewModel));
        IsPresented = false;
    }
    private void AddItemsClicked(object sender, EventArgs e)
    {
        Detail = new NavigationPage(new AddedItems(_itemViewModel));
        IsPresented = false;
    }
    private void TransactionListClicked(object sender, EventArgs e)
    {
         Detail = new NavigationPage(new TransactionsList(_transactionsvm));
        IsPresented = false;
    }
    private void TodayTransactionsClicked(object sender, EventArgs e)
    {
              Detail = new NavigationPage(new TodayTransactions(_TtransactionViewModel));
        IsPresented = false;
    }
    private void SalesAnalysisClicked(object sender, EventArgs e)
    {
           Detail = new NavigationPage(new SalesAnalysis(_salesAnalysisViewModel));
        IsPresented = false;
    }
}