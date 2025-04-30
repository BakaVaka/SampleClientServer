using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace bProxyTunnelCore;
public static class Guard {

    public static void ThrowIfNull([NotNull] object? value, [CallerArgumentExpression(nameof(value))] string parameterName = "") {
        if (value == null)
            throw new ArgumentNullException(parameterName);
    }
}
