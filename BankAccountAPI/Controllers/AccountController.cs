using BankAccountAPI.Data;
using BankAccountAPI.Models;
using BankAccountAPI.RequestModels;
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

        [HttpGet]
        [Route("GetAllAccounts")]
        public IActionResult GetAllAccounts()
        {
            var accounts = _applicationDbContext.Accounts.Include(x => x.User).ToList();
            if (accounts != null)
            {
                return Ok(accounts);
            }
            else
                return NoContent();
        }

        [HttpPut]
        [Route("AddAccountToUser")]
        public IActionResult AddAccountToUser(AccountRequest accountRequest)
        {
            var user = _applicationDbContext.Users.Where(x => x.UserID == accountRequest.UserID).FirstOrDefault();
            if (user != null)
            {
                Account account = new Account();
                account.AccountNumber = accountRequest.AccountNumber;
                account.BankName = accountRequest.BankName;
                account.IBAN = accountRequest.IBAN;
                account.Currency = accountRequest.Currency;
                account.User = user;
                _applicationDbContext.Accounts.Add(account);
                _applicationDbContext.SaveChanges();
                return Ok();
            }
            else
                return NotFound();
        }

        [HttpDelete]
        [Route("DeleteAccount")]
        public IActionResult DeleteAccount(int id)
        {
            var accountToRemove = _applicationDbContext.Accounts.Where(x => x.AccountId == id).FirstOrDefault();

            if (accountToRemove != null)
            {
                _applicationDbContext.Accounts.Remove(accountToRemove);
                _applicationDbContext.SaveChanges();
                return Ok();
            }
            else
                return NotFound($"Account with {id} was not found");
        }


    }
}
