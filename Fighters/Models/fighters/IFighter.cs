using Fighters.Models.Armors;

namespace Fighters.Models.Fighters
{
    public interface IFighter
    {
        string Name { get; }

        int Armor { get; }

        int Health { get; }

        int Strength { get; }

        int Initiative { get; }

        public int GetCurrentHealth();

        public string ToString();

        void TakeDamage( int damage );
        int CalculateDamage();
    }
}