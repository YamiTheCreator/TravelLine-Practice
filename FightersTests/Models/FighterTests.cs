using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Moq;

namespace FightersTests.Models
{
    public class FighterTests
    {
        private readonly Mock<IArmor> _mockArmor;
        private readonly Mock<IRace> _mockRace;
        private readonly Mock<IClass> _mockClass;
        private readonly Mock<IWeapon> _mockWeapon;

        public FighterTests()
        {
            _mockArmor = new Mock<IArmor>();
            _mockRace = new Mock<IRace>();
            _mockClass = new Mock<IClass>();
            _mockWeapon = new Mock<IWeapon>();
        }

        [Fact]
        public void Constructor_WithValidParameters_CreatesCorrectFighter()
        {
            // Arrange
            const string? name = "TestFighter";
            const int armorValue = 5;
            const int raceArmor = 2;
            const int raceHealth = 100;
            const int raceStrength = 10;
            const int classHealth = 50;
            const int classStrength = 15;
            const int weaponStrength = 20;

            SetupMocks( armorValue, raceArmor, raceHealth, raceStrength, classHealth, classStrength, weaponStrength );

            // Act
            Fighter fighter = new( name, _mockArmor.Object, _mockRace.Object, _mockClass.Object, _mockWeapon.Object );

            // Assert
            Assert.Equal( name, fighter.Name );
            Assert.Equal( armorValue + raceArmor, fighter.Armor );
            Assert.Equal( raceHealth + classHealth, fighter.Health );
            Assert.Equal( raceStrength + classStrength + weaponStrength, fighter.Strength );
            Assert.Equal( fighter.Health, fighter.GetCurrentHealth() );
            Assert.True( fighter.Initiative is >= 1 and <= 99 );
        }

        [Fact]
        public void Constructor_WithDirectParameters_CreatesCorrectFighter()
        {
            // Arrange
            const string? name = "DirectFighter";
            const int armor = 10;
            const int health = 150;
            const int strength = 25;
            const int initiative = 50;

            // Act
            Fighter fighter = new( name, armor, health, strength, initiative );

            // Assert
            Assert.Equal( name, fighter.Name );
            Assert.Equal( armor, fighter.Armor );
            Assert.Equal( health, fighter.Health );
            Assert.Equal( strength, fighter.Strength );
            Assert.Equal( initiative, fighter.Initiative );
            Assert.Equal( health, fighter.GetCurrentHealth() );
        }

        [Theory]
        [InlineData( "" )]
        [InlineData( null )]
        public void Constructor_WithInvalidName_AcceptsEmptyOrNullName( string? name )
        {
            // Arrange
            SetupMocks( 5, 2, 100, 10, 50, 15, 20 );

            // Act & Assert
            Fighter fighter = new Fighter( name, _mockArmor.Object, _mockRace.Object, _mockClass.Object,
                _mockWeapon.Object );
            Assert.Equal( name, fighter.Name );
        }

        [Fact]
        public void TakeDamage_WithPositiveDamage_ReducesCurrentHealth()
        {
            // Arrange
            Fighter fighter = CreateTestFighter();
            const int damage = 30;
            int initialHealth = fighter.GetCurrentHealth();

            // Act
            fighter.TakeDamage( damage );

            // Assert
            Assert.Equal( initialHealth - damage, fighter.GetCurrentHealth() );
        }

        [Fact]
        public void TakeDamage_WithZeroDamage_DoesNotChangeHealth()
        {
            // Arrange
            Fighter fighter = CreateTestFighter();
            int initialHealth = fighter.GetCurrentHealth();

            // Act
            fighter.TakeDamage( 0 );

            // Assert
            Assert.Equal( initialHealth, fighter.GetCurrentHealth() );
        }

        [Fact]
        public void TakeDamage_WithNegativeDamage_IncreasesCurrentHealth()
        {
            // Arrange
            Fighter fighter = CreateTestFighter();
            const int negativeDamage = -20;
            int initialHealth = fighter.GetCurrentHealth();

            // Act
            fighter.TakeDamage( negativeDamage );

            // Assert
            Assert.Equal( initialHealth - negativeDamage, fighter.GetCurrentHealth() );
        }

        [Fact]
        public void TakeDamage_ReducingHealthBelowZero_AllowsNegativeHealth()
        {
            // Arrange
            Fighter fighter = CreateTestFighter();
            int massiveDamage = fighter.GetCurrentHealth() + 50;

            // Act
            fighter.TakeDamage( massiveDamage );

            // Assert
            Assert.True( fighter.GetCurrentHealth() < 0 );
        }

