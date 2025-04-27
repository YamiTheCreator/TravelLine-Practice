namespace CarFactory.Engines;

public class GasPoweredEngine  : IEngine
{
    public EngineType Type =>  EngineType.GasPowered;
    public int Power => 500;
    public int Torque => 800;
    public double Efficiency => 0.90;
}