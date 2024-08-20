namespace Paraminter.Semantic.Type.Apheleia.Errors.Commands;

internal sealed class HandleDifferentNumberOfArgumentsAndParametersCommand
    : IHandleDifferentNumberOfArgumentsAndParametersCommand
{
    public static IHandleDifferentNumberOfArgumentsAndParametersCommand Instance { get; } = new HandleDifferentNumberOfArgumentsAndParametersCommand();

    private HandleDifferentNumberOfArgumentsAndParametersCommand() { }
}
