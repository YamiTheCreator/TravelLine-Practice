using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.GameEngine.Tests
{
    public class FighterFactoryTests
    {
        [Fact]
        public void Create_ShouldReturnFighterWithCorrectProperties()
        {
            // Arrange
            var name = "TestFighter";
            var armor = new Helmet();
            var fighterClass = new Wizard();
            var race = new Elf();
            var weapon = new Staff();

            // Act
            var fighter = FighterFactory.Create(name, armor, fighterClass, race, weapon);

            // Assert
            Assert.Equal(name, fighter.Name);
            Assert.Equal(armor.Armor + race.Armor, fighter.Armor);
            Assert.Equal(race.Health + fighterClass.Health, fighter.Health);
            Assert.Equal(race.Strength + fighterClass.Strength + weapon.Strength, fighter.Strength);
            Assert.InRange(fighter.Initiative, 1, 100);
        }

        [Fact]
        public void Clone_ShouldReturnFighterWithSameProperties()
        {
            // Arrange
            var original = new Fighter("Original", new Chestplate(), new Human(), new Palladin(), new Sword());

            // Act
            var clone = FighterFactory.Clone(original);

            // Assert
            Assert.Equal(original.Name, clone.Name);
            Assert.Equal(original.Armor, clone.Armor);
            Assert.Equal(original.Health, clone.Health);
            Assert.Equal(original.Strength, clone.Strength);
            Assert.Equal(original.Initiative, clone.Initiative);
        }
    }
}