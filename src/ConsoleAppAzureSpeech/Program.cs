using ConsoleAppAzureSpeech.Inputs;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;
using System.Globalization;

Console.WriteLine("***** Testes com Azure Speech *****");
Console.WriteLine();

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

do
{
    await RecognizeSpeechAsync();
    Console.WriteLine();
} while (InputHelper.ContinueWithTests());

async Task RecognizeSpeechAsync()
{
    var speechKey = configuration["AzureSpeech:ApiKey"];
    var serviceRegion = configuration["AzureSpeech:ServiceRegion"];

    var config = SpeechConfig.FromSubscription(speechKey, serviceRegion);
    config.EnableAudioLogging();
    config.SpeechRecognitionLanguage = "pt-BR";

    using var recognizer = new SpeechRecognizer(config);
    Console.WriteLine("Fale algo em português do Brasil...");
    Console.WriteLine("Pressione ENTER para iniciar a captura de fala...");
    Console.ReadLine();

    var result = await recognizer.RecognizeOnceAsync();

    switch (result.Reason)
    {
        case ResultReason.RecognizedSpeech:
            Console.WriteLine($"Reconhecido: {result.Text}");
            break;

        case ResultReason.NoMatch:
            Console.WriteLine("Não foi possível reconhecer fala.");
            break;

        case ResultReason.Canceled:
            var cancellation = CancellationDetails.FromResult(result);
            Console.WriteLine($"Reconhecimento cancelado. Motivo: {cancellation.Reason}");
            if (cancellation.Reason == CancellationReason.Error)
            {
                Console.WriteLine($"Detalhes do erro: {cancellation.ErrorDetails}");
            }
            break;

        default:
            break;
    }
}