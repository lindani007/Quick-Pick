using QuickPick.QuickPickEmployer.ViewModel;

namespace QuickPick.QuickPickEmployer.Views;

public partial class SalesAnalysis : ContentPage
{
	public SalesAnalysis(SalesAnalysisViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}