using Order_Distribution.OrderDistribution.ViwewModels;

namespace Order_Distribution.OrderDistribution.Views;

public partial class ReadyOrders : ContentPage
{
	public ReadyOrders(OrdersViewModel ordersViewModel)
	{
		InitializeComponent();
		BindingContext = ordersViewModel;
    }
}