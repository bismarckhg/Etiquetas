using Xunit;
using Etiquetas.Bibliotecas.Comum.Datas;
using System;

namespace Etiquetas.Bibliotecas.Comum.Tests.Datas
{
    public class ConverteDateTimeEmArrayStringTests
    {
        private readonly DateTime _testDate = new DateTime(2023, 10, 26, 14, 30, 45, 123);

        [Fact]
        public void ObtemAnoMesDiaArrayString_RetornaArrayCorreto()
        {
            // Arrange
            var expected = new[] { "2023", "10", "26" };

            // Act
            var result = ConverteDateTimeEmArrayString.ObtemAnoMesDiaArrayString(_testDate);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ObtemAnoMesDiaHoraMinutoArrayString_RetornaArrayCorreto()
        {
            // Arrange
            var expected = new[] { "2023", "10", "26", "14", "30" };

            // Act
            var result = ConverteDateTimeEmArrayString.ObtemAnoMesDiaHoraMinutoArrayString(_testDate);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ObtemAnoMesDiaHoraMinutoSegundoArrayString_RetornaArrayCorreto()
        {
            // Arrange
            var expected = new[] { "2023", "10", "26", "14", "30", "45" };

            // Act
            var result = ConverteDateTimeEmArrayString.ObtemAnoMesDiaHoraMinutoSegundoArrayString(_testDate);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ObtemAnoMesDiaHoraMinutoSegundoMilesimoArrayString_RetornaArrayCorreto()
        {
            // Arrange
            var expected = new[] { "2023", "10", "26", "14", "30", "45", "123" };

            // Act
            var result = ConverteDateTimeEmArrayString.ObtemAnoMesDiaHoraMinutoSegundoMilesimoArrayString(_testDate);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
