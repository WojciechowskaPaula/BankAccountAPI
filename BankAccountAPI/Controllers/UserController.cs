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
    }
}
