namespace CarFactory.Transmissions
{
    public class RoboticTransmission : ITransmission
    {
        public TransmissionType Type => TransmissionType.Robotic;
        public int GearCount => 7;

        public double Efficiency => 0.90;
    }
}