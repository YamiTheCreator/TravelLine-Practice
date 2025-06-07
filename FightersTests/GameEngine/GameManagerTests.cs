using Fighters.GameEngine;
using Fighters.Models.Fighters;
using Moq;

namespace FightersTests.GameEngine
{
    public class GameManagerTests
    {
        [Fact]
        public void GoOneRound_WithTwoAliveFighters_AttackersAttackTargets()
        {
            // Arrange
            Mock<IFighter> fighter1 = CreateMockFighter( "Fighter1", 100, 20, 50 );
            Mock<IFighter> fighter2 = CreateMockFighter( "Fighter2", 80, 15, 30 );

            List<IFighter> fighters = [ fighter1.Object, fighter2.Object ];

            // Act
            GameManager.GoOneRound( fighters );

            // Assert
            fighter1.Verify( f => f.CalculateDamage(), Times.Once );
            fighter2.Verify( f => f.CalculateDamage(), Times.Once );
            fighter1.Verify( f => f.TakeDamage( It.IsAny<int>() ), Times.Once );
            fighter2.Verify( f => f.TakeDamage( It.IsAny<int>() ), Times.Once );
        }

        [Fact]
        public void GoOneRound_WithDeadFighter_DeadFighterDoesNotAttack()
        {
            // Arrange
            Mock<IFighter> aliveFighter = CreateMockFighter( "Alive", 50, 20, 60 );
            Mock<IFighter> deadFighter = CreateMockFighter( "Dead", 0, 15, 40 );

            List<IFighter> fighters = [ aliveFighter.Object, deadFighter.Object ];

            // Act
            GameManager.GoOneRound( fighters );

            // Assert
            aliveFighter.Verify( f => f.CalculateDamage(), Times.Once );
            deadFighter.Verify( f => f.CalculateDamage(), Times.Never );
        }

        [Fact]
        public void GoOneRound_WithOneFighter_NoAttacksOccur()
        {
            // Arrange
            Mock<IFighter> singleFighter = CreateMockFighter( "Alone", 100, 20, 50 );
            List<IFighter> fighters = [ singleFighter.Object ];

            // Act
            GameManager.GoOneRound( fighters );

            // Assert
            singleFighter.Verify( f => f.CalculateDamage(), Times.Never );
            singleFighter.Verify( f => f.TakeDamage( It.IsAny<int>() ), Times.Never );
        }

        [Fact]
        public void GoOneRound_WithEmptyList_NoExceptionThrown()
        {
            // Arrange
            List<IFighter> fighters = new List<IFighter>();

            // Act & Assert
            Exception? exception = Record.Exception( () => GameManager.GoOneRound( fighters ) );
            Assert.Null( exception );
        }

        [Fact]
        public void GoOneRound_WithMultipleFighters_EachFighterAttacksOnce()
        {
            // Arrange
            Mock<IFighter> fighter1 = CreateMockFighter( "Fighter1", 100, 20, 80 );
            Mock<IFighter> fighter2 = CreateMockFighter( "Fighter2", 90, 18, 70 );
            Mock<IFighter> fighter3 = CreateMockFighter( "Fighter3", 85, 22, 60 );

            List<IFighter> fighters = [ fighter1.Object, fighter2.Object, fighter3.Object ];

            // Act
            GameManager.GoOneRound( fighters );

            // Assert
            fighter1.Verify( f => f.CalculateDamage(), Times.Once );
            fighter2.Verify( f => f.CalculateDamage(), Times.Once );
            fighter3.Verify( f => f.CalculateDamage(), Times.Once );
        }

        [Theory]
        [InlineData( 1 )]
        [InlineData( 5 )]
        [InlineData( 50 )]
        public void GoOneRound_WithVariousDamageValues_AppliesDamageCorrectly( int damageValue )
        {
            // Arrange
            Mock<IFighter> attacker = CreateMockFighter( "Attacker", 100, 20, 50 );
            Mock<IFighter> target = CreateMockFighter( "Target", 100, 15, 30 );

            attacker.Setup( f => f.CalculateDamage() ).Returns( damageValue );

            List<IFighter> fighters = [ attacker.Object, target.Object ];

            // Act
            GameManager.GoOneRound( fighters );

            // Assert
            target.Verify( f => f.TakeDamage( damageValue ), Times.Once );
        }

        private static readonly string[] _expected = [ "HighInitiative", "MediumInitiative", "LowInitiative" ];

        [Fact]
        public void GoOneRound_FightersAttackInInitiativeOrder()
        {
            // Arrange
            Mock<IFighter> fighter1 = CreateMockFighter( "HighInitiative", 100, 20, 90 );
            Mock<IFighter> fighter2 = CreateMockFighter( "MediumInitiative", 100, 20, 60 );
            Mock<IFighter> fighter3 = CreateMockFighter( "LowInitiative", 100, 20, 30 );

            List<string> callOrder = [ ];

            fighter1.Setup( f => f.CalculateDamage() ).Callback( () => callOrder.Add( "HighInitiative" ) );
            fighter2.Setup( f => f.CalculateDamage() ).Callback( () => callOrder.Add( "MediumInitiative" ) );
            fighter3.Setup( f => f.CalculateDamage() ).Callback( () => callOrder.Add( "LowInitiative" ) );

            List<IFighter> fighters = [ fighter1.Object, fighter2.Object, fighter3.Object ];

            // Act
            GameManager.GoOneRound( fighters );

            // Assert
            Assert.Equal( _expected, callOrder );
        }

        [Fact]
        public void GoOneRound_WhenFighterDies_RemovesFromListBeforeNextAttack()
        {
            // Arrange
            Mock<IFighter> attacker = CreateMockFighter( "Attacker", 100, 20, 50 );
            Mock<IFighter> victim1 = CreateMockFighter( "Victim1", 1, 15, 40 );
            Mock<IFighter> victim2 = CreateMockFighter( "Victim2", 100, 15, 30 );

            victim1.Setup( v => v.GetCurrentHealth() ).Returns( 1 )
                .Callback( () => victim1.Setup( v => v.GetCurrentHealth() ).Returns( -10 ) );

            List<IFighter> fighters = [ attacker.Object, victim1.Object, victim2.Object ];

            // Act
            GameManager.GoOneRound( fighters );

            // Assert
            Assert.DoesNotContain( victim1.Object, fighters );
            Assert.Equal( 2, fighters.Count );
        }

        private static Mock<IFighter> CreateMockFighter( string name, int currentHealth, int strength, int initiative )
        {
            Mock<IFighter> mock = new();
            mock.Setup( f => f.Name ).Returns( name );
            mock.Setup( f => f.GetCurrentHealth() ).Returns( currentHealth );
            mock.Setup( f => f.Strength ).Returns( strength );
            mock.Setup( f => f.Initiative ).Returns( initiative );
            mock.Setup( f => f.CalculateDamage() ).Returns( 10 );
            mock.Setup( f => f.Armor ).Returns( 5 );
            mock.Setup( f => f.Health ).Returns( 100 );
            return mock;
        }
    }
}