// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using AuthAPI.Data;
// using Swashbuckle.AspNetCore.Annotations; // Import namespace ini


// namespace AuthAPI.Controllers
// {
//     [HttpGet]
// [Route("api/[controller]")]
// [SwaggerTag("Endpoints untuk mengelola data pengguna")]
// public class UsersController : ControllerBase
// {
//     private readonly MyDbContext _context;

//     public UsersController(MyDbContext context)
//     {
//         _context = context;
//     }

//     [HttpGet]
//     [Route("api/users")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [Produces("application/json")]
//     public ActionResult<IEnumerable<User>> GetUsers()
//     {
//         var users = _context.Users.ToList();
//         return Ok(users);
//     }
// }

// }

using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Mvc;
using AuthAPI.models;
using AuthAPI.Data;
using Swashbuckle.AspNetCore.Annotations;
using AuthAPI.models.response;
using AuthAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthAPI.Controllers
{
    [ApiController]
    // [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [SwaggerTag("Endpoints untuk mengelola data pengguna")]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(MyDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet] // This attribute should be applied to methods
        // [SwaggerOperation("Mendapatkan daftar pengguna", Tags = new[] { "Pengguna" })]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Fetching users...");
                var users = _context.Users.ToList();
                return Ok(new GenericResponse(200, "SUCCESS", users));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users.");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        public IActionResult GetById([FromRoute] int id)
        {
            _logger.LogInformation("Fetching user...");
            var user = _context.Users.Find(id);

            if (user == null)
            {
                _logger.LogInformation("user not found.");
                return NotFound(new GenericResponse(404, "Data User tidak ditemukan"));
            }

            return Ok(new GenericResponse(200, "SUCCESS", user));
        }
    }
}
