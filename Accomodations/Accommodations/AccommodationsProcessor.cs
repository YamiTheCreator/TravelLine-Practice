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
            //добавил обработку исключений
            catch (FormatException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            //добавил обработку исключений
            catch (InvalidOperationException ex)
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
                //изменил логику проверки, добавил исключение
                if (!Enum.TryParse(typeof(CurrencyDto), parts[5], ignoreCase: true, out object? currency))
                {
                    throw new ArgumentException($"Invalid currency: {parts[5]}");
                }

                DateTime startBookingDate = TryParseDate( parts[3] );
                DateTime endBookingDate = TryParseDate( parts[4] );
                //добавил проверку на то, что конечная дата раньше начальной
                if (endBookingDate < startBookingDate)
                {
                    throw new ArgumentException($"Start date {startBookingDate} is before end date {endBookingDate}");
                }
                //перенес проверку на то, что бронируемая дата может оказаться в прошедшем времени
                if (startBookingDate < DateTime.Now.Date)
                {
                    throw new ArgumentException($"Start date {startBookingDate} cannot be earlier than the current date {DateTime.Now.Date}");
                }

                BookingDto bookingDto = new()
                {
                    UserId = int.Parse(parts[1]),
                    Category = parts[2],
                    StartDate = DateTime.Parse(parts[3]),
                    EndDate = DateTime.Parse(parts[4]),
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
                //добавлен парсинг id
                Guid bookingId = TryParseId( parts[1] );
                CancelBookingCommand cancelCommand = new(_bookingService, bookingId);
                cancelCommand.Execute();
                _executedCommands.Add(++s_commandIndex, cancelCommand);
                Console.WriteLine("Cancellation command run is successful.");
                break;

            case "undo":
                //добавил проверку на количество команд
                if (_executedCommands.Count == 0)
                {
                    throw new InvalidOperationException("No booking commands have been executed.");
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
                //добавлен парсинг id
                Guid id = TryParseId( parts[1] );

                FindBookingByIdCommand findCommand = new(_bookingService, id);

                findCommand.Execute();

                _executedCommands.Add( ++s_commandIndex, findCommand );
                break;

            case "search":
                if (parts.Length != 4)
                {
                    Console.WriteLine("Invalid arguments for 'search'. Expected format: 'search <StartDate> <EndDate> <CategoryName>'");
                    return;
                }
                DateTime startDate = DateTime.Parse(parts[1]);
                DateTime endDate = DateTime.Parse(parts[2]);
                string categoryName = parts[3].ToLower().Trim();
                SearchBookingsCommand searchCommand = new(_bookingService, startDate, endDate, categoryName);
                searchCommand.Execute();
                _executedCommands.Add(++s_commandIndex, searchCommand);
                break;

            default:
                Console.WriteLine("Unknown command.");
                break;
        }
    }
//добавил методы на корректный парсинг даты и id
    private static DateTime TryParseDate( string date )
    {
        if (!DateTime.TryParse(date, out DateTime result))
        {
            throw  new FormatException("Invalid format. Try mm/dd/yyyy");
        }

        return result;
    }

    private static Guid TryParseId( string id )
    {
        if (!Guid.TryParse( id, out Guid result) )
        {
            throw new FormatException("Invalid id format");
        }

        return result;
        }
    }
