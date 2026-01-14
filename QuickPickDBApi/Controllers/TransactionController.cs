using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickPickDBApi.Models;
using QuickPickDBApi.Models.dbContext_folder;

namespace QuickPickDBApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly QuickPickDbContext _dbcontext;
        public TransactionController(QuickPickDbContext quickPickDbContext)
        {
            _dbcontext = quickPickDbContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Sale>> GetAllTransactions()
        {
            return _dbcontext.Sales;
        }
        [HttpPost]
        public async Task<ActionResult<Sale>> CreateTransaction(Sale sale)
        {
            _dbcontext.Sales.Add(sale);
            await _dbcontext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllTransactions), new { id = sale.Transaction_Id }, sale);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateTransaction(Sale sale)
        {
            _dbcontext.Sales.Update(sale);
            await _dbcontext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            var sale = _dbcontext.Sales.Find(id);
            if (sale == null)
            {
                return NotFound();
            }
            _dbcontext.Sales.Remove(sale);
            await _dbcontext.SaveChangesAsync();
            return Ok();
        }
    }
}
