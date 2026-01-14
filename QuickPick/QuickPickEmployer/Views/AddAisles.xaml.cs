using QuickPick.QuickPickEmployer.ViewModel;

namespace QuickPick.QuickPickEmployer.Views;

public partial class AddAisles : ContentPage
{
    ViewModelAisle _viewmodel;
	public AddAisles(ViewModelAisle vm)
	{
		InitializeComponent();
		BindingContext = vm;
        _viewmodel = vm;
    }

    private void PageRoot_Unloaded(object sender, EventArgs e)
    {
        _viewmodel.UloadAislesCommand.Execute(null);
    }
}