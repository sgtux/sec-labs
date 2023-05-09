using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreWebGoat.Models;
using NetCoreWebGoat.Repositories;

namespace NetCoreWebGoat.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private UserRepository _userRepository;

        public UsersController(UserRepository userRepository, ILogger<UsersController> logger) : base(logger) => _userRepository = userRepository;

        [HttpGet]
        public IActionResult Flag()
        {
            if (IsAdmin)
            {
                var users = _userRepository.GetAll();
                return Ok(users.Select(p => new UserModelResult(p)));
            }

            return Forbid();
        }
    }
}