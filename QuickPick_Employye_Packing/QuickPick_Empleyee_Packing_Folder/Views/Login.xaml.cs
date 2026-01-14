using QuickPick_Employye_Packing.QuickPick_Empleyee_Packing_Folder.ViewModels;

namespace QuickPick_Employye_Packing.QuickPick_Empleyee_Packing_Folder.Views;

public partial class Login : ContentPage
{
	public Login(OrdersViewModel ordersViewModel)
	{
		InitializeComponent();
		BindingContext = ordersViewModel;
    }
}