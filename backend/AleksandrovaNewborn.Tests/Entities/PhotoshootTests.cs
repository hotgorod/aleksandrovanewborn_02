using AleksandrovaNewborn.Entities;
using AleksandrovaNewborn.Tests.Api;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;

namespace AleksandrovaNewborn.Tests.Entities;

[TestFixture]
public class PhotoshootTests
{
    [TestCase((string)null)]
    [TestCase("")]
    public void Ctor_Requires_ChildName(string name)
    {
        // Arrange
        var client = new Client(FakeData.Random.Name.FirstName());
        
        // Act
        var action = () => new Photoshoot(client, name, DateTime.Now);
        action.Should().Throw<ArgumentNullException>();
    }
    
    public void Ctor_Requires_Client(string name)
    {
        // Act
        var action = () => new Photoshoot(null, FakeData.Random.Name.FirstName(), DateTime.Now);
        action.Should().Throw<ArgumentNullException>();
    }
    
}