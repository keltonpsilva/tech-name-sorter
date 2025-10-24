using NameSorter.Console.Features.SortName;
using NameSorter.Console.SharedKernel;
using NameSorter.Console.SharedKernel.ValueObjects;

namespace NameSorter.Console;

public class Program
{
    private const string NameSorterCommand = "name-sorter";
    private static readonly HashSet<string> AllowedCommands = [NameSorterCommand, "exit"];

    public static int Main(string[] args)
    {
        string? commandLine;
        string? filePath;

        while (true)
        {
            System.Console.Write("Enter command (e.g., name-sorter <input-file-path>) or 'exit' to quit: ");
            commandLine = System.Console.ReadLine();

            if (string.IsNullOrWhiteSpace(commandLine))
            {
                System.Console.Error.WriteLine($"Invalid command: {commandLine}.\nUsage: name-sorter <input-file-path>");
                continue;
            }

            var commandArgs = commandLine.Split([' '], StringSplitOptions.RemoveEmptyEntries);

            commandLine = commandArgs[0];

            if (string.Equals(commandLine, "exit", StringComparison.OrdinalIgnoreCase))
            {
                System.Console.WriteLine("Exiting.");
                return (int)ProgramExit.Success;
            }


            if (commandArgs.Length < 1 || !AllowedCommands.Contains(commandArgs[0]))
            {
                System.Console.Error.WriteLine("Invalid command. Usage: name-sorter <input-file-path>");
                continue;
            }

            filePath = commandArgs[1];

            break;
        }

        INameSorter sorter = new NameSorterService();
        INameParser parser = new NameParserService();
        IFileManager fileManager = new FileManager();

        switch (commandLine)
        {
            case NameSorterCommand:
                var command = new SortNameCommandRequest(filePath);
                var handler = new SortNameCommandHandler(sorter, fileManager, parser);
                return handler.Handle(command);
            default:
                System.Console.Error.WriteLine("Invalid command. Usage: name-sorter <input-file-path>");
                return (int)ProgramExit.InvalidCommandLine;
        }
    }
}