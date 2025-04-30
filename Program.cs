namespace OrderManager
{
    internal static class Program
    {
        private const string _confirmLiteral = "y";

        private static string TryGetValidInput( string prompt )
        {
            Console.Write( prompt );

            string? input = Console.ReadLine();

            if ( string.IsNullOrWhiteSpace( input ) )
            {
                throw new ArgumentException( "Input cannot be empty" );
            }

            return input;
        }

        private static int TryGetValidNumber( string prompt )
        {
            Console.Write( prompt );
            string? input = Console.ReadLine();

            if ( string.IsNullOrWhiteSpace( input ) )
            {
                throw new ArgumentException( "Input cannot be empty" );
            }

            if ( !int.TryParse( input, out int number ) || number < 0 )
            {
                throw new ArgumentException( "Invalid number. Must be a positive integer" );
            }

            return number;
        }

        private static void Request( out string product, out int amount, out string username, out string address )
        {
            product = TryGetValidInput( "Product name: " );

            amount = TryGetValidNumber( "Amount of product: " );

            username = TryGetValidInput( "Enter your name: " );

            address = TryGetValidInput( "Enter your address: " );
        }

        private static bool Confirm( string product, int amount, string username, string address )
        {
            Console.WriteLine( $"Hello, {username}, you have ordered {amount} {product} to {address}, is it right?" );
            Console.Write(
                $"Press {_confirmLiteral.ToUpper() + "/" + _confirmLiteral} to confirm, any other key to cancel: " );
            return Console.ReadLine()?.ToLower() == _confirmLiteral;
        }

        private static void Order( string product, int amount, string username, string address )
        {
            DateTime date = DateTime.Now.AddDays( 3 );
            Console.WriteLine(
                $"{username}! Your {product} order in the amount of {amount} has been placed! Expect delivery to {address} by {date:dd.MM.yyyy}" );
        }

        private static int Main()
        {
            try
            {
                Request( out string product, out int amount, out string username, out string address );

                if ( !Confirm( product, amount, username, address ) )
                {
                    Console.WriteLine( "Your order could not be placed. Please try again later" );
                    return 0;
                }

                Order( product, amount, username, address );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e );
                return 1;
            }

            return 0;
        }
    }
}