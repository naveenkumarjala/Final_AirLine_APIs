using Authenticate.ViewModels;

namespace Authenticate.Interfaces
{
    public interface IJWTManagerRepository
    {
        IEnumerable<Users> GetUsers();

       // Tokens Registration(Users users, bool value);

        Tokens Authenticate(Users users);
        string Authenticate(Users users, bool value);
    }
}
