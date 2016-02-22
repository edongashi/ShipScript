using System;

namespace ShipScript.RShipCore
{
    public class NativeObjectAttribute : Attribute
    {
        public NativeObjectAttribute(string nativeType = null)
        {
            NativeType = nativeType;
        }

        public string NativeType { get; }
    }
}
