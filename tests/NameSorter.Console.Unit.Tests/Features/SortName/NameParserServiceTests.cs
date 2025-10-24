using Bogus;
using FluentAssertions;
using NameSorter.Console.Features.SortName;

namespace NameSorter.Console.Unit.Tests.Features.SortName;

public class NameParserServiceTests
{
    private readonly Faker _faker = new();
    private readonly NameParserService _sut = new();

    [Fact]
    public void Parse_WhenValidNamesProvided_ShouldReturnListOfFullnames()
    {
        // Arrange
        var validNames = new HashSet<string>
            {
                _faker.Name.FirstName() + " " + _faker.Name.LastName(),
                _faker.Name.FirstName() + " " + _faker.Name.LastName()
            };

        // Act
        var result = _sut.Parse(validNames);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(validNames.Count);

        var first = result.First();
        first.GivenName.Should().NotBeNullOrWhiteSpace();
        first.LastName.Should().NotBeNullOrWhiteSpace();

        result.Select(x => x.ToString()).Should().BeEquivalentTo(validNames);
    }

    [Fact]
    public void Parse_WhenNameHasTooFewParts_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidNames = new HashSet<string> { "Prince" };

        // Act
        Action act = () => _sut.Parse(invalidNames);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("*at least one given name and a last name*");
    }

    [Fact]
    public void Parse_WhenNameHasTooManyGivenNames_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidName = "John Paul George Ringo Lennon";
        var names = new HashSet<string> { invalidName };

        // Act
        Action act = () => _sut.Parse(names);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("*at least 1 and at most 3 given names*");
    }

    [Fact]
    public void Parse_WhenNameIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var names = new HashSet<string?> { null! };

        // Act
        Action act = () => _sut.Parse(names!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

}