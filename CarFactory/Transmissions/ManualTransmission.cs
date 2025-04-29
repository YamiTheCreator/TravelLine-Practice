namespace CarFactory.Transmissions
{
    public class ManualTransmission  : ITransmission
    {
        public TransmissionType Type => TransmissionType.Manual;
    
        public int GearCount => 5;

        public double Efficiency => 0.98;
    }
}