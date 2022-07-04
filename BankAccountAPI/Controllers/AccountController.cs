using BankAccountAPI.Data;
using BankAccountAPI.Models;
using BankAccountAPI.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BankAccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<AccountController> _logger;
        public AccountController(ApplicationDbContext applicationDbContext, ILogger<AccountController> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        /// <summary>
        /// Finding the account by id
        /// </summary>
        /// <param name="id">Account identifier</param>
        /// <returns>Account details</returns>
        [HttpGet]
        [Route("GetAccountByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAccountByUserId(int id)
        {
            try
            {
                _logger.LogInformation($"action=getAccountByUserId id={id}");
                var account = _applicationDbContext.Accounts.Where(x => x.User.UserID == id).Include(x => x.User).FirstOrDefault();

                if (account != null)
                {
                    _logger.LogInformation($"action=getAccountByUserId msg='{account} was found'");
                    return Ok(account);
                }
                _logger.LogWarning($"action = getAccountByUserId msg='Account with {id} was not found'");
                return NotFound();
            }
            catch(Exception ex)
            {
                _logger.LogError($"action=getAccountByUserId msg='{ex.Message}'", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get a list with all accounts
        /// </summary>
        /// <returns>List of accounts</returns>
        [HttpGet]
        [Route("GetAllAccounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllAccounts()
        {
            try
            {
                _logger.LogInformation("action=getAllAccounts");
                var accounts = _applicationDbContext.Accounts.Include(x => x.User).ToList();
                if (accounts != null)
                {
                    _logger.LogInformation($"action=getAllAccounts accounts={accounts}");
                    return Ok(accounts);
                }
                _logger.LogWarning("action=getAllAccounts msg='Accounts was not found'");
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError($"action=getAllAccounts msg='{ex.Message}'", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Add account to an existing user
        /// </summary>
        /// <param name="accountRequest"></param>
        /// <returns>HTTP status code</returns>
        [HttpPut]
        [Route("AddAccountToUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddAccountToUser(AccountRequest accountRequest)
        {
            try
            {
                _logger.LogInformation($"action=addAccountToUser account='{JsonSerializer.Serialize(accountRequest)}'");
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
                    _logger.LogInformation($"action=addAccountToUser msg='A new user has been saved'");
                    return Ok();
                }
                _logger.LogWarning($"action=addAccountToUser msg='User not found'");
                return BadRequest("User not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"action=addAccountToUser msg='{ex.Message}'", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Delete account by identifier
        /// </summary>
        /// <param name="id">Account identifier</param>
        /// <returns>HTTP status code</returns>
        [HttpDelete]
        [Route("DeleteAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteAccount(int id)
        {
            try
            {
                _logger.LogInformation($"action=deleteAccount id={id}");
                var accountToRemove = _applicationDbContext.Accounts.Where(x => x.AccountId == id).FirstOrDefault();

                if (accountToRemove != null)
                {
                    _applicationDbContext.Accounts.Remove(accountToRemove);
                    _applicationDbContext.SaveChanges();
                    _logger.LogInformation($"action=deleteAccount msg='Account with {id} has been removed'");
                    return Ok();
                }
                _logger.LogWarning($"action=deleteAccount msg='Account with {id} was not found'");
                return NotFound($"Account with {id} was not found");
            }
            catch(Exception ex)
            {
                _logger.LogError($"action=deleteAccount msg='{ex.Message}'", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Edit account
        /// </summary>
        /// <param name="editAccountRequest"></param>
        /// <returns>HTTP status code</returns>
        [HttpPost]
        [Route("EditAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EditAccount(EditAccountRequest editAccountRequest)
        {
            try
            {
                _logger.LogInformation($"action=editAccount account='{JsonSerializer.Serialize(editAccountRequest)}'");
                var accountFromDb = _applicationDbContext.Accounts.Where(x => x.AccountId == editAccountRequest.AccountId).FirstOrDefault();
                if (accountFromDb != null)
                {
                    accountFromDb.BankName = editAccountRequest.BankName;
                    accountFromDb.AccountNumber = editAccountRequest.AccountNumber;
                    accountFromDb.IBAN = editAccountRequest.IBAN;
                    accountFromDb.Currency = editAccountRequest.Currency;
                    _applicationDbContext.SaveChanges();
                    _logger.LogInformation($"action=editAccount msg='Account updated correctly'");
                    return Ok();
                }
                _logger.LogWarning($"action=editAccount msg='Account was not found'");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"action=editAccount msg='{ex.Message}'", ex);
                return StatusCode(500);
            }
        }
    }
}