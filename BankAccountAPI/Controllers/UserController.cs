using BankAccountAPI.Data;
using BankAccountAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankAccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public UserController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var usersList = _applicationDbContext.Users.ToList();
            return Ok(usersList);
        }

        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser(int id)
        {
            var user = _applicationDbContext.Users.Where(x => x.UserID == id).FirstOrDefault();
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [HttpPut]
        [Route("AddUser")]
        public IActionResult AddUser(User user)
        {
            _applicationDbContext.Users.Add(user);
            _applicationDbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            var user = _applicationDbContext.Users.Where(x => x.UserID == id).FirstOrDefault();

            if (user == null)
            {
                return NotFound("User not found");
            }
            _applicationDbContext.Users.Remove(user);
            _applicationDbContext.SaveChanges();
            return Ok(user);
        }

        [HttpGet]
        [Route("GetUsersByNationality")]
        public IActionResult GetUsersByNationality(string nationality)
        {
            var users = _applicationDbContext.Users.Where(x => x.Nationality == nationality).ToList();
            if (users.Count > 0)
            {
                return Ok(users);
            }
            else
                return NotFound($"Users with {nationality} not found");
        }

        [HttpGet]
        [Route("GetUserByPhoneNumber")]
        public IActionResult GetUserByPhoneNumber(string phoneNumber)
        {
            var user = _applicationDbContext.Users.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefault();
            if (user != null)
            {
                return Ok(user);
            }
            else
                return NotFound($"Users with {phoneNumber} not found");
        }
    }
}
