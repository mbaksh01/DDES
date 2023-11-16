using DDES.Server.Services.Abstractions;

namespace DDES.Server.Tests.Unit.Services;

public class ClientServiceTests
{
    private readonly IClientService _sut = new ClientService();

    [Fact]
    public void Get_Username_Should_Return_Correct_Username()
    {
        // Arrange
        Client client = new(Guid.NewGuid(), "testClient", 8613);
        _sut.AddClient(client);

        // Act
        string? username = _sut.GetUsername(client.Id);

        // Assert
        username.Should().NotBeNull();
        username.Should().Be("testClient");
    }

    [Fact]
    public void Append_Username_Should_Change_The_Username_Of_An_Existing_User()
    {
        // Arrange
        Client client = new(Guid.NewGuid(), "testClient", 8613);
        _sut.AddClient(client);

        // Act
        _sut.AppendUsername(client.Id, "newClientUsername");

        // Assert
        string? username = _sut.GetUsername(client.Id);

        username.Should().NotBeNull();
        username.Should().Be("newClientUsername");
    }

    [Fact]
    public void
        Append_Username_Should_Not_Change_The_Username_Of_An_Existing_User_If_The_Id_Is_Not_Found()
    {
        // Arrange
        Client client = new(Guid.NewGuid(), "testClient", 8613);
        _sut.AddClient(client);

        // Act
        _sut.AppendUsername(Guid.NewGuid(), "newClientUsername");

        // Assert
        string? username = _sut.GetUsername(client.Id);

        username.Should().NotBeNull();
        username.Should().Be("testClient");
    }
}