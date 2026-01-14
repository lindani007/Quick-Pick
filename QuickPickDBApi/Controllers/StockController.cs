using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickPickDBApi.Models;
using QuickPickDBApi.Models.dbContext_folder;

namespace QuickPickDBApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly QuickPickDbContext _dbContext;
        public StockController(QuickPickDbContext quickPickDbContext)
        {
            _dbContext = quickPickDbContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Stock>> GetAllItems()
        {
            try
            {
                return _dbContext.Stocks;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Stock>> CreateItem(Stock item)
        {
            try
            {
                _dbContext.Stocks.Add(item);
                await _dbContext.SaveChangesAsync();
                return item;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult> UpdateItem(Stock item)
        {
            try
            {
                _dbContext.Stocks.Update(item);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            try
            {
                var item = _dbContext.Stocks.Find(id);
                if (item == null)
                {
                    return NotFound();
                }
                _dbContext.Stocks.Remove(item);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
