using QuickPick_Customer.QuieckPickCustomer.ViewModels;

namespace QuickPick_Customer.QuieckPickCustomer.Views;

public partial class Cart : ContentPage
{
	public Cart(ChooseAiselViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}