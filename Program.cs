namespace OrderManager;

class Program
{
    static string ValidateInput( string promt )
    {
        string input;
        do
        {
            Console.Write( promt );
            input = Console.ReadLine();
            if ( string.IsNullOrEmpty( input ) )
            {
                Console.WriteLine( "This field must not be empty. Please try again." );
            }
        } while ( string.IsNullOrWhiteSpace( input ) );

        return input;
    }

    static void Request( out string product, out int amount, out string username, out string address )
    {
        product = ValidateInput( "Product name: " );

        Console.Write( "Enter amount of products: " );
        while ( !int.TryParse( Console.ReadLine(), out amount ) || amount <= 0 )
        {
            Console.WriteLine( "Invalid input. Please enter a positive number: " );
        }

        username = ValidateInput( "Enter your name: " );

        address = ValidateInput( "Enter your address: " );
    }

    static bool Confirm( string product, int amount, string username, string address )
    {
        Console.WriteLine( $"Hello, {username}, you have ordered {amount} {product} to {address}, is it right?" );
        Console.Write( "Press Y/y to confirm, any other key to cancel: " );
        return Console.ReadLine().ToLower() == "y";
    }

    static void Order( string product, int amount, string username, string address )
    {
        DateTime date = DateTime.Now.AddDays( 3 );
        Console.WriteLine(
            $"{username}! Your {product} order in the amount of {amount} has been placed! Expect delivery to {address} by {date
                :dd.MM.yyyy}" );
    }

    static void Main( string[] args )
    {
        string product;
        int amount;
        string username;
        string address;

        Request( out product, out amount, out username, out address );
        if ( Confirm( product, amount, username, address ) )
        {
            Order( product, amount, username, address );
        }
        else
        {
            Console.WriteLine( "Order failed. Please try again." );
        }
    }
}