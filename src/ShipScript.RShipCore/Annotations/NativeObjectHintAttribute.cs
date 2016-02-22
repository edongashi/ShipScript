using System;

namespace ShipScript.RShipCore
{
    public class NativeObjectHintAttribute : Attribute
    {
        public NativeObjectHintAttribute(string nativeType = null)
        {
            NativeType = nativeType;
        }

        public string NativeType { get; }
    }
}
