namespace ShipScript.RShipCore.Pipes.TransferModel
{
    public interface ICompositeOutput
    {
        IWritableStream DataOutput { get; }
         
        IWritableStream AltOutput { get; }

        IWritableStream LogOutput { get; }

        IWritableStream ErrOutput { get; }

        IWritableStream CoreOutput { get; }

        IWritableStream CommandOutput { get; }

        IWritableStream ResultOutput { get; }
    }
}
