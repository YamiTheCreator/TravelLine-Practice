using CarFactory.Cars;
using CarFactory.Services;

namespace CarFactory;

class Program
{
    static void Main()
    {
        Console.WriteLine( "Welcome to car configurator!" );

        ICar car = CarConfigurator.Configure();

        Console.WriteLine( car.GetConfiguration() );
    }
}