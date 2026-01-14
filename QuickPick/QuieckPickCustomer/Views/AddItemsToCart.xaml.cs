using QuickPick.QuieckPickCustomer.ViewModels;

namespace QuickPick.QuieckPickCustomer.Views;

public partial class AddItemsToCart : ContentPage
{
	public AddItemsToCart(ChooseAiselViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}