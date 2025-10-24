using NameSorter.Console.SharedKernel;
using NameSorter.Console.SharedKernel.ValueObjects;

namespace NameSorter.Console.Features.SortName;

public sealed record SortNameCommandRequest(string FilePath);

public sealed class SortNameCommandHandler
{
    private const string ExpectedCommand = "name-sorter";
    private const string OutputFileName = "sorted-names-list.txt";

    private readonly INameSorter _sorter;
    private readonly INameParser _parser;
    private readonly IFileManager _fileManager;

    public SortNameCommandHandler(INameSorter sorter, IFileManager fileManager, INameParser parser)
    {
        _sorter = sorter;
        _fileManager = fileManager;
        _parser = parser;
    }

    public int Handle(SortNameCommandRequest request)
    {
        if (!_fileManager.Exists(request.FilePath))
        {
            System.Console.Error.WriteLine($"Input file not found: {request.FilePath}");
            return (int)ProgramExit.InvalidArguments;
        }

        var unsortedNames = _fileManager.ReadAllLines(request.FilePath).ToHashSet();

        if (unsortedNames.Count == 0)
        {
            return (int)ProgramExit.InvalidArguments;
        }

        var nameParsed = _parser.Parse(unsortedNames);
        var sortedNames = _sorter.Sort(nameParsed.ToList());

        _fileManager.Write(OutputFileName, sortedNames.Select(n => n.ToString()));

        DisplayToConsole(sortedNames);

        return (int)ProgramExit.Success;
    }

    private static void DisplayToConsole(IReadOnlyList<Fullname> sortedNames)
    {
        foreach (var name in sortedNames)
            System.Console.WriteLine(name);
    }
}