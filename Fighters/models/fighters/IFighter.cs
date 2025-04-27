using Fighters.models.armors;

namespace Fighters.models.fighters;

public interface IFighter
{
    string Name { get; }

    int Armor { get; }

    int Health { set; get; }

    int Strength { get; }

    int Initiative { get; }

    string FighterConfiguration();
}