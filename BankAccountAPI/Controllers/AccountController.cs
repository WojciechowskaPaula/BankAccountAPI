using BankAccountAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AccountController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        [Route("GetAccountByUserId")]
        public IActionResult GetAccountByUserId(int id)
        {
            var account = _applicationDbContext.Accounts.Where(x => x.User.UserID == id).Include(x => x.User).FirstOrDefault();
           
            if (account != null)
            {
                return Ok(account);
            }
            else
                return NotFound();
        }
    }
}
