using QuickPick.QuieckPickCustomer.ViewModels;

namespace QuickPick.QuieckPickCustomer.Views;

public partial class Cart : ContentPage
{
	public Cart(ChooseAiselViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}