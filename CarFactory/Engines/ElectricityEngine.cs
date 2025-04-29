namespace CarFactory.Engines;

public class ElectricityEngine : IEngine
{
    public EngineType Type => EngineType.Electricity;
    public int Power => 500;
    public double Efficiency => 0.90;
}