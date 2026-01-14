using QuickPick_Employer.QuickPickEmployer.ViewModel;

namespace QuickPick_Employer.QuickPickEmployer.Views;

public partial class TodayTransactions : ContentPage
{
	public TodayTransactions(TodayTransactionViewModel todayTransactionViewModel)
	{
		InitializeComponent();
		BindingContext = todayTransactionViewModel;
	}
}