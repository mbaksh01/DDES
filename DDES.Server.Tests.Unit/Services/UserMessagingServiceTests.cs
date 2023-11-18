using DDES.Common.Enums;
using DDES.Server.Services.Abstractions;
using Thread = DDES.Common.Models.Thread;

namespace DDES.Server.Tests.Unit.Services;

public class UserMessagingServiceTests
{
    private readonly IPublishingService _publishingService =
        Substitute.For<IPublishingService>();

    private readonly IUserMessagingService _sut;

    public UserMessagingServiceTests()
    {
        _sut = new UserMessagingService(_publishingService);
    }

    [Fact]
    private void Threads_Should_Be_Loaded_On_Construction()
    {
        // Act
        IEnumerable<Thread> threads = _sut.GetThreads("supplier");

        // Assert
        threads.Should().NotBeEmpty();
    }

    [Fact]
    private void Add_Thread_Message_Should_Save_Message_To_Thread()
    {
        // Arrange
        ThreadMessage message = new()
        {
            From = "supplier",
            To = "customer",
            MessageText = "This is a test message!",
            DateTimeSent = DateTime.Now,
        };

        // Act
        _sut.AddThreadMessage("supplier", "customer", message);

        // Assert
        IEnumerable<Thread> threads = _sut.GetThreads("supplier");

        threads.First(t => t.CustomerUsername == "customer").Messages
            .Should().Contain(message);

        _publishingService
            .Received(1)
            .PublishMessage(Topics.NewDirectMessage, message);
    }
}