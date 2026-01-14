using QuickPick_Employer.QuickPickEmployer.ViewModel;

namespace QuickPick_Employer.QuickPickEmployer.Views;

public partial class Ordered_Items : ContentPage
{
	public Ordered_Items(TransactionListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}