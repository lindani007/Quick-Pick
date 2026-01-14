using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using QuickPickDBApi.Models;
using QuickPickDBApi.Models.dbContext_folder;

namespace QuickPickDBApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AisleController : ControllerBase
    {
        private readonly QuickPickDbContext _dbContext;
        public AisleController(QuickPickDbContext quickPickDbContext)
        {
            _dbContext = quickPickDbContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Aisle>> GetAllAisle()
        {
            try
            {
                return _dbContext.Aisles;

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Aisle>> CreateAisle(Aisle aisle)
        {
            try
            {
                _dbContext.Aisles.Add(aisle);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAllAisle), new { id = aisle.Id }, aisle);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
            
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAisle(Aisle aisle)
        {
            try
            {
                _dbContext.Aisles.Update(aisle);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAisle(int id)
        {
            try
            {
                var aisle = _dbContext.Aisles.Find(id);
                if (aisle == null)
                {
                    return NotFound();
                }
                _dbContext.Aisles.Remove(aisle);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
