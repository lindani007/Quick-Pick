using QuickPick.QuickPickEmployer.ViewModel;

namespace QuickPick.QuickPickEmployer.Views;

public partial class ChooseAisle : ContentPage
{
	ItemViewModel _viewModel;
    public ChooseAisle(ItemViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		_viewModel = vm;
    }

    private void PageRoot_Loaded(object sender, EventArgs e)
    {
        _viewModel.LoadAislesCommand.Execute(null);
    }
}