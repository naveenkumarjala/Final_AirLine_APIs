using Authenticate.Interfaces;
using Authenticate.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Authenticate.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public static Users users = new Users();
        private object _configuration;
        private readonly IJWTManagerRepository _iJWTManager;

        public UsersController(IJWTManagerRepository jWTManager)
        {
            _iJWTManager = jWTManager;
        }

        [HttpGet]
        [Route("get-all-users")/*, Authorize(Roles = "1")*/]
        public IActionResult GetUsers()
        {
            try
            {
                IEnumerable < Users > listusers = _iJWTManager.GetUsers().ToList();
                var selectuser = (from p in listusers
                               select new

                               {
                                   p.UserID,
                                   p.Email,
                                   p.FullName,
                                   p.Mobile,
                                  // p.PasswordHash
                               }).ToList();

                return new OkObjectResult(selectuser);
            }
            catch
            {
                return BadRequest();
            }
        }
      
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<string>> login(Login request)
        {
            Users userecord = _iJWTManager.GetUsers().ToList().Where((x => x.Email == request.Email)).FirstOrDefault();
            if (userecord == null)
            {
                  return Unauthorized();
            }
            if (!verifyPasswordHash(request.Password, userecord.PasswordHash, userecord.PasswordSalt))
            {
                return BadRequest("Wrong Password");
            }

            var token = _iJWTManager.Authenticate(userecord);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
            
        }

        [HttpPost]
        [Route("registor")]
        public async Task<ActionResult<Users>> Register(Register request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            users.UserID=request.UserID;
            users.FullName=request.FullName;
            users.Role=request.Role;
            users.Mobile=request.Mobile;
            users.Email = request.Email;
            users.PasswordHash = passwordHash;
            users.PasswordSalt = passwordSalt;

            var token = _iJWTManager.Authenticate(users, true);
            if (token == null)
            {
                return Ok("user already existed");
            }
            return Ok( new  { result = token });
        }

        private void CreatePasswordHash(string Password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            }
        }
        private bool verifyPasswordHash(string Password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var hmac = new HMACSHA512(PasswordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
                return computeHash.SequenceEqual(PasswordHash);
            }
        }

    }
}
