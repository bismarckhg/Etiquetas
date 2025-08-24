using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ConcatenarVariosArraysStringEmStringTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Execute_ComVariosArrays_RetornaConcatenacao(bool useLinq)
        {
            // Arrange
            var array1 = new[] { "a", "b" };
            var array2 = new[] { "c", "d" };
            var expected = "abcd";

            // Act
            var result = useLinq ? ConcatenarVariosArraysStringEmString.ExecuteLinq(array1, array2) : ConcatenarVariosArraysStringEmString.ExecuteStringBuilder(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Execute_ComParametroNulo_RetornaStringVazia(bool useLinq)
        {
            // Arrange & Act
            var result = useLinq ? ConcatenarVariosArraysStringEmString.ExecuteLinq(null) : ConcatenarVariosArraysStringEmString.ExecuteStringBuilder(null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Execute_ComArrayInternoNulo_IgnoraArrayNulo(bool useLinq)
        {
            // Arrange
            var array1 = new[] { "a", "b" };
            string[] array2 = null;
            var array3 = new[] { "c", "d" };
            var expected = "abcd";

            // Act
            var result = useLinq ? ConcatenarVariosArraysStringEmString.ExecuteLinq(array1, array2, array3) : ConcatenarVariosArraysStringEmString.ExecuteStringBuilder(array1, array2, array3);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Execute_ComElementosNulosEVazios_PreservaElementos(bool useLinq)
        {
            // Arrange
            var array1 = new[] { "a", null };
            var array2 = new[] { "", "d" };
            var expected = "ad"; // Concat trata null como ""

            // Act
            var result = useLinq ? ConcatenarVariosArraysStringEmString.ExecuteLinq(array1, array2) : ConcatenarVariosArraysStringEmString.ExecuteStringBuilder(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
