using CarFactory.CarBodyShapes;
using CarFactory.Cars;
using CarFactory.Colors;
using CarFactory.Engines;
using CarFactory.Transmissions;
using Spectre.Console;

namespace CarFactory.Services
{
    public static class CarConfigurator
    {
        public static ICar Configure()
        {
            CarBodyShapeType bodyShapeType = AnsiConsole.Prompt(
                new SelectionPrompt<CarBodyShapeType>()
                    .Title( "Select car body shape:" )
                    .AddChoices( Enum.GetValues<CarBodyShapeType>() )
            );

            ICarBodyShape bodyShape = bodyShapeType switch
            {
                CarBodyShapeType.Sedan => new Sedan(),
                CarBodyShapeType.Hatchback => new Hatchback(),
                CarBodyShapeType.StationWagon => new StationWagon(),
                _ => throw new ArgumentException( "Invalid body shape" )
            };

            EngineType engineType = AnsiConsole.Prompt(
                new SelectionPrompt<EngineType>()
                    .Title( "Select engine type:" )
                    .AddChoices( Enum.GetValues<EngineType>() )
                    .UseConverter( type =>
                    {
                        IEngine sampleEngine = CreateSampleEngine( type );
                        return $"{type} [grey]({sampleEngine.Power} HP, Eff: {sampleEngine.Efficiency:P0})[/]";
                    } )
            );

            IEngine engine = engineType switch
            {
                EngineType.Gasoline => new GasolineEngine(),
                EngineType.Diesel => new DieselEngine(),
                EngineType.Electricity => new ElectricityEngine(),
                EngineType.Hybrid => new HybridEngine(),
                EngineType.GasPowered => new GasPoweredEngine(),
                _ => throw new ArgumentException( "Invalid engine type" )
            };

            TransmissionType transmissionType = AnsiConsole.Prompt(
                new SelectionPrompt<TransmissionType>()
                    .Title( "Select transmission type:" )
                    .AddChoices( Enum.GetValues<TransmissionType>() )
                    .UseConverter( type =>
                    {
                        ITransmission sampleTransmission = CreateSampleTransmission( type );
                        return
                            $"{type} [grey](Gears: {sampleTransmission.GearCount}, Eff: {sampleTransmission.Efficiency:P0})[/]";
                    } )
            );

            ITransmission transmission = transmissionType switch
            {
                TransmissionType.Manual => new ManualTransmission(),
                TransmissionType.Automatic => new AutomaticTransmission(),
                TransmissionType.Robotic => new RoboticTransmission(),
                TransmissionType.Variable => new VariableTransmission(),
                _ => throw new ArgumentException( "Invalid transmission type" )
            };

            ColorType color = AnsiConsole.Prompt(
                new SelectionPrompt<ColorType>()
                    .Title( "Select car color:" )
                    .AddChoices( Enum.GetValues<ColorType>() )
            );

            AnsiConsole.Clear();

            return CarFactory.Create( engine, transmission, bodyShape, color );
        }

        private static IEngine CreateSampleEngine( EngineType type )
        {
            return type switch
            {
                EngineType.Gasoline => new GasolineEngine(),
                EngineType.Diesel => new DieselEngine(),
                EngineType.Electricity => new ElectricityEngine(),
                EngineType.Hybrid => new HybridEngine(),
                EngineType.GasPowered => new GasPoweredEngine(),
                _ => throw new ArgumentException( "Invalid engine type" )
            };
        }

        private static ITransmission CreateSampleTransmission( TransmissionType type )
        {
            return type switch
            {
                TransmissionType.Manual => new ManualTransmission(),
                TransmissionType.Automatic => new AutomaticTransmission(),
                TransmissionType.Robotic => new RoboticTransmission(),
                TransmissionType.Variable => new VariableTransmission(),
                _ => throw new ArgumentException( "Invalid transmission type" )
            };
        }
    }
}