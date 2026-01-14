using QuickPick.QuickPickEmployer.ViewModel;

namespace QuickPick.QuickPickEmployer.Views;

public partial class TodayTransactions : ContentPage
{
	public TodayTransactions(TodayTransactionViewModel todayTransactionViewModel)
	{
		InitializeComponent();
		BindingContext = todayTransactionViewModel;
	}
}