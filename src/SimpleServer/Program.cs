using System.Diagnostics;

using Server;

using Shared;
using Shared.Messages;

using SimpleServer;

var application = new NetworkApplicationBuilder()
    .Use(async (connection, next, ct) => {
        if(next is null) {
            // todo add diagnostic message
            return;
        }
        try{
            Console.WriteLine("Connected");
            await next(connection, ct);
        }
        catch(Exception ex){
            Console.Error.WriteLine(ex.ToString());
        }
    })
    .Use((connection, ct) => {
        Console.WriteLine($"New connection accepted : {connection.RemoteEndPoint}");
        return Task.CompletedTask;
    })
    .Use(ConnectionHandler)
    .Build();

var settings = new ServerBase.Settings(application, 8888);
var server = new ServerBase(settings);

using var cts = new CancellationTokenSource();

var observer = new DiagnosticObserver();
IDisposable subscription = DiagnosticListener.AllListeners.Subscribe(observer);

Console.CancelKeyPress += (_, _) => { cts.Cancel(); };

await server.Run(cts.Token);

async Task ConnectionHandler(Connection connection, CancellationToken cancellationToken = default) {

    while( !cancellationToken.IsCancellationRequested ) {
        var message = await connection.Receive(XmlChannel.Decode);
        if( message is MathOperationRequestMessage mathRequest ) {
            switch( mathRequest.Method ) {
                case "SUM": {
                        var response = new MathOperationResponseMessage(){
                            Status = "OK",
                            Result = mathRequest.Operands.Sum()
                        };
                        await connection.Send<Message>(response, XmlChannel.Encode);
                        break;
                    }
                default: {
                        var response = new NotSupportedOperationMessage();
                        await connection.Send<Message>(response, XmlChannel.Encode);
                        break;
                    }
            }
        }
        else {
            connection.Close();
            break;
        }
    }
}
