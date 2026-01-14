using QuickPick_Employer.QuickPickEmployer.ViewModel;

namespace QuickPick_Employer.QuickPickEmployer.Views;

public partial class AddAisles : ContentPage
{
    ViewModelAisle _viewmodel;
	public AddAisles(ViewModelAisle vm)
	{
		InitializeComponent();
		BindingContext = vm;
        _viewmodel = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.StartConnection();
    }
    private void PageRoot_Unloaded(object sender, EventArgs e)
    {
        _viewmodel.UloadAislesCommand.Execute(null);
    }
}