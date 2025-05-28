// Tests/Fighters.GameEngine.Tests/GameManagerTests.cs
using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using System.Collections.Generic;
using Xunit;

namespace Fighters.GameEngine.Tests
{
    public class GameManagerTests
    {
        [Fact]
        public void StartSimulation_ShouldEndWithOneWinner()
        {
            // Arrange
            var fighters = new List<IFighter>
            {
                new Fighter("Fighter1", new Helmet(), new Human(), new Palladin(), new Sword()),
                new Fighter("Fighter2", new Chestplate(), new Elf(), new Wizard(), new Staff())
            };

            // Act
            var aliveFighters = new List<IFighter>(fighters);
            int round = 1;

            while (aliveFighters.Count > 1)
            {
                GameManager.GoOneRound(aliveFighters);
                round++;
            }

            // Assert
            Assert.Single(aliveFighters);
            Assert.True(aliveFighters[0].GetCurrentHealth() > 0);
        }

        [Fact]
        public void GoOneRound_ShouldReduceFightersHealth()
        {
            // Arrange
            var fighters = new List<IFighter>
            {
                new Fighter("Fighter1", new Helmet(), new Human(), new Palladin(), new Sword()),
                new Fighter("Fighter2", new Chestplate(), new Elf(), new Wizard(), new Staff())
            };
            var initialHealth1 = fighters[0].GetCurrentHealth();
            var initialHealth2 = fighters[1].GetCurrentHealth();

            // Act
            GameManager.GoOneRound(fighters);

            // Assert
            Assert.True(fighters[0].GetCurrentHealth() < initialHealth1 || 
                        fighters[1].GetCurrentHealth() < initialHealth2);
        }
    }
}