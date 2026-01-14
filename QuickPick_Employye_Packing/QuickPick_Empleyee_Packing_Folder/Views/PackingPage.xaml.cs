using QuickPick_Employye_Packing.QuickPick_Empleyee_Packing_Folder.ViewModels;

namespace QuickPick_Employye_Packing.QuickPick_Empleyee_Packing_Folder.Views;

public partial class PackingPage : ContentPage
{
	public PackingPage(OrdersViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}