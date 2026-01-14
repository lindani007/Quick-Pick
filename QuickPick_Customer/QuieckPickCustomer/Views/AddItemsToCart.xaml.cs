using QuickPick_Customer.QuieckPickCustomer.ViewModels;

namespace QuickPick_Customer.QuieckPickCustomer.Views;

public partial class AddItemsToCart : ContentPage
{
	public AddItemsToCart(ChooseAiselViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}