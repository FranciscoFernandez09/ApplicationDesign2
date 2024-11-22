using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.ResponseDTOs;

namespace SmartHome.BusinessLogic.Services;

public sealed class SessionService(
    IRepository<User> userRepository,
    IRepository<Session> sessionRepository)
    : ISessionService
{
    public SessionDto Login(string email, string password)
    {
        User? user = userRepository.Get(u => u.Email == email && u.Password == password);
        if (user == null)
        {
            throw new InvalidOperationException("Email or password is incorrect");
        }

        var session = new Session(user);
        Session? sessionToDelete = sessionRepository.Get(s => s.UserId == user.Id);
        if (sessionToDelete != null)
        {
            sessionRepository.Delete(sessionToDelete);
        }

        sessionRepository.Add(session);

        Guid role = user.Role.Id;
        var result = new SessionDto(session.SessionId, role, user.Name);

        return result;
    }

    public void Logout(User? user)
    {
        if (user == null)
        {
            throw new InvalidOperationException("User not logged in");
        }

        Session? session = sessionRepository.Get(s => s.UserId == user.Id);
        if (session == null)
        {
            throw new InvalidOperationException("User not logged in");
        }

        sessionRepository.Delete(session);
    }

    public User GetUserByToken(Guid token)
    {
        Session? session = sessionRepository.Get(s => s.SessionId == token);

        if (session == null)
        {
            throw new Exception("Session not found");
        }

        User? user = userRepository.Get(u => u.Id == session.UserId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return user;
    }

    public bool IsValidSession(Guid token)
    {
        return sessionRepository.Exists(s => s.SessionId == token);
    }
}
