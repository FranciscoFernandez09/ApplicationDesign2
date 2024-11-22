using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Interfaces;

public interface ISessionService
{
    User GetUserByToken(Guid token);
    bool IsValidSession(Guid token);
    SessionDto Login(string email, string password);
    void Logout(User? user);
}
