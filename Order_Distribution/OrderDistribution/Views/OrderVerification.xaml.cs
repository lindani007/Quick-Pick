using Order_Distribution.OrderDistribution.ViwewModels;

namespace Order_Distribution.OrderDistribution.Views;

public partial class OrderVerification : ContentPage
{
	public OrderVerification(OrdersViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}