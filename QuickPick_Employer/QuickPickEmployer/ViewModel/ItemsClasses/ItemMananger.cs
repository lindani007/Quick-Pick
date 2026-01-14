using QuickPickDBApiService.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPick_Employer.QuickPickEmployer.ViewModel.ItemsClasses
{
    public class ItemMananger
    {
        StockService _stockService;
        ItemService _itemService;
        public ItemMananger(StockService stockService, ItemService itemService)
        {
            _stockService = stockService;
            _itemService = itemService;
        }
        public async Task AddItem(string name, string description,double price, int quantity,string imageurl, int aisleid)
        {
          
        }
    }
}
