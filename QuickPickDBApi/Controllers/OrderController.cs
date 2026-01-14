using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickPickDBApi.Models;
using QuickPickDBApi.Models.dbContext_folder;

namespace QuickPickDBApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly QuickPickDbContext _dbContext;
        public OrderController(QuickPickDbContext quickPickDbContext)
        {
            _dbContext = quickPickDbContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAllOrders()
        {
            try
            {
                return _dbContext.Orders;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            try
            {
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAllOrders), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult> UpdateOrder(Order order)
        {
            try
            {
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                var order = _dbContext.Orders.Find(id);
                if (order == null)
                {
                    return NotFound();
                }
                _dbContext.Orders.Remove(order);
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
