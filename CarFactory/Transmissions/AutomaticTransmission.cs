namespace CarFactory.Transmissions
{
    public class AutomaticTransmission : ITransmission
    {
        public TransmissionType Type => TransmissionType.Automatic;
        public int GearCount => 8;

        public double Efficiency => 0.85;
    }
}