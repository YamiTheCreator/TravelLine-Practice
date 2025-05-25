using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Models.Tests.Fighters
{
    public class FighterTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var name = "TestFighter";
            var armor = new Chestplate();
            var race = new Human();
            var fighterClass = new Palladin();
            var weapon = new Sword();

            // Act
            var fighter = new Fighter(name, armor, race, fighterClass, weapon);

            // Assert
            Assert.Equal(name, fighter.Name);
            Assert.Equal(armor.Armor + race.Armor, fighter.Armor);
            Assert.Equal(race.Health + fighterClass.Health, fighter.Health);
            Assert.Equal(race.Strength + fighterClass.Strength + weapon.Strength, fighter.Strength);
            Assert.InRange(fighter.Initiative, 1, 100);
            Assert.Equal(fighter.Health, fighter.GetCurrentHealth());
        }

        [Fact]
        public void TakeDamage_ShouldReduceCurrentHealth()
        {
            // Arrange
            var fighter = new Fighter("Test", new Chestplate(), new Human(), new Palladin(), new Sword());
            int initialHealth = fighter.GetCurrentHealth();
            int damage = 10;

            // Act
            fighter.TakeDamage(damage);

            // Assert
            Assert.Equal(initialHealth - damage, fighter.GetCurrentHealth());
        }

        [Fact]
        public void CalculateDamage_ShouldReturnPositiveValue()
        {
            // Arrange
            var fighter = new Fighter("Test", new Chestplate(), new Human(), new Palladin(), new Sword());

            // Act
            int damage = fighter.CalculateDamage();

            // Assert
            Assert.True(damage > 0);
        }

        [Fact]
        public void CloneConstructor_ShouldCreateIdenticalFighter()
        {
            // Arrange
            var original = new Fighter("Original", new Chestplate(), new Human(), new Palladin(), new Sword());

            // Act
            var clone = new Fighter(original.Name, original.Armor, original.Health, original.Strength, original.Initiative);

            // Assert
            Assert.Equal(original.Name, clone.Name);
            Assert.Equal(original.Armor, clone.Armor);
            Assert.Equal(original.Health, clone.Health);
            Assert.Equal(original.Strength, clone.Strength);
            Assert.Equal(original.Initiative, clone.Initiative);
            Assert.Equal(original.GetCurrentHealth(), clone.GetCurrentHealth());
        }
    }
}