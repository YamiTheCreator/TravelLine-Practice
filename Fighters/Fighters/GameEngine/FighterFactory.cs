using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.GameEngine
{
    public static class FighterFactory
    {
        public static IFighter Create( string name, IArmor armor, IClass fighterClass, IRace race, IWeapon weapon )
        {
            return new Fighter( name, armor, race, fighterClass, weapon );
        }

        public static IFighter Clone( IFighter fighter )
        {
            return new Fighter( fighter.Name, fighter.Armor, fighter.Health, fighter.Strength, fighter.Initiative );
        }
    }
}