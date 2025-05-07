namespace CarFactory.Engines
{
    public class HybridEngine : IEngine
    {
        public EngineType Type => EngineType.Hybrid;
        public int Power => 300;
        public double Efficiency => 0.35;
    }
}