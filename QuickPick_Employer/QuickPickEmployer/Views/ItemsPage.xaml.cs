using QuickPick_Employer.QuickPickEmployer.ViewModel;

namespace QuickPick_Employer.QuickPickEmployer.Views;

public partial class ItemsPage : ContentPage
{
	ItemViewModel _viewmodel;
	public ItemsPage(ItemViewModel itemViewModel)
	{
		InitializeComponent();
         BindingContext = itemViewModel;
        _viewmodel = itemViewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.LoadItemsCommand.Execute(null);
    }
}