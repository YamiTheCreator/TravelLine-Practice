namespace CarFactory.Transmissions
{
    public interface ITransmission
    {
        public TransmissionType Type { get; }
        public int GearCount { get; }

        public double Efficiency { get; }
    }
}