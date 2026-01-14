using QuickPick_Employer.QuickPickEmployer.ViewModel;

namespace QuickPick_Employer.QuickPickEmployer.Views
{
    public partial class TransactionsList : ContentPage
    {
    	public TransactionsList(TransactionListViewModel transactionsList)
    	{
    		InitializeComponent();
    		BindingContext = transactionsList;
        }
    }
}