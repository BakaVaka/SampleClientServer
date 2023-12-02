namespace Shared;

public delegate Task ConnectionHandlerDelegate(Connection connection, CancellationToken cancellationToken);
public delegate Task ConnectionHandlerPipleineDelegate(Connection connection, ConnectionHandlerDelegate next = null, CancellationToken cancellationToken = default);

// простой билдер приложения
public sealed class NetworkApplicationBuilder {

    private List<ConnectionHandlerPipleineDelegate> _handlers = new();

    public NetworkApplicationBuilder Use(ConnectionHandlerDelegate middleware) {
        _handlers.Add(async (connection, next, ct) => {
            await middleware(connection, ct);
            if(next != null ) {
                await next(connection, ct);
            }
        });
        return this;
    }

    public NetworkApplicationBuilder Use(ConnectionHandlerPipleineDelegate middleware) {
        _handlers.Add(middleware);
        return this;
    }

    public ConnectionHandlerDelegate Build() {
        // default handler, aka - end of application
        ConnectionHandlerDelegate handler = (connection, ct) => {
            return Task.CompletedTask;
        };

        handler = _handlers.Reverse<ConnectionHandlerPipleineDelegate>().Aggregate(handler, (acc, next) => {
            return async (Connection c, CancellationToken ct) => {
                await next(c, acc, ct);
            };
        });

        return handler;
    }
}
