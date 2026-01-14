using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickPickDBApi.Models;
using QuickPickDBApi.Models.dbContext_folder;

namespace QuickPickDBApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoughtItemController : ControllerBase
    {
        private readonly QuickPickDbContext _dbContext;
        public BoughtItemController(QuickPickDbContext quickPickDbContext)
        {
            _dbContext = quickPickDbContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<BoughtItem>> GetAllItems()
        {
            try
            {
                return _dbContext.BoughtItems;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<List<BoughtItem>>> CreateItem(List<BoughtItem> items)
        {
            try
            {
                _dbContext.BoughtItems.AddRange(items);
                await _dbContext.SaveChangesAsync();
                return items;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult<List<BoughtItem>>> UpdateItem(List<BoughtItem> items)
        {
            try
            {
                _dbContext.BoughtItems.UpdateRange(items);
                await _dbContext.SaveChangesAsync();
                return items;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteItem(int id)
        {
            try
            {
                var item = _dbContext.Items.Find(id);
                if (item == null)
                {
                    return NotFound();
                }
                _dbContext.Items.Remove(item);
                await _dbContext.SaveChangesAsync();
                return $"Item removed succesful {item}";
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
