using QuickPick.QuickPickEmployer.ViewModel;

namespace QuickPick.QuickPickEmployer.Views;

public partial class TransactionsList : ContentPage
{
	public TransactionsList(TransactionListViewModel transactionsList)
	{
		InitializeComponent();
		BindingContext = transactionsList;
    }
}