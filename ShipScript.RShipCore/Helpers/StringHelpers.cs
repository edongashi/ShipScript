namespace ShipScript.RShipCore.Helpers
{
    public static class StringHelpers
    {
        public static string RemoveNativeLineNumbers(string stackTrace)
        {
            var pass = stackTrace.Replace("(native:4:52) -> var module = nativeRequire.invoke(id);", "(native)");
            return pass.Replace("(native:4:52)", "(native)");
        }
    }
}
