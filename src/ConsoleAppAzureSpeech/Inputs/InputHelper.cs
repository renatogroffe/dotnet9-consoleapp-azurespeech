using Sharprompt;

namespace ConsoleAppAzureSpeech.Inputs;

public static class InputHelper
{
    public const string SIM = "Sim";
    public const string NAO = "Nao";

    public static bool ContinueWithTests()
    {
        var answer = Prompt.Select<string>(options =>
        {
            options.Message = "Continuar com novos testes?";
            options.Items = [SIM, NAO];
        });
        Console.WriteLine();
        return (answer == SIM);
    }
}