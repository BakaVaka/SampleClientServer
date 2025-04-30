using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Server;

using Shared;

namespace bProxyTunnelHostServer;
public class NetServerHost : IHostedService {
    private readonly ILogger<NetServerHost> _logger;
    private readonly NetApplication _baseApplication;
    private readonly IConfiguration _configuration;
    private readonly ConnectionHandlerDelegate _application;
    private CancellationTokenSource _stopToken = new();

    private Task? _serverTask;

    public NetServerHost(ILogger<NetServerHost> logger, NetApplication application, IConfiguration configuration) {
        _logger = logger;
        _baseApplication = application;
        _configuration = configuration;
        _application = new NetworkApplicationBuilder()
            .Use(Handlers.ExceptionHandler)
            .Use(Handlers.LogConnectionAccepted(logger))
            .Use(application.RunBaseApplication)
            .Use(Handlers.LogConnectionComplete(logger))
            .Build();
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        var server = new TcpServer(new TcpServerSettings(_application, [16000]));
        _serverTask = server.Run(_stopToken.Token);
        return Task.CompletedTask;
    }
    public async Task StopAsync(CancellationToken cancellationToken) {
        _stopToken.Cancel();
        if(_serverTask is not null)
            await _serverTask;
    }

}
