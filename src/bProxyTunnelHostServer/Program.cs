using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace bProxyTunnelHostServer;

internal class Program {
    public static ILogger Logger { get; }
    public static IServiceProvider ServiceProvider { get; }
    public static IConfiguration Configuration { get; }
    private static DiagnosticObserver DiagnosticObserver;

    static Program() {

        Configuration = new ConfigurationManager()
            .AddJsonFile("config.json", false, true)
            .Build();

        var services = new ServiceCollection();

        services.AddLogging(config => {
            config.SetMinimumLevel(LogLevel.Trace);
            config.AddConsole();
        });

        services.AddSingleton(Server.Diagnostic.Source);
        services.AddSingleton(Configuration);
        services.AddSingleton<NetApplication>();
        services.AddSingleton<NetServerHost>();

        ServiceProvider = services.BuildServiceProvider();
        services.BuildServiceProvider();

        Logger = ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger<Program>();

        DiagnosticObserver = new();
    }

    static async Task Main() {

        var cts = new CancellationTokenSource();
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var token = cts.Token;
        token.Register(() => {
            tcs.SetResult();
        });

        ServiceProvider.GetRequiredService<DiagnosticListener>()
            .Subscribe(DiagnosticObserver);
        var host = ServiceProvider.GetRequiredService<NetServerHost>();
        
        await host.StartAsync(default);

        Logger.LogInformation("Application started. Press [Ctrl+c] to complete...");
        Console.CancelKeyPress += (s, e) => {
            cts.Cancel();
        };
        await tcs.Task;

        Logger.LogInformation("Stopping application...");
        await host.StopAsync(default);

        Logger.LogInformation("Application done");
    }
}
