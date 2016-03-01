using Microsoft.ClearScript;

namespace ShipScript.RShipCore.Pipes
{
    public interface IPipe : IEventConnection
    {
        [ScriptMember("source", ScriptMemberFlags.ExposeRuntimeType)]
        IReadableStream Source { get; }

        [ScriptMember("target", ScriptMemberFlags.ExposeRuntimeType)]
        IWritableStream Target { get; }

        [ScriptMember("transform")]
        [NativeObjectHint]
        object TransformFunction { get; set; }

        [ScriptMember("write")]
        void Write(object value);

        [ScriptMember("break")]
        void BreakPipe();
    }
}
