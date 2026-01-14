using QuickPick_Employer.QuickPickEmployer.ViewModel;

namespace QuickPick_Employer.QuickPickEmployer.Views;

public partial class AddItems : ContentPage
{
	public AddItems(ItemViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}