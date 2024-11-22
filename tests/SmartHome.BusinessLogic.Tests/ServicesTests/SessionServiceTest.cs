using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.Models.ResponseDTOs;
using SmartHome.BusinessLogic.Services;

namespace SmartHome.BusinessLogic.Tests.ServicesTests;

[TestClass]
public class SessionServiceTests
{
    private const string Email = "test@example.com";
    private const string Password = "Password-1";
    private readonly Mock<IRepository<Session>> _sessionRepositoryMock;
    private readonly SessionService _sessionService;
    private readonly User _user;
    private readonly Mock<IRepository<User>> _userRepositoryMock;

    public SessionServiceTests()
    {
        _userRepositoryMock = new Mock<IRepository<User>>();
        _sessionRepositoryMock = new Mock<IRepository<Session>>();
        _sessionService = new SessionService(_userRepositoryMock.Object, _sessionRepositoryMock.Object);

        _user = new User
        {
            Id = Guid.NewGuid(),
            Email = Email,
            Password = Password,
            Role = new Role { Id = Guid.NewGuid() }
        };
    }

    #region Login

    #region Error

    [TestMethod]
    public void Login_WhenEmailOrPasswordIsIncorrect_ShouldThrowInvalidOperationException()
    {
        const string email = "test@example.com";
        const string password = "WrongPassword-1";
        _userRepositoryMock.Setup(us => us.Get(u => u.Email == email && u.Password == password))
            .Returns((User?)null);

        Action act = () => _sessionService.Login(email, password);

        act.Should().Throw<InvalidOperationException>().WithMessage("Email or password is incorrect");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Login_WhenParametersAreValidAndNotHasPreviousSession_ShouldReturnSessionId()
    {
        User user = _user;
        _userRepositoryMock.Setup(us => us.Get(u => u.Email == Email && u.Password == Password)).Returns(user);
        _sessionRepositoryMock.Setup(repo => repo.Get(s => s.UserId == user.Id)).Returns((Session?)null);
        _sessionRepositoryMock.Setup(repo => repo.Add(It.IsAny<Session>())).Verifiable();

        SessionDto result = _sessionService.Login(Email, Password);

        result.Token.Should().NotBeEmpty();
        result.Role.Should().Be(user.Role.Id);
        result.Name.Should().Be(user.Name);
        _sessionRepositoryMock.Verify(s => s.Add(It.IsAny<Session>()), Times.Once);
    }

    [TestMethod]
    public void Login_WhenParametersAreValidAndHasPreviousSession_ShouldReturnSessionId()
    {
        User user = _user;
        var session = new Session { SessionId = Guid.NewGuid(), UserId = user.Id };

        _userRepositoryMock.Setup(us => us.Get(u => u.Email == Email && u.Password == Password)).Returns(user);
        _sessionRepositoryMock.Setup(repo => repo.Get(s => s.UserId == user.Id)).Returns(session);
        _sessionRepositoryMock.Setup(repo => repo.Delete(session)).Verifiable();
        _sessionRepositoryMock.Setup(repo => repo.Add(It.IsAny<Session>())).Verifiable();

        SessionDto result = _sessionService.Login(Email, Password);

        result.Token.Should().NotBeEmpty();
        result.Role.Should().Be(_user.Role.Id);
        result.Name.Should().Be(_user.Name);
        _sessionRepositoryMock.Verify(repo => repo.Delete(session), Times.Once);
        _sessionRepositoryMock.Verify(repo => repo.Add(It.Is<Session>(s => s.UserId == user.Id && s.User == user)),
            Times.Once);
    }

    #endregion

    #endregion

    #region Logout

    #region Error

    [TestMethod]
    public void Logout_WhenUserIsNull_ShouldThrowInvalidOperationException()
    {
        Action act = () => _sessionService.Logout(null);

        act.Should().Throw<InvalidOperationException>().WithMessage("User not logged in");
    }

    [TestMethod]
    public void Logout_WhenSessionIsNotFound_ShouldThrowInvalidOperationException()
    {
        var user = new User { Id = Guid.NewGuid() };
        _sessionRepositoryMock.Setup(repo => repo.Get(s => s.UserId == user.Id)).Returns((Session?)null);

        Action act = () => _sessionService.Logout(user);

        act.Should().Throw<InvalidOperationException>().WithMessage("User not logged in");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Logout_WhenUserAreLogged_ShouldDeleteSession()
    {
        var user = new User { Id = Guid.NewGuid() };
        var session = new Session { UserId = user.Id };

        _sessionRepositoryMock.Setup(repo => repo.Get(s => s.UserId == user.Id)).Returns(session);
        _sessionRepositoryMock.Setup(repo => repo.Delete(session)).Verifiable();

        _sessionService.Logout(user);

        _sessionRepositoryMock.Verify(repo => repo.Delete(session), Times.Once);
    }

    #endregion

    #endregion

    #region GetUserByToken

    #region Error

    [TestMethod]
    public void GetUserByToken_WhenSessionIsNotFound_ShouldThrowException()
    {
        var token = Guid.NewGuid();
        _sessionRepositoryMock.Setup(repo => repo.Get(s => s.SessionId == token)).Returns((Session?)null);

        Action act = () => _sessionService.GetUserByToken(token);

        act.Should().Throw<Exception>().WithMessage("Session not found");
    }

    [TestMethod]
    public void GetUserByToken_WhenUserIsNotFound_ShouldThrowException()
    {
        var token = Guid.NewGuid();
        var session = new Session { SessionId = token, UserId = Guid.NewGuid() };

        _sessionRepositoryMock.Setup(repo => repo.Get(s => s.SessionId == token)).Returns(session);
        _userRepositoryMock.Setup(repo => repo.Get(u => u.Id == session.UserId)).Returns((User?)null);

        Action act = () => _sessionService.GetUserByToken(token);

        act.Should().Throw<Exception>().WithMessage("User not found");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetUserByToken_WhenSessionAndUserAreFound_ShouldReturnUser()
    {
        var token = Guid.NewGuid();
        var user = new User { Id = Guid.NewGuid() };
        var session = new Session { SessionId = token, UserId = user.Id };

        _sessionRepositoryMock.Setup(repo => repo.Get(s => s.SessionId == token)).Returns(session);
        _userRepositoryMock.Setup(repo => repo.Get(u => u.Id == session.UserId)).Returns(user);

        User result = _sessionService.GetUserByToken(token);

        result.Should().Be(user);
    }

    #endregion

    #endregion

    #region IsValidSession

    [TestMethod]
    public void IsValidSession_WhenSessionExists_ShouldReturnTrue()
    {
        var token = Guid.NewGuid();

        _sessionRepositoryMock.Setup(repo => repo.Exists(s => s.SessionId == token)).Returns(true);

        var result = _sessionService.IsValidSession(token);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsValidSession_WhenSessionDoesNotExist_ShouldReturnFalse()
    {
        var token = Guid.NewGuid();

        _sessionRepositoryMock.Setup(repo => repo.Exists(s => s.SessionId == token)).Returns(false);

        var result = _sessionService.IsValidSession(token);

        result.Should().BeFalse();
    }

    #endregion
}
