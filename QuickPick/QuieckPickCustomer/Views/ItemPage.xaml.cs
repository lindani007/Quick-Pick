using QuickPick.QuieckPickCustomer.ViewModels;

namespace QuickPick.QuieckPickCustomer.Views;

public partial class ItemPage : ContentPage
{
	public ItemPage(ChooseAiselViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}