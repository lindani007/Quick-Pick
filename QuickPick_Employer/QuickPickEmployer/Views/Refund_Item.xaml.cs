using QuickPick_Employer.QuickPickEmployer.ViewModel;

namespace QuickPick_Employer.QuickPickEmployer.Views;

public partial class Refund_Item : ContentPage
{
	public Refund_Item(TransactionListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}