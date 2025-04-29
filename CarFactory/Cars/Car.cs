using CarFactory.Colors;
using CarFactory.CarBodyShapes;
using CarFactory.Engines;
using CarFactory.Transmissions;
using Spectre.Console;

namespace CarFactory.Cars
{
    public class Car( IEngine engine, ITransmission transmission, ICarBodyShape bodyShape, ColorType color )
        : ICar
    {
        public IEngine Engine { get; } = engine;
        public ITransmission Transmission { get; } = transmission;
        public ICarBodyShape BodyShape { get; } = bodyShape;
        public ColorType Color { get; } = color;

        public int MaxSpeed
        {
            get
            {
                if ( Engine == null )
                    throw new InvalidOperationException( "Engine is not initialized." );

                if ( Transmission == null )
                    throw new InvalidOperationException( "Transmission is not initialized." );

                if ( Engine.Power <= 0 )
                    throw new InvalidOperationException( "Engine power must be positive." );

                if ( Transmission.GearCount <= 0 )
                    throw new InvalidOperationException( "Gear count must be positive." );

                if ( Engine.Efficiency is <= 0 or > 1 )
                    throw new InvalidOperationException( "Engine efficiency must be in range (0, 1]." );

                if ( Transmission.Efficiency is <= 0 or > 1 )
                    throw new InvalidOperationException( "Transmission efficiency must be in range (0, 1]." );

                return ( int )(
                    _metersPerSecondToKmH *
                    Math.Sqrt( Engine.Power * _wattsToHorsepower ) *
                    ( 1 + Transmission.GearCount / _gearCountMultiplier ) *
                    Engine.Efficiency *
                    Transmission.Efficiency *
                    _aerodynamicDragFactor
                );
            }
        }

        public override string ToString()
        {
            return $"Car Configuration:\n" +
                   $"Engine: {Engine.Type} ({Engine.Power} HP, Efficiency: {Engine.Efficiency:P0})\n" +
                   $"Transmission: {Transmission.Type} ({Transmission.GearCount} gears, Efficiency: {Transmission.Efficiency:P0})\n" +
                   $"Max Speed: {MaxSpeed} km/h\n" +
                   $"Body: {BodyShape.Type}\n" +
                   $"Color: {Color}";
        }

        private const double _wattsToHorsepower = 735.5;
        private const double _metersPerSecondToKmH = 3.6;
        private const double _aerodynamicDragFactor = 0.3;
        private const double _gearCountMultiplier = 20.0;
    }
}