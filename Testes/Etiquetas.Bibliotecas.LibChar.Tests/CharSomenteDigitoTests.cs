using Xunit;
using Etiquetas.Bibliotecas.LibChar;

namespace Etiquetas.Bibliotecas.LibChar.Tests
{
    public class CharSomenteDigitoTests
    {
        [Theory]
        [InlineData('1', true)]
        [InlineData('9', true)]
        [InlineData('a', false)]
        [InlineData(' ', false)]
        [InlineData('$', false)]
        public void Execute_ShouldCorrectlyIdentifyDigits(char input, bool expected)
        {
            // Act
            bool result = CharSomenteDigito.Execute(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData('1', true)]
        [InlineData('a', false)]
        [InlineData(null, false)]
        public void Execute_Nullable_ShouldCorrectlyIdentifyDigits(char? input, bool expected)
        {
            // Act
            bool result = CharSomenteDigito.Execute(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
