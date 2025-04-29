using CarFactory.Cars;
using CarFactory.Services;

namespace CarFactory
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine( "Welcome to car configurator!" );

            ICar car = CarConfigurator.Configure();
            Console.WriteLine( car );
        }
    }
}