using Fighters.GameEngine;
using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Moq;

namespace FightersTests.GameEngine
{
    public class FighterFactoryTests
    {
        private readonly Mock<IArmor> _mockArmor;
        private readonly Mock<IRace> _mockRace;
        private readonly Mock<IClass> _mockClass;
        private readonly Mock<IWeapon> _mockWeapon;

        public FighterFactoryTests()
        {
            _mockArmor = new Mock<IArmor>();
            _mockRace = new Mock<IRace>();
            _mockClass = new Mock<IClass>();
            _mockWeapon = new Mock<IWeapon>();
        }

        [Fact]
        public void Create_WithValidParameters_ReturnsFighterWithCorrectProperties()
        {
            // Arrange
            const string? name = "TestHero";
            SetupMocks();

            // Act
            IFighter fighter = FighterFactory.Create(name, _mockArmor.Object, _mockClass.Object, _mockRace.Object,
                _mockWeapon.Object);

            // Assert
            Assert.NotNull(fighter);
            Assert.Equal(name, fighter.Name);
            Assert.IsAssignableFrom<IFighter>(fighter);
        }

        [Theory]
        [InlineData("Hero1")]
        [InlineData("")]
        [InlineData("Very Long Fighter Name With Special Characters!@#")]
        public void Create_WithDifferentNames_CreatesCorrectFighter(string? name)
        {
            // Arrange
            SetupMocks();

            // Act
            IFighter fighter = FighterFactory.Create(name, _mockArmor.Object, _mockClass.Object, _mockRace.Object,
                _mockWeapon.Object);

            // Assert
            Assert.Equal(name, fighter.Name);
        }

        [Fact]
        public void Create_WithNullName_CreatesFighterWithNullName()
        {
            // Arrange
            SetupMocks();

            // Act
            IFighter fighter = FighterFactory.Create(null, _mockArmor.Object, _mockClass.Object, _mockRace.Object,
                _mockWeapon.Object);

            // Assert
            Assert.Null(fighter.Name);
        }

        [Fact]
        public void Clone_WithValidFighter_ReturnsCloneWithSameProperties()
        {
            // Arrange
            IFighter originalFighter = CreateTestFighter();

            // Act
            IFighter clonedFighter = FighterFactory.Clone(originalFighter);

            // Assert
            Assert.NotNull(clonedFighter);
            Assert.NotSame(originalFighter, clonedFighter);
            Assert.Equal(originalFighter.Name, clonedFighter.Name);
            Assert.Equal(originalFighter.Armor, clonedFighter.Armor);
            Assert.Equal(originalFighter.Health, clonedFighter.Health);
            Assert.Equal(originalFighter.Strength, clonedFighter.Strength);
            Assert.Equal(originalFighter.Initiative, clonedFighter.Initiative);
        }

        [Fact]
        public void Clone_WithDamagedFighter_ReturnsCloneWithFullHealth()
        {
            // Arrange
            IFighter originalFighter = CreateTestFighter();
            originalFighter.TakeDamage(50);

            // Act
            IFighter clonedFighter = FighterFactory.Clone(originalFighter);

            // Assert
            Assert.Equal(originalFighter.Health, clonedFighter.GetCurrentHealth());
            Assert.NotEqual(originalFighter.GetCurrentHealth(), clonedFighter.GetCurrentHealth());
        }

        [Fact]
        public void Clone_IndependentHealthManagement_ClonesAreIndependent()
        {
            // Arrange
            IFighter originalFighter = CreateTestFighter();
            IFighter clonedFighter = FighterFactory.Clone(originalFighter);

            // Act
            originalFighter.TakeDamage(30);
            clonedFighter.TakeDamage(20);

            // Assert
            Assert.NotEqual(originalFighter.GetCurrentHealth(), clonedFighter.GetCurrentHealth());
        }

        [Fact]
        public void Create_WithMinimalValues_CreatesValidFighter()
        {
            // Arrange
            _mockArmor.Setup(a => a.Armor).Returns(0);
            _mockRace.Setup(r => r.Armor).Returns(0);
            _mockRace.Setup(r => r.Health).Returns(1);
            _mockRace.Setup(r => r.Strength).Returns(0);
            _mockClass.Setup(c => c.Health).Returns(0);
            _mockClass.Setup(c => c.Strength).Returns(0);
            _mockWeapon.Setup(w => w.Strength).Returns(0);

            // Act
            IFighter fighter = FighterFactory.Create("MinFighter", _mockArmor.Object, _mockClass.Object,
                _mockRace.Object, _mockWeapon.Object);

            // Assert
            Assert.Equal(0, fighter.Armor);
            Assert.Equal(1, fighter.Health);
            Assert.Equal(0, fighter.Strength);
        }

        [Fact]
        public void Create_MultipleTimes_InitiativeIsDifferent()
        {
            // Arrange
            SetupMocks();
            List<IFighter> fighters = [];

            // Act
            for (int i = 0; i < 10; i++)
            {
                fighters.Add(FighterFactory.Create($"Fighter{i}", _mockArmor.Object, _mockClass.Object,
                    _mockRace.Object, _mockWeapon.Object));
            }

            // Assert
            Assert.True(fighters.Select(f => f.Initiative).Distinct().Count() > 1);
        }

        private IFighter CreateTestFighter()
        {
            SetupMocks();
            return FighterFactory.Create("TestFighter", _mockArmor.Object, _mockClass.Object, _mockRace.Object,
                _mockWeapon.Object);
        }

        private void SetupMocks()
        {
            _mockArmor.Setup(a => a.Armor).Returns(5);
            _mockRace.Setup(r => r.Armor).Returns(2);
            _mockRace.Setup(r => r.Health).Returns(100);
            _mockRace.Setup(r => r.Strength).Returns(15);
            _mockClass.Setup(c => c.Health).Returns(50);
            _mockClass.Setup(c => c.Strength).Returns(10);
            _mockWeapon.Setup(w => w.Strength).Returns(20);
        }
    }
}