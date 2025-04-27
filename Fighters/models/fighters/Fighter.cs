using Fighters.models.armors;
using Fighters.models.classes;
using Fighters.models.races;
using Fighters.models.weapons;

namespace Fighters.models.fighters;

public class Fighter : IFighter
{
    public string Name { get; }
    public int Armor { get; }
    public int Health { set; get; }
    public int Strength { get; }

    public int Initiative { get; }

    public Fighter( string name, IArmor armor, IRace race, IClass fighterClass, IWeapon weapon )
    {
        Name = name;
        Armor = armor.Armor + race.Armor;
        Health = race.Health + fighterClass.Health;
        Strength = race.Strength + fighterClass.Strength + weapon.Strength;
        Initiative = GetInitiative();
    }

    public Fighter( string name, int armor, int health, int strength, int initiative )
    {
        Name = name;
        Armor = armor;
        Health = health;
        Strength = strength;
        Initiative = initiative;
    }

    private static int GetInitiative()
    {
        return Random.Shared.Next( 1, 100 );
    }

    public string FighterConfiguration()
    {
        return $"Name: {Name}\n" +
               $"Armor: {Armor}\n" +
               $"Health: {Health}\n" +
               $"Strength: {Strength}\n" +
               $"Initiative: {Initiative}\n";
    }
}