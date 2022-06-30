﻿using BankAccountAPI.Data;
using BankAccountAPI.Models;
using BankAccountAPI.RequestModels;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        [Route("GetAccountByUserId")]
        public IActionResult GetAccountByUserId(int id)
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

        [HttpGet]
        [Route("GetAllAccounts")]
        public IActionResult GetAllAccounts()
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

        [HttpPut]
        [Route("AddAccountToUser")]
        public IActionResult AddAccountToUser(AccountRequest accountRequest)
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

        [HttpDelete]
        [Route("DeleteAccount")]
        public IActionResult DeleteAccount(int id)
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

        [HttpPost]
        [Route("EditAccount")]
        public IActionResult EditAccount(EditAccountRequest editAccountRequest)
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
            _logger.LogWarning($"Account was not found");
            return NotFound();
        }
    }
}
