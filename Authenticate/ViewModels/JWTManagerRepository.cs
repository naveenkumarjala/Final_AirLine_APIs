using Authenticate.DBContext;
using Authenticate.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenticate.ViewModels
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
        //Dictionary<string, string> UserRecords = new Dictionary<string, string>
        //{
        //    { "user1","password1"},
        //     { "user2","password2"}

        //};
        public static Users users = new Users();
        private readonly IConfiguration configuartion;

        private readonly UsersApplicationDbContext _usersDbContext;

        public IEnumerable<Users> GetUsers()
        {
            return _usersDbContext.tblUserRegistor.ToList();
        }
        public JWTManagerRepository(IConfiguration iconfiguration, UsersApplicationDbContext usersDbContext)
        {
            configuartion = iconfiguration;
            _usersDbContext = usersDbContext;
        }

       
        public Tokens Authenticate(Users users)//User login Method
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(configuartion["JWT:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,users.Email),
                     new Claim(ClaimTypes.Role,users.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Tokens { Token = tokenHandler.WriteToken(token),
                Role = users.Role, Email = users.Email, UserID = users.UserID };
        }
        public string  Authenticate(Users users, bool value)//user Registration Method
        {
            if (value)
            {
                Users userecordemail = _usersDbContext.tblUserRegistor.ToList().Where(o=>o.Email==users.Email ).FirstOrDefault();
                if (userecordemail != null)
                {
                    return null;
                }
                _usersDbContext.tblUserRegistor.Add(users);
                _usersDbContext.SaveChanges();
            }

            return "Registered Successfully";

        }
       

    }
}
