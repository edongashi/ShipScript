using System.Text.RegularExpressions;

namespace ShipScript.RShipCore.Helpers
{
    public static class StringHelpers
    {
        private static readonly Regex LineRegex = new Regex(@" ->\s*(.*)\s*\z");

        public static string CleanupStackTrace(string stackTrace)
        {
            var lines = stackTrace.Split('\n');
            lines[1] = LineRegex.Replace(lines[1], match =>
            {
                var str = match.Groups[1].ToString();
                return " -> " + str;
            });

            for (var i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line.StartsWith("    at"))
                {
                    lines[i] = line.Substring(2);
                }
            }

            return string.Join("\n", lines);
        }
    }
}
