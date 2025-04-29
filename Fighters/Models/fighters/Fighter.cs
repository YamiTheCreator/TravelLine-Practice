using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters
{
    public class Fighter : IFighter
    {
        private int _currentHealth;
        
        public string Name { get; }
        public int Armor { get; }
        public int Health { get; }
        public int Strength { get; }
        public int Initiative { get; }

        public Fighter( string name, IArmor armor, IRace race, IClass fighterClass, IWeapon weapon )
        {
            Name = name;
            Armor = armor.Armor + race.Armor;
            Health = race.Health + fighterClass.Health;
            Strength = race.Strength + fighterClass.Strength + weapon.Strength;
            Initiative = GetInitiative();
            _currentHealth = Health;
        }

        public Fighter( string name, int armor, int health, int strength, int initiative )
        {
            Name = name;
            Armor = armor;
            Health = health;
            Strength = strength;
            Initiative = initiative;
        }

        public void TakeDamage( int damage )
        {
            _currentHealth -= damage;
        }

        public int CalculateDamage()
        {
            const int critChancePercent = 15;
            const int critMultiplierNumerator = 3;
            const int critMultiplierDenominator = 2;
    
            const int minDamageMultiplier = 8;
            const int maxDamageMultiplier = 11;
            const int multiplierDenominator = 10;
            
            bool isCritical = Random.Shared.Next(100) < critChancePercent;
            
            int baseDamage = Math.Max(Strength - Armor, 0);
            
            int damageMultiplier = Random.Shared.Next(minDamageMultiplier, maxDamageMultiplier + 1);
            
            if (isCritical)
            {
                damageMultiplier = damageMultiplier * critMultiplierNumerator / critMultiplierDenominator;
            }
            
            int finalDamage = Math.Max(baseDamage * damageMultiplier / multiplierDenominator, 1);
    
            return finalDamage;
        }
        
        public void OutputFighterConfiguration()
        {
            Console.WriteLine(
                $"Name: {Name}\n" +
                $"Armor: {Armor}\n" +
                $"Health: {Health}\n" +
                $"Strength: {Strength}\n" +
                $"Initiative: {Initiative}\n" );
        }

        public int GetCurrentHealth()
        {
            return _currentHealth;
        }
        

        private static int GetInitiative()
        {
            return Random.Shared.Next( 1, 100 );
        }
    }
}