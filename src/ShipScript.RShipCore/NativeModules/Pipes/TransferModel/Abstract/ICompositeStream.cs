namespace ShipScript.RShipCore.Pipes.TransferModel
{
    public interface ICompositeStream<out TStream> where TStream : IReadableStream
    {
        TStream DataStream { get; }

        TStream AltStream { get; }

        TStream LogStream { get; }

        TStream ErrorStream { get; }

        TStream CoreStream { get; }

        TStream CommandStream { get; }

        TStream ResultStream { get; }
    }
}
