using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Spectre.Console;

namespace Fighters.GameEngine
{
    public static class GameManager
    {
        public static void GameController( List<IFighter> fighters )
        {
            Console.WriteLine( "Greetings! Welcome to Fighters!" );
            bool isGameRunning = true;

            while ( isGameRunning )
            {
                string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title( "What do you want to do?" )
                        .PageSize( _pageSize )
                        .AddChoices(
                            "Create Character",
                            "Start Game",
                            "Exit" ) );

                switch ( choice )
                {
                    case "Create Character":
                        AnsiConsole.Clear();

                        IFighter character = CreateCharacter();
                        Console.WriteLine( character );
                        fighters.Add( character );
                        AnsiConsole.MarkupLine( $"[green]Character '{character.Name}' created successfully![/]" );
                        break;

                    case "Start Game":
                        AnsiConsole.Clear();

                        if ( fighters.Count < 2 )
                        {
                            AnsiConsole.MarkupLine(
                                "[red]No characters available! Create at least two character first.[/]" );
                        }
                        else
                        {
                            StartSimulation( fighters );
                        }

                        break;

                    case "Exit":
                        AnsiConsole.Clear();

                        isGameRunning = false;
                        AnsiConsole.MarkupLine( "[yellow]Thank you for playing! Goodbye![/]" );
                        break;
                }
            }
        }

        private static IFighter CreateCharacter()
        {
            return FighterFactory.Create(
                GetName( "What's your name, hero?" ),
                SelectOption<IArmor>( "armor", "blue",
                [
                    new Helmet(),
                    new Chestplate(),
                    new Pants(),
                    new Boots(),
                    new NoArmor()
                ] ),
                SelectOption<IClass>( "class", "green", [
                    new Druid(),
                    new Palladin(),
                    new Rogue(),
                    new Wizard()
                ] ),
                SelectOption<IRace>( "race", "orange3", [
                    new Elf(),
                    new Gnome(),
                    new Human(),
                    new Orc()
                ] ),
                SelectOption<IWeapon>( "weapon", "red3", [
                    new Charm(),
                    new Dagger(),
                    new Staff(),
                    new Sword()
                ] )
            );

            T SelectOption<T>( string title, string color, List<T> options ) where T : class
            {
                return AnsiConsole.Prompt(
                    new SelectionPrompt<T>()
                        .Title( $"[gray]Choose your [{color}]{title}[/]: [/]" )
                        .PageSize( _pageSize )
                        .UseConverter( item => item.GetType().Name )
                        .AddChoices( options ) );
            }
        }

        private static void StartSimulation( List<IFighter> fighters )
        {
            List<IFighter> aliveFighters =
                fighters.Select( FighterFactory.Clone ).OrderByDescending( f => f.Initiative ).ToList();
            int round = 1;

            while ( aliveFighters.Count > 1 )
            {
                AnsiConsole.MarkupLine( $"[underline]Round {round}[/]" );
                GoOneRound( aliveFighters );
                round++;
            }

            IFighter winner = aliveFighters.First();

            AnsiConsole.MarkupLine( $"[green]Winner: {winner.Name} with {winner.GetCurrentHealth()} HP remaining![/]" );
        }

        private static void GoOneRound( List<IFighter> fighters )
        {
            foreach ( IFighter attacker in fighters.ToList() )
            {
                if ( attacker.GetCurrentHealth() <= 0 )
                {
                    continue;
                }

                List<IFighter> possibleTargets = fighters.Where( f => f != attacker ).ToList();

                if ( possibleTargets.Count == 0 )
                {
                    break;
                }

                IFighter target = possibleTargets[ Random.Shared.Next( possibleTargets.Count ) ];

                int damage = attacker.CalculateDamage();

                target.TakeDamage( damage );

                fighters.RemoveAll( f => f.GetCurrentHealth() <= 0 );

                AnsiConsole.MarkupLine(
                    $"[red]{attacker.Name}[/] attacks [blue]{target.Name}[/] " +
                    $"- [dim](damage: {damage})[/]\n" +
                    $"[blue]{target.Name}[/] has [red]{target.GetCurrentHealth()}[/] HP left." );
            }
        }

        private static string GetName( string prompt ) => AnsiConsole.Prompt(
            new TextPrompt<string>( prompt ) );

        private const int _pageSize = 10;
    }
}