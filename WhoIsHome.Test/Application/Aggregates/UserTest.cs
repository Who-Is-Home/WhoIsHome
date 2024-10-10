using WhoIsHome.Aggregates;
using WhoIsHome.Shared.Exceptions;

namespace WhoIsHome.Test.Application.Aggregates;

[TestFixture]
public class UserTest
{
    [Test]
    public void Create_ReturnsNewUser_RepresentingAUserFromTheGivenData()
    {
        // Arrange
        const string userName = "Darki";
        const string email = "darki@whoishome.dev";
        const string password = "securePassword1234";
        
        // Act
        var result = User.Create(userName, email, password);
        
        // Assert
        result.Id.Should().BeNull();
        result.UserName.Should().Be(userName);
        result.Email.Should().Be(email);
        result.Password.Should().Be(password);
    }

    [Test]
    public void Create_ThrowsFormatException_WhenEmailIsInvalidFormat()
    {
        // Arrange
        const string userName = "Darki";
        const string invalidEmail = "missingAt.dev";
        const string password = "securePassword1234";
        
        // Act
        var act = () => User.Create(userName, invalidEmail, password);
        
        // Assert
        act.Should().Throw<FormatException>();
    }
    
    [Test]
    public void Create_ThrowsInvalidModelException_WhenUserNameIsTooLong()
    {
        // Arrange
        var userName = string.Join("", Enumerable.Repeat('a', 32));
        const string invalidEmail = "darki@whoishome.dev";
        const string password = "securePassword1234";
        
        // Act
        var act = () => User.Create(userName, invalidEmail, password);
        
        // Assert
        act.Should().Throw<InvalidModelException>();
    }
}