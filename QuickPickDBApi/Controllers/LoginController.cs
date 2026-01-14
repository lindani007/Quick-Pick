using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickPickDBApi.Models;
using QuickPickDBApi.Models.dbContext_folder;

namespace QuickPickDBApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly QuickPickDbContext _dbContext;
        public LoginController(QuickPickDbContext quickPickDbContext)
        {
            _dbContext = quickPickDbContext;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Login>> GetLoginAsync()
        {
           return _dbContext.Employees;
        }
    }
}
