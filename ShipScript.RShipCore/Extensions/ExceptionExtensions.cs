using System;

namespace ShipScript.RShipCore.Extensions
{
    public static class ExceptionExtensions
    {
        public static T GetInnerMost<T>(this Exception exception) where T : class
        {
            var testEx = exception;
            T result = null;
            while (testEx != null)
            {
                var targetEx = testEx as T;
                if (targetEx != null)
                {
                    result = targetEx;
                }

                testEx = testEx.InnerException;
            }

            return result;
        }
    }
}
