namespace Fighters.Models.Races
{
    public class Human : IRace
    {
        public int Health => 100;
        public int Strength => 8;
        public int Armor => 10;
    }
}