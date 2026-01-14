using QuickPick_Customer.QuieckPickCustomer.ViewModels;

namespace QuickPick_Customer.QuieckPickCustomer.Views;

public partial class ItemPage : ContentPage
{
	public ItemPage(ChooseAiselViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}