using Fighters.models.armors;
using Fighters.models.classes;
using Fighters.models.fighters;
using Fighters.models.races;
using Fighters.models.weapons;

namespace Fighters;

public class FighterFactory
{
    public static IFighter Create( string name, IArmor armor, IClass fighterClass, IRace race, IWeapon weapon )
    {
        return new Fighter( name, armor, race, fighterClass, weapon );
    }
}