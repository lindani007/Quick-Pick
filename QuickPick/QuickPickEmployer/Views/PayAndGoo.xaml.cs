using QuickPick.QuieckPickCustomer.ViewModels;

namespace QuickPick.QuickPickEmployer.Views;

public partial class PayAndGoo : ContentPage
{
	public PayAndGoo(ChooseAiselViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}