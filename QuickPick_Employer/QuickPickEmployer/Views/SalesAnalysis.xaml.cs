using QuickPick_Employer.QuickPickEmployer.ViewModel;

namespace QuickPick_Employer.QuickPickEmployer.Views;

public partial class SalesAnalysis : ContentPage
{
	public SalesAnalysis(SalesAnalysisViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}