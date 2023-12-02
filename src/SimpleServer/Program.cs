using Server;

using Shared;
using Shared.Messages;

var xmlChannel = new XmlChannel();

var application = new NetworkApplicationBuilder()
    .Use(async (connection, next, ct) => {
        try{
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

Console.CancelKeyPress += (_, _) => { cts.Cancel(); };
Console.WriteLine("Press [Ctrl+c] to stop server");
await server.Run(cts.Token);
Console.WriteLine("Server is stopped now");

async Task ConnectionHandler(Connection connection, CancellationToken cancellationToken = default) {

    while( !cancellationToken.IsCancellationRequested ) {
        var message = await connection.Receive(xmlChannel.Decode);
        if( message is MathOperationRequestMessage mathRequest ) {
            switch( mathRequest.Method ) {
                case "SUM": {
                        var response = new MathOperationResponseMessage(){
                            Status = "OK",
                            Result = mathRequest.Operands.Sum()
                        };
                        await connection.Send<Message>(response, xmlChannel.Encode);
                        break;
                    }
                default: {
                        var response = new NotSupportedOperationMessage();
                        await connection.Send<Message>(response, xmlChannel.Encode);
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
