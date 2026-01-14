using QuickPick_Customer.QuieckPickCustomer.ViewModels;

namespace QuickPick_Customer.QuieckPickCustomer.Views;

public partial class PayAndGoo : ContentPage
{
	public PayAndGoo(ChooseAiselViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}