namespace CarFactory.Engines;

public interface IEngine
{
    public EngineType Type { get; }
    public int Power { get; }
    public double Efficiency { get; }
}