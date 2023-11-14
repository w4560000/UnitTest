using Xunit.Abstractions;

namespace xUnitTests.Helper
{
    public class Helpers
    {
        public static string GetDisplayName(IMethodInfo method, object[] data)

        {
            var a = data[0];
            var b = data[1];
            return $"{method.Name}({a}, {b})";
        }
    }
}