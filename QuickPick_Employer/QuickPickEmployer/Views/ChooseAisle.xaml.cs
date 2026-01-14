using QuickPick_Employer.QuickPickEmployer.ViewModel;

namespace QuickPick_Employer.QuickPickEmployer.Views;

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