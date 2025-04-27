using CarFactory.CarBodyShapes;
using CarFactory.Cars;
using CarFactory.Colors;
using CarFactory.Engines;
using CarFactory.Transmissions;

namespace CarFactory;

public static class CarFactory
{
    public static ICar Create( IEngine engine, ITransmission transmission, ICarBodyShape carBodyShape, ColorType color )
    {
        return new Car( engine, transmission, carBodyShape, color );
    }
}