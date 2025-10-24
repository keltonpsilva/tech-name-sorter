namespace NameSorter.Console.SharedKernel;

public enum ProgramExit : int
{
    Success = 0,
    InvalidCommandLine = 1,
    NoArguments = 2,
    InvalidArguments = 3,
    FileNotFound = 4,
    ProcessingError = 5
}