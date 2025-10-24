using Bogus;
using FluentAssertions;
using Moq;
using NameSorter.Console.Features.SortName;
using NameSorter.Console.SharedKernel;
using NameSorter.Console.SharedKernel.ValueObjects;

namespace NameSorter.Console.Unit.Tests.Features.SortName;

public class SortNameCommandHandlerTests
{
    private readonly Mock<INameSorter> _sorterMock = new();
    private readonly Mock<INameParser> _parserMock = new();
    private readonly Mock<IFileManager> _fileManagerMock = new();
    private readonly Faker _faker = new();

    private SortNameCommandHandler CreateHandler() =>
        new(_sorterMock.Object, _fileManagerMock.Object, _parserMock.Object);

    [Fact]
    public void Handle_WhenFileDoesNotExist_ShouldReturnInvalidArguments()
    {
        // Arrange
        var request = new SortNameCommandRequest("missing.txt");
        _fileManagerMock.Setup(f => f.Exists(request.FilePath)).Returns(false);
        var handler = CreateHandler();

        // Act
        var result = handler.Handle(request);

        // Assert
        result.Should().Be((int)ProgramExit.InvalidArguments);
        _fileManagerMock.Verify(f => f.ReadAllLines(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void Handle_WhenFileExistsButIsEmpty_ShouldReturnInvalidArguments()
    {
        // Arrange
        var request = new SortNameCommandRequest("empty.txt");
        _fileManagerMock.Setup(f => f.Exists(request.FilePath)).Returns(true);
        _fileManagerMock.Setup(f => f.ReadAllLines(request.FilePath)).Returns(Array.Empty<string>());
        var handler = CreateHandler();

        // Act
        var result = handler.Handle(request);

        // Assert
        result.Should().Be((int)ProgramExit.InvalidArguments);
        _parserMock.Verify(p => p.Parse(It.IsAny<HashSet<string>>()), Times.Never);
    }

    [Fact]
    public void Handle_WhenValidFileProvided_ShouldSortAndWriteFile()
    {
        // Arrange
        var filePath = "names.txt";
        var unsortedNames = new[] { "Charlie Brown", "Alice Johnson" };
        var parsedNames = unsortedNames.Select(Fullname.Create).ToList();
        var sortedNames = parsedNames.OrderBy(n => n.LastName).ToList();

        _fileManagerMock.Setup(f => f.Exists(filePath)).Returns(true);
        _fileManagerMock.Setup(f => f.ReadAllLines(filePath)).Returns(unsortedNames);
        _parserMock.Setup(p => p.Parse(It.IsAny<HashSet<string>>())).Returns(parsedNames);
        _sorterMock.Setup(s => s.Sort(It.IsAny<List<Fullname>>())).Returns(sortedNames);

        var handler = CreateHandler();

        // Act
        var result = handler.Handle(new SortNameCommandRequest(filePath));

        // Assert
        result.Should().Be((int)ProgramExit.Success);
        _parserMock.Verify(p => p.Parse(It.Is<HashSet<string>>(h => h.SetEquals(unsortedNames))), Times.Once);
        _sorterMock.Verify(s => s.Sort(It.IsAny<List<Fullname>>()), Times.Once);
        _fileManagerMock.Verify(f => f.Write("sorted-names-list.txt", It.IsAny<IEnumerable<string>>()), Times.Once);
    }

    [Fact]
    public void Handle_WhenParserThrows_ShouldPropagateException()
    {
        // Arrange
        var request = new SortNameCommandRequest("names.txt");
        _fileManagerMock.Setup(f => f.Exists(request.FilePath)).Returns(true);
        _fileManagerMock.Setup(f => f.ReadAllLines(request.FilePath)).Returns(new[] { "Bad Name" });
        _parserMock.Setup(p => p.Parse(It.IsAny<HashSet<string>>()))
                   .Throws(new ArgumentException("Invalid name format"));

        var handler = CreateHandler();

        // Act
        Action act = () => handler.Handle(request);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("*Invalid name format*");
    }

    [Fact]
    public void Handle_WhenSorterThrows_ShouldPropagateException()
    {
        // Arrange
        var request = new SortNameCommandRequest("names.txt");
        var parsed = new List<Fullname> { Fullname.Create("John Doe") };

        _fileManagerMock.Setup(f => f.Exists(request.FilePath)).Returns(true);
        _fileManagerMock.Setup(f => f.ReadAllLines(request.FilePath)).Returns(["John Doe"]);
        _parserMock.Setup(p => p.Parse(It.IsAny<HashSet<string>>())).Returns(parsed);
        _sorterMock.Setup(s => s.Sort(It.IsAny<List<Fullname>>()))
                   .Throws(new InvalidOperationException("Sorting failed"));

        var handler = CreateHandler();

        // Act
        Action act = () => handler.Handle(request);

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("*Sorting failed*");
    }
}