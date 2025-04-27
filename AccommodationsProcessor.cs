using System.Globalization;
using Accommodations.Commands;
using Accommodations.Dto;

namespace Accommodations;

public static class AccommodationsProcessor
{
    private static BookingService _bookingService = new();
    private static Dictionary<int, ICommand> _executedCommands = new();
    private static int s_commandIndex = 0;

    public static void Run()
    {
        Console.WriteLine("Booking Command Line Interface");
        Console.WriteLine("Commands:");
        Console.WriteLine("'book <UserId> <Category> <StartDate> <EndDate> <Currency>' - to book a room");
        Console.WriteLine("'cancel <BookingId>' - to cancel a booking");
        Console.WriteLine("'undo' - to undo the last command");
        Console.WriteLine("'find <BookingId>' - to find a booking by ID");
        Console.WriteLine("'search <StartDate> <EndDate> <CategoryName>' - to search bookings");
        Console.WriteLine("'exit' - to exit the application");

        string input;
        while ((input = Console.ReadLine()) != "exit")
        {
            try
            {
                ProcessCommand(input);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            //добавил новый блок catch
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            //добавил новый блок catch
            catch (FormatException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private static void ProcessCommand(string input)
    {
        string[] parts = input.Split(' ');
        string commandName = parts[0];

        switch (commandName)
        {
            case "book":
                if (parts.Length != 6)
                {
                    Console.WriteLine("Invalid number of arguments for booking.");
                    return;
                }

                //исправил проверку, добавил исключение
                if ( !Enum.TryParse( typeof( CurrencyDto ), parts[ 5 ], ignoreCase: true, out object? currency ) )
                {
                    throw new ArgumentException( $"Invalid currency {parts[ 5 ]}" );
                }

                //теперь даты проходят доп валидацию
                if ( TryParseDate( parts[ 3 ] ) < DateTime.Now.Date )
                {
                    throw new ArgumentException( "Start date cannot be earlier than today" );
                }

                if (TryParseDate( parts[ 4 ] ) < TryParseDate( parts[ 3 ] ))
                {
                    throw new ArgumentException( "End date cannot be earlier than start date" );
                }

                BookingDto bookingDto = new()
                {
                    UserId = int.Parse( parts[ 1 ] ),
                    Category = parts[2],
                    StartDate = TryParseDate( parts[ 3 ] ),
                    EndDate = TryParseDate( parts[ 4 ] ),
                    Currency = ( CurrencyDto ) currency,
                };

                BookCommand bookCommand = new(_bookingService, bookingDto);
                bookCommand.Execute();
                _executedCommands.Add(++s_commandIndex, bookCommand);
                Console.WriteLine("Booking command run is successful.");
                break;

            case "cancel":
                if (parts.Length != 2)
                {
                    Console.WriteLine("Invalid number of arguments for canceling.");
                    return;
                }
                //применил функцию TryParseBookingId
                Guid bookingId = TryParseBookingId(parts[1]);
                CancelBookingCommand cancelCommand = new(_bookingService, bookingId);
                cancelCommand.Execute();
                _executedCommands.Add(++s_commandIndex, cancelCommand);
                Console.WriteLine("Cancellation command run is successful.");
                break;

            case "undo":
                //добавил проверку на отсутствие команд в списке
                if ( _executedCommands.Count == 0 )
                {
                    throw new KeyNotFoundException( "No booking commands found for booking." );
                }
                _executedCommands[s_commandIndex].Undo();
                _executedCommands.Remove(s_commandIndex);
                s_commandIndex--;
                Console.WriteLine("Last command undone.");

                break;
            case "find":
                if (parts.Length != 2)
                {
                    Console.WriteLine("Invalid arguments for 'find'. Expected format: 'find <BookingId>'");
                    return;
                }
                //применил функцию TryParseBookingId
                Guid id = TryParseBookingId(parts[1]);
                FindBookingByIdCommand findCommand = new(_bookingService, id);
                findCommand.Execute();
                //добавил добавление команды в список команд
                _executedCommands.Add(++s_commandIndex, findCommand);
                Console.WriteLine("Find command run is successful.");
                break;

            case "search":
                if (parts.Length != 4)
                {
                    Console.WriteLine("Invalid arguments for 'search'. Expected format: 'search <StartDate> <EndDate> <CategoryName>'");
                    return;
                }
                //применил функцию TryParseDate
                DateTime startDate = TryParseDate(parts[1]);
                //применил функцию TryParseDate
                DateTime endDate = TryParseDate(parts[2]);
                string categoryName = parts[3];
                SearchBookingsCommand searchCommand = new(_bookingService, startDate, endDate, categoryName);
                searchCommand.Execute();
                //добавил добавление команды в список команд
                _executedCommands.Add(++s_commandIndex, searchCommand);
                Console.WriteLine("Search command run is successful.");
                break;

            default:
                //добавил указание команды в вывод
                Console.WriteLine( $"Unknown command: {commandName}" );
                break;
        }
    }

    //добавил функцию для проверки даты
    private static DateTime TryParseDate( string date )
    {
        if ( !DateTime.TryParse( date, out DateTime result ) )
        {
            throw new FormatException( "Invalid date format. Try mm/dd/yyyy" );
        }

        return result;
    }

    //добавил функцию для проверки id
    private static Guid TryParseBookingId( string bookingId )
    {
        if (  !Guid.TryParse( bookingId, out Guid result ) )
        {
            throw new FormatException( "Invalid booking id!" );
        }

        return result;
    }
}
