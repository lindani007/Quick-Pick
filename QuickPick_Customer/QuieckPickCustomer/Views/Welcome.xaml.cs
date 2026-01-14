using QuickPick_Customer.QuieckPickCustomer.ViewModels;

namespace QuickPick_Customer.QuieckPickCustomer.Views;

public partial class Welcome : ContentPage
{
    ChooseAiselViewModel viewModel;
	public Welcome(ChooseAiselViewModel vm)
	{
		InitializeComponent();
        viewModel = vm;
	}
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChooseAisle(viewModel));
    }
}