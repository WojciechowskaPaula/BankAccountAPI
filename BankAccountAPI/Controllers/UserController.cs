using BankAccountAPI.Data;
using BankAccountAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BankAccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<UserController> _logger;
        public UserController(ApplicationDbContext applicationDbContext, ILogger<UserController> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                _logger.LogInformation("action=getAllUsers");
                var usersList = _applicationDbContext.Users.ToList();
                _logger.LogInformation($"action=getAllUsers usersCount:{usersList.Count}");
                return Ok(usersList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"action=getAllUsers msg='{ex.Message}'", ex);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser(int id)
        {
            try
            {
                _logger.LogInformation($"action=getUser id={id}");
                var user = _applicationDbContext.Users.Where(x => x.UserID == id).FirstOrDefault();
                if (user == null)
                {
                    _logger.LogWarning($"action=getUser msg='User with {id} not found'");
                    return NotFound($"User with {id} not found");
                }
                _logger.LogInformation($"action=getUser msg='user with {id} was found'");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"action=getUser msg='{ex.Message}'", ex);
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("AddUser")]
        public IActionResult AddUser(User user)
        {
            try
            {
                _logger.LogInformation($"action=addUser user='{JsonSerializer.Serialize(user)}'");
                _applicationDbContext.Users.Add(user);
                _applicationDbContext.SaveChanges();
                _logger.LogInformation("action=addUser msg='A new user has been saved'");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"action=addUser msg='{ex.Message}'", ex);
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation($"action=deleteUser id={id}");
                var user = _applicationDbContext.Users.Where(x => x.UserID == id).FirstOrDefault();
                if (user == null)
                {
                    _logger.LogWarning($"action=getUser msg='User with {id} not found'");
                    return NotFound("User not found");
                }
                _applicationDbContext.Users.Remove(user);
                _applicationDbContext.SaveChanges();
                _logger.LogInformation($"action=deleteUser msg='User with {id} has been removed'");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"action=deleteUser msg='{ex.Message}'", ex);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("GetUsersByNationality")]
        public IActionResult GetUsersByNationality(string nationality)
        {
            try
            {
                _logger.LogInformation($"action=getUsersByNationality nationality={nationality}");
                var users = _applicationDbContext.Users.Where(x => x.Nationality == nationality).ToList();
                if (users.Count > 0)
                {
                    _logger.LogInformation($"action=getUsersByNationality usersCount={users.Count}");
                    return Ok(users);
                }
                _logger.LogWarning($"action=getUsersByNationality msg='Users with {nationality} not found'");
                return NotFound($"Users with {nationality} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"action=getUsersByNationality msg='{ex.Message}'", ex);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("GetUserByPhoneNumber")]
        public IActionResult GetUserByPhoneNumber(string phoneNumber)
        {
            try
            {
                _logger.LogInformation("action=getUserByPhoneNumber");
                var user = _applicationDbContext.Users.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefault();
                if (user != null)
                {
                    _logger.LogInformation($"action=getUserByPhoneNumber msg='User with phone number: {phoneNumber} was found'");
                    return Ok(user);
                }
                _logger.LogWarning($"action=getUserByPhoneNumber msg='User with {phoneNumber} not found'");
                return NotFound($"User with {phoneNumber} not found");
            }

            catch (Exception ex)
            {
                _logger.LogError($"action = getUserByPhoneNumber msg='{ex.Message}'", ex);
                return StatusCode(500);
            }
        }
    }
}
