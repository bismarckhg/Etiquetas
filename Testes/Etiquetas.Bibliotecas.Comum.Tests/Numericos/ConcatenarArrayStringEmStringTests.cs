using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiqueta.Bibliotecas.LibArrayString.Tests
{
    public class ConcatenarArrayStringEmStringTests
    {
        [Fact]
        public void Execute_WithValidArray_ShouldConcatenateElements()
        {
            // Arrange
            var inputArray = new string[] { "a", "b", "c" };
            var expected = "abc";

            // Act
            var result = ConcatenarArrayStringEmString.Execute(inputArray);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_WithEmptyArray_ShouldReturnEmptyString()
        {
            // Arrange
            var inputArray = new string[] { };
            var expected = "";

            // Act
            var result = ConcatenarArrayStringEmString.Execute(inputArray);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_WithNullArray_ShouldReturnEmptyString()
        {
            // Arrange
            string[] inputArray = null;
            var expected = "";

            // Act
            var result = ConcatenarArrayStringEmString.Execute(inputArray);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_WithArrayContainingNullsAndEmpty_ShouldConcatenateAll()
        {
            // Arrange
            var inputArray = new string[] { "a", null, "b", "", "c" };
            var expected = "abc"; // string.Concat ignores nulls

            // Act
            var result = ConcatenarArrayStringEmString.Execute(inputArray);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
