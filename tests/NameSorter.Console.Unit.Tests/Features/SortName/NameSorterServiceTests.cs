using Bogus;
using FluentAssertions;
using NameSorter.Console.Features.SortName;
using NameSorter.Console.SharedKernel.ValueObjects;

namespace NameSorter.Console.Unit.Tests.Features.SortName;

public class NameSorterServiceTests
{
    private readonly Faker _faker = new();
    private readonly NameSorterService _sut = new();

    [Fact]
    public void Sort_WhenValidListProvided_ShouldSortByLastThenGivenName()
    {
        // Arrange
        var names = new List<Fullname>
        {
            Fullname.Create("Alice Johnson"),
            Fullname.Create("Bob Adams"),
            Fullname.Create("Charlie Adams"),
            Fullname.Create("David Brown")
        };

        // Act
        var result = _sut.Sort(names);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(names.Count);

        result.Select(n => n.ToString()).Should().ContainInOrder(
            "Bob Adams",
            "Charlie Adams",
            "David Brown",
            "Alice Johnson"
        );
    }

    [Fact]
    public void Sort_WhenNamesHaveDifferentCasing_ShouldSortCaseInsensitive()
    {
        // Arrange
        var names = new List<Fullname>
        {
            Fullname.Create("Bob smith"),
            Fullname.Create("Alice Smith")
        };

        // Act
        var result = _sut.Sort(names);

        // Assert
        result.First().ToString().Should().Be("Alice Smith");
    }

    [Fact]
    public void Sort_WhenEmptyListProvided_ShouldReturnEmptyList()
    {
        // Arrange
        var names = new List<Fullname>();

        // Act
        var result = _sut.Sort(names);

        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }
}