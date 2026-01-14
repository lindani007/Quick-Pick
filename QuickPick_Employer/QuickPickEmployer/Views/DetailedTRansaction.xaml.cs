using QuickPick_Employer.QuickPickEmployer.Models;
using System.Globalization;

namespace QuickPick_Employer.QuickPickEmployer.Views;

public partial class DetailedTRansaction : ContentPage
{
	readonly List<BoughtItem> boughtItems;
	public DetailedTRansaction(List<BoughtItem> items)
	{
		InitializeComponent();
		boughtItems = items;
		DisplayTransacts();
	}
	private void DisplayTransacts()
	{
		foreach (var item in boughtItems)
		{
			lblName.Text += $"\n{item.ItemName}";
			lblDate.Text += $"\n{item.TransactionDtae.ToString("yyyy-MMM-dd-H-mm-tt")}";
			lblPrice.Text += $"\n{item.Price.ToString("C", new CultureInfo("en-ZA"))}";
			lblquantity.Text += $"\n{item.Quantity.ToString()}";
			lblearned.Text += $"\n{item.TotalAmount.ToString("C", new CultureInfo("en-ZA"))}";
			lblPackedBy.Text += $"\n{item.Packed_By}";
		}
	}
}