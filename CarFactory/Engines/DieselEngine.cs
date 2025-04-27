namespace CarFactory.Engines;

public class DieselEngine : IEngine
{
    public EngineType Type =>  EngineType.Diesel;
    public int Power => 120;
    public double Efficiency => 0.40;
}