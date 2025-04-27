namespace CarFactory.Engines;

public class GasolineEngine : IEngine
{
    public EngineType Type =>  EngineType.Gasoline;
    public int Power => 120;
    public double Efficiency => 0.30;
}