using QuickPick.QuickPickEmployer.ViewModel;

namespace QuickPick.QuickPickEmployer.Views;

public partial class ItemsPage : ContentPage
{
	ItemViewModel _viewmodel;
	public ItemsPage(ItemViewModel itemViewModel)
	{
		InitializeComponent();
         BindingContext = itemViewModel;
        _viewmodel = itemViewModel;
	}

    private void PageRoot_Loaded(object sender, EventArgs e)
    {
        _viewmodel.LoadItemsCommand.Execute(null);
    }
}