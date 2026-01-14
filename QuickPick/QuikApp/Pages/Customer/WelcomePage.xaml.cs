using System.Threading.Tasks;

namespace QuickPick.QuikApp.Pages.Customer;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new Isels());
    }
}