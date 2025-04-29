namespace CarFactory.Transmissions
{
    public class VariableTransmission : ITransmission
    {
        public TransmissionType Type => TransmissionType.Variable;

        //вообще виртуальные передачи, но так нагляднее
        public int GearCount => 7;

        public double Efficiency => 0.80;
    }
}