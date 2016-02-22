using ShipScript.Common;
using ShipScript.RShipCore.NativeTypes;

namespace ShipScript.RShipCore.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToScriptString(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            string result = obj as string;
            if (result != null)
            {
                return result;
            }

            try
            {
                return ((dynamic)obj).toString();
            }
            catch
            {
                return obj.ToString();
            }
        }

        public static bool IsDefined(this object obj)
        {
            return !(obj is Undefined || obj is VoidResult);
        }
    }
}