        [Fact]
        public void CalculateDamage_WithHighStrengthLowArmor_ReturnsPositiveDamage()
        {
            // Arrange
            Fighter fighter = new( "Strong", 0, 100, 50, 50 );

            // Act
            int damage = fighter.CalculateDamage();

            // Assert
            Assert.True( damage >= 1 );
        }

        [Fact]
        public void CalculateDamage_WithLowStrengthHighArmor_ReturnsMinimumDamage()
        {
            // Arrange
            Fighter fighter = new( "Weak", 100, 100, 5, 50 );

            // Act
            int damage = fighter.CalculateDamage();

            // Assert
            Assert.Equal( 1, damage );
        }

        [Fact]
        public void CalculateDamage_MultipleCalls_ReturnsDifferentValues()
        {
            // Arrange
            Fighter fighter = CreateTestFighter();
            List<int> damages = [ ];

            // Act
            for ( int i = 0; i < 100; i++ )
            {
                damages.Add( fighter.CalculateDamage() );
            }

            // Assert
            Assert.True( damages.Distinct().Count() > 1, "Damage calculation should have some randomness" );
            Assert.All( damages, damage => Assert.True( damage >= 1 ) );
        }

        [Fact]
        public void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            Fighter fighter = new( "TestName", 10, 100, 25, 50 );

            // Act
            string result = fighter.ToString();

            // Assert
            Assert.Contains( "Fighter Configuration:", result );
            Assert.Contains( "Name: TestName", result );
            Assert.Contains( "Armor: 10", result );
            Assert.Contains( "Health: 100", result );
            Assert.Contains( "Strength: 25", result );
            Assert.Contains( "Initiative: 50", result );
        }

        [Fact]
        public void GetCurrentHealth_AfterMultipleDamageOperations_ReturnsCorrectValue()
        {
            // Arrange
            Fighter fighter = CreateTestFighter();
            int initialHealth = fighter.GetCurrentHealth();

            // Act
            fighter.TakeDamage( 20 );
            fighter.TakeDamage( 15 );
            fighter.TakeDamage( -5 );

            // Assert
            Assert.Equal( initialHealth - 20 - 15 + 5, fighter.GetCurrentHealth() );
        }

        [Fact]
        public void CalculateDamage_CriticalHit_Deals50PercentMoreDamage()
        {
            // Arrange
            Fighter fighter = new Fighter( "CritTest", 0, 100, 100, 50 );
            bool criticalOccurred = false;
            const int attempts = 1000;

            // Act
            for ( int i = 0; i < attempts; i++ )
            {
                int damage = fighter.CalculateDamage();
                if ( damage <= 110 )
                {
                    continue;
                }

                criticalOccurred = true;
                Assert.InRange( damage, 120, 165 );
            }

            // Assert
            Assert.True( criticalOccurred, $"Critical hit should occur in {attempts} attempts" );
        }

        [Fact]
        public void CalculateDamage_WhenStrengthLessThanArmor_ReturnsMinimumDamage()
        {
            // Arrange
            Fighter fighter = new Fighter( "Weak", 100, 100, 5, 50 );

            // Act & Assert
            for ( int i = 0; i < 100; i++ )
            {
                Assert.Equal( 1, fighter.CalculateDamage() );
            }
        }

        private Fighter CreateTestFighter()
        {
            SetupMocks( 5, 2, 100, 15, 50, 10, 20 );
            return new Fighter( "TestFighter", _mockArmor.Object, _mockRace.Object, _mockClass.Object,
                _mockWeapon.Object );
        }

        private void SetupMocks( int armorValue, int raceArmor, int raceHealth, int raceStrength,
            int classHealth, int classStrength, int weaponStrength )
        {
            _mockArmor.Setup( a => a.Armor ).Returns( armorValue );
            _mockRace.Setup( r => r.Armor ).Returns( raceArmor );
            _mockRace.Setup( r => r.Health ).Returns( raceHealth );
            _mockRace.Setup( r => r.Strength ).Returns( raceStrength );
            _mockClass.Setup( c => c.Health ).Returns( classHealth );
            _mockClass.Setup( c => c.Strength ).Returns( classStrength );
            _mockWeapon.Setup( w => w.Strength ).Returns( weaponStrength );
        }
    }
}