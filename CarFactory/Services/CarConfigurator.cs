using CarFactory.CarBodyShapes;
using CarFactory.Cars;
using CarFactory.Colors;
using CarFactory.Engines;
using CarFactory.Transmissions;
using Spectre.Console;

namespace CarFactory.Services;

public class CarConfigurator
{
    public static ICar Configure()
    {
        var bodyShapeType = AnsiConsole.Prompt(
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

        var engineType = AnsiConsole.Prompt(
            new SelectionPrompt<EngineType>()
                .Title( "Select engine type:" )
                .AddChoices( Enum.GetValues<EngineType>() )
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

        var transmissionType = AnsiConsole.Prompt(
            new SelectionPrompt<TransmissionType>()
                .Title( "Select transmission type:" )
                .AddChoices( Enum.GetValues<TransmissionType>() )
        );

        ITransmission transmission = transmissionType switch
        {
            TransmissionType.Manual => new ManualTransmission(),
            TransmissionType.Automatic => new AutomaticTransmission(),
            TransmissionType.Robotic => new RoboticTransmission(),
            TransmissionType.Variable => new VariableTransmission(),
            _ => throw new ArgumentException( "Invalid transmission type" )
        };

        var color = AnsiConsole.Prompt(
            new SelectionPrompt<ColorType>()
                .Title( "Select car color:" )
                .AddChoices( Enum.GetValues<ColorType>() )
        );

        return CarFactory.Create( engine, transmission, bodyShape, color );
    }
}