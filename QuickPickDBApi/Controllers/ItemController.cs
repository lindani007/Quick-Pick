using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickPickDBApi.Models;
using QuickPickDBApi.Models.dbContext_folder;

namespace QuickPickDBApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly QuickPickDbContext _dbContext;
        public ItemController(QuickPickDbContext quickPickDbContext)
        {
            _dbContext = quickPickDbContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Item>> GetAllItems()
        {
            try
            {
                return _dbContext.Items;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem(Item item)
        {
            try
            {
                _dbContext.Items.Add(item);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAllItems), new { id = item.ID }, item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult> UpdateItem(Item item)
        {
            try
            {
                _dbContext.Items.Update(item);
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
                var item = _dbContext.Items.Find(id);
                if (item == null)
                {
                    return NotFound();
                }
                _dbContext.Items.Remove(item);
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
