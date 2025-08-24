using Xunit;
using Etiquetas.Bibliotecas.Comum.Datas;
using System;
using System.Globalization;
using System.Threading;

namespace Etiquetas.Bibliotecas.Comum.Tests.Datas
{
    public class ConverteDateTimeEmStringTests
    {
        private readonly DateTime _testDate = new DateTime(2023, 10, 26, 14, 30, 45, 123);
        private readonly CultureInfo _testCulture = new CultureInfo("en-US");

        public ConverteDateTimeEmStringTests()
        {
            // Garantir que a cultura padrão não interfira nos testes que não a especificam
            Thread.CurrentThread.CurrentCulture = _testCulture;
            Thread.CurrentThread.CurrentUICulture = _testCulture;
        }

        [Fact]
        public void DiaMesAnoFormatoCurtoLocal_RetornaFormatoCorreto()
        {
            // Act
            var result = ConverteDateTimeEmString.DiaMesAnoFormatoCurtoLocal(_testDate, _testCulture);

            // Assert
            Assert.Equal("10/26/2023", result);
        }

        [Fact]
        public void HoraMinutoSegundoLocal_RetornaFormatoCorreto()
        {
            // Act
            var result = ConverteDateTimeEmString.HoraMinutoSegundoLocal(_testDate, _testCulture);

            // Assert
            Assert.Equal("2:30:45 PM".Replace(" ", ""), result.Replace(" ", ""));
        }

        [Fact]
        public void ObtemAnoEmString_RetornaAno()
        {
            // Act
            var result = ConverteDateTimeEmString.ObtemAnoEmString(_testDate);

            // Assert
            Assert.Equal("2023", result);
        }

        [Fact]
        public void ObtemMesEmString_RetornaMes()
        {
            // Act
            var result = ConverteDateTimeEmString.ObtemMesEmString(_testDate);

            // Assert
            Assert.Equal("10", result);
        }

        [Fact]
        public void ObtemDiaEmString_RetornaDia()
        {
            // Act
            var result = ConverteDateTimeEmString.ObtemDiaEmString(_testDate);

            // Assert
            Assert.Equal("26", result);
        }

        [Fact]
        public void ObtemAnoMesDiaString_RetornaStringConcatenada()
        {
            // Act
            var result = ConverteDateTimeEmString.ObtemAnoMesDiaString(_testDate);

            // Assert
            Assert.Equal("20231026", result);
        }
    }
}
