namespace ShipScript.RShipCore.Helpers
{
    public static class StringHelpers
    {
        public static string CleanupStackTrace(string stackTrace)
        {
            var pass = stackTrace.Replace("(native:4:52) -> var module = nativeRequire.invoke(id);", "(native)");
            pass = pass.Replace("    at", "  at");
            return pass.Replace("(native:4:52)", "(native)");
        }
    }
}
