using QuickPick_Customer.QuieckPickCustomer.ViewModels;

namespace QuickPick_Customer.QuieckPickCustomer.Views;

public partial class ChooseAisle : ContentPage
{
	public ChooseAisle(ChooseAiselViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}