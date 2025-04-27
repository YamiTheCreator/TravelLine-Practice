using Fighters.models.armors;
using Fighters.models.classes;
using Fighters.models.fighters;
using Fighters.models.races;
using Fighters.models.weapons;
using Spectre.Console;

namespace Fighters.GameEngine;

public class GameManager
{
    public void StartGame( List<IFighter> fighters )
    {
        Console.WriteLine( "Greetings! Welcome to Fighters!" );
        bool isGame = true;

        while ( isGame )
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title( "What do you want to do?" )
                    .PageSize( 10 )
                    .AddChoices(
                        "Create Character",
                        "Start Game",
                        "Exit" ) );

            switch ( choice )
            {
                case "Create Character":
                    AnsiConsole.Clear();

                    var character = CreateCharacter();
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

                    isGame = false;
                    AnsiConsole.MarkupLine( "[yellow]Thank you for playing! Goodbye![/]" );
                    break;
            }
        }
    }

    private IFighter CreateCharacter()
    {
        string GetName() => AnsiConsole.Prompt(
            new TextPrompt<string>( "What's your name, hero?" ) );

        T SelectOption<T>( string title, string color, List<T> options ) where T : class
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<T>()
                    .Title( $"[gray]Choose your [{color}]{title}[/]: [/]" )
                    .PageSize( 10 )
                    .UseConverter( item => item.GetType().Name )
                    .AddChoices( options ) );
        }

        return FighterFactory.Create(
            GetName(),
            SelectOption<IArmor>( "armor", "blue",
                new()
                {
                    new Helmet(),
                    new Chestplate(),
                    new Pants(),
                    new Boots(),
                    new NoArmor()
                } ),
            SelectOption<IClass>( "class", "green", new()
            {
                new Druid(),
                new Palladin(),
                new Rogue(),
                new Wizard()
            } ),
            SelectOption<IRace>( "race", "orange3", new()
            {
                new Elf(),
                new Gnome(),
                new Human(),
                new Orc()
            } ),
            SelectOption<IWeapon>( "weapon", "red3", new()
            {
                new Charm(),
                new Dagger(),
                new Staff(),
                new Sword()
            } )
        );
    }

    private void StartSimulation( List<IFighter> fighters )
    {
        var aliveFighters = fighters.Select( f => new Fighter(
            f.Name,
            f.Armor,
            f.Health,
            f.Strength,
            f.Initiative ) ).Cast<IFighter>().ToList();

        int round = 1;

        while ( aliveFighters.Count > 1 )
        {
            AnsiConsole.MarkupLine( $"[underline]Round {round}[/]" );
            GoOneRound( aliveFighters );
            round++;

            aliveFighters.RemoveAll( f => f.Health <= 0 );
        }

        var winner = aliveFighters.First();
        AnsiConsole.MarkupLine( $"[green]Winner: {winner.Name} with {winner.Health} HP remaining![/]" );
    }

    private void GoOneRound( List<IFighter> fighters )
    {
        const float CRIT_CHANCE = 0.15f;
        const float CRIT_MULTIPLIER = 1.5f;

        var orderedFighters = fighters.OrderByDescending( f => f.Initiative ).ToList();

        foreach ( var attacker in orderedFighters.Where( f => f.Health > 0 ) )
        {
            var possibleTargets = fighters.Where( f => f != attacker && f.Health > 0 ).ToList();
            if ( !possibleTargets.Any() ) break;

            var target = possibleTargets[ Random.Shared.Next( possibleTargets.Count ) ];

            int baseDamage = Math.Max( attacker.Strength - target.Armor, 0 );

            bool isCritical = Random.Shared.NextSingle() < CRIT_CHANCE;

            float damageMultiplier = 0.8f + ( Random.Shared.NextSingle() * 0.3f ); // 0.8-1.1

            if ( isCritical )
            {
                damageMultiplier *= CRIT_MULTIPLIER;
            }

            int randomizedDamage = ( int )Math.Round( baseDamage * damageMultiplier );
            int finalDamage = Math.Max( randomizedDamage, 1 );

            target.Health -= finalDamage;

            AnsiConsole.MarkupLine(
                $"[red]{attacker.Name}[/] attacks [blue]{target.Name}[/] " +
                $"- [dim](base: {baseDamage}, modifier: {damageMultiplier:P0}{( isCritical ? " + CRIT" : "" )})[/]\n" +
                $"[blue]{target.Name}[/] has [red]{target.Health}[/] HP left." );
        }
    }
}