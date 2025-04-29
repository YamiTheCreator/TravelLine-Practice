using CarFactory.Colors;
using CarFactory.CarBodyShapes;
using CarFactory.Engines;
using CarFactory.Transmissions;

namespace CarFactory.Cars
{
    public interface ICar
    {
        IEngine Engine { get; }
        ITransmission Transmission { get; }
        ICarBodyShape BodyShape { get; }
        ColorType Color { get; }
        int MaxSpeed { get; }

        public string ToString();
    }
}