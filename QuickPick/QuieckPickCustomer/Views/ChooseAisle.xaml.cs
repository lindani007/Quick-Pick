using QuickPick.QuieckPickCustomer.ViewModels;

namespace QuickPick.QuieckPickCustomer.Views;

public partial class ChooseAisle : ContentPage
{
	public ChooseAisle(ChooseAiselViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}