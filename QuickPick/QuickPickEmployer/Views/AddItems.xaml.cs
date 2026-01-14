using QuickPick.QuickPickEmployer.ViewModel;

namespace QuickPick.QuickPickEmployer.Views;

public partial class AddItems : ContentPage
{
	public AddItems(ItemViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}