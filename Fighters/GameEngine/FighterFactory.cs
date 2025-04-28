using System.Runtime.CompilerServices;

using Fighters.models.armors;
using Fighters.models.classes;
using Fighters.models.fighters;
using Fighters.models.races;
using Fighters.models.weapons;

namespace Fighters.GameEngine;

public class FighterFactory
{
    public static IFighter Create( string name, IArmor armor, IClass fighterClass, IRace race, IWeapon weapon )
    {
        return new Fighter( name, armor, race, fighterClass, weapon );
    }
    
    public static IFighter Clone(IFighter fighter)
    {
        return new Fighter(fighter.Name, fighter.Armor, fighter.Health, fighter.Strength, fighter.Initiative);
    }
}