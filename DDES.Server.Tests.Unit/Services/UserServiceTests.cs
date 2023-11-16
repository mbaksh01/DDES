using DDES.Server.Services.Abstractions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;

namespace DDES.Server.Tests.Unit.Services;

public class UserServiceTests
{
    private readonly IClientService _clientService =
        Substitute.For<IClientService>();

    private readonly ILogger<UserService> _logger =
        Substitute.For<ILogger<UserService>>();

    private readonly IUserService _sut;

    public UserServiceTests()
    {
        _sut = new UserService(_clientService, _logger);
    }

    [Fact]
    public void
        Authenticate_Should_Return_User_If_Username_And_Password_Are_Correct()
    {
        // Arrange
        string username = "supplier";
        string password = "Password123!";

        // Act
        User? user = _sut.Authenticate(username, password);

        // Assert
        user.Should().NotBeNull();

        using (new AssertionScope())
        {
            user!.Username.Should().Be(username);
            user.Password.Should().Be(password);
        }
    }

    [Theory]
    [InlineData("supplier", "password123!")]
    [InlineData("supplier2", "Password123!")]
    public void
        Authenticate_Should_Return_Null_If_Username_Or_Password_Are_Incorrect(
            string username,
            string password)
    {
        // Act
        User? user = _sut.Authenticate(username, password);

        // Assert
        user.Should().BeNull();
    }

    [Fact]
    public void Add_Subscription_Should_Add_To_A_Users_List_Of_Subscriptions()
    {
        // Arrange
        Guid clientId = Guid.NewGuid();
        const string subscriptionName = "TestSubscription";

        _clientService.GetUsername(clientId).Returns("supplier");

        // Act
        _sut.AddSubscription(clientId, subscriptionName);

        // Assert
        User user = _sut.Authenticate("supplier", "Password123!")!;

        user.Subscriptions.Should().Contain(subscriptionName);
    }

    [Fact]
    public void
        Add_Subscription_Should_Not_Add_To_A_Users_List_Of_Subscriptions_If_The_Client_Id_Is_Not_Found()
    {
        // Arrange
        Guid clientId = Guid.NewGuid();
        const string subscriptionName = "TestSubscription";

        // Act
        _sut.AddSubscription(clientId, subscriptionName);

        // Assert
        User user = _sut.Authenticate("supplier", "Password123!")!;

        user.Subscriptions.Should().NotContain(subscriptionName);
    }
}