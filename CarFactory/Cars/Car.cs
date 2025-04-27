using System.Drawing;
using CarFactory.Colors;
using CarFactory.CarBodyShapes;
using CarFactory.Engines;
using CarFactory.Transmissions;

namespace CarFactory.Cars;

public class Car : ICar
{
    public IEngine Engine { get; }
    public ITransmission Transmission { get; }
    public ICarBodyShape BodyShape { get; }
    public ColorType Color { get; }

    public Car( IEngine engine, ITransmission transmission, ICarBodyShape bodyShape, ColorType color )
    {
        Engine = engine;
        Transmission = transmission;
        BodyShape = bodyShape;
        Color = color;
    }

    public int MaxSpeed => ( int )( 3.6 *
                                    double.Sqrt( Engine.Power * 735.5 ) *
                                    ( 1 + Transmission.GearCount / 20.0 ) *
                                    Engine.Efficiency *
                                    Transmission.Efficiency * 0.3 );

    public string GetConfiguration()
    {
        return $"Engine: {Engine.Type}\n" +
               $"Transmission: {Transmission.Type}\n" +
               $"BodyShape: {BodyShape.Type}\n" +
               $"Color: {Color}\n" +
               $"Max Speed: {MaxSpeed}\n" +
               $"Gear Count: {Transmission.GearCount}\n";
    }
}