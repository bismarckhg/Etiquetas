using System;
using System.Threading.Tasks;
using Xunit;
using Etiquetas.Bibliotecas.Armazem.Core;

namespace Etiquetas.Bibliotecas.Armazem.Tests
{
    public class AsyncStackTests
    {
        [Fact]
        public async Task EnqueueAsync_And_DequeueAsync_ShouldWorkInOrder()
        {
            // Arrange
            var asyncStack = new AsyncStack<int>();

            // Act
            await asyncStack.EnqueueAsync(1);
            await asyncStack.EnqueueAsync(2);
            var item1 = await asyncStack.DequeueAsync();
            var item2 = await asyncStack.DequeueAsync();

            // Assert
            Assert.Equal(1, item1);
            Assert.Equal(2, item2);
            Assert.Equal(0, asyncStack.Count);
        }

        [Fact]
        public async Task DequeueAsync_ShouldBlock_WhenQueueIsEmpty()
        {
            // Arrange
            var asyncStack = new AsyncStack<string>();
            var dequeueTask = asyncStack.DequeueAsync();

            // Act
            // Give a moment to ensure DequeueAsync is waiting
            await Task.Delay(50);
            Assert.False(dequeueTask.IsCompleted);

            await asyncStack.EnqueueAsync("test");
            var result = await dequeueTask; // Now it should complete

            // Assert
            Assert.Equal("test", result);
        }

        [Fact]
        public void TryDequeue_ShouldReturnFalse_WhenQueueIsEmpty()
        {
            // Arrange
            var asyncStack = new AsyncStack<int>();

            // Act
            var result = asyncStack.TryDequeue(out var item);

            // Assert
            Assert.False(result);
            Assert.Equal(default(int), item);
        }

        [Fact]
        public async Task TryDequeue_ShouldReturnTrue_WhenQueueHasItems()
        {
            // Arrange
            var asyncStack = new AsyncStack<int>();
            await asyncStack.EnqueueAsync(10);

            // Act
            var result = asyncStack.TryDequeue(out var item);

            // Assert
            Assert.True(result);
            Assert.Equal(10, item);
        }

        [Fact]
        public async Task ClearAsync_ShouldRemoveAllItems()
        {
            // Arrange
            var asyncStack = new AsyncStack<int>();
            await asyncStack.EnqueueAsync(1);
            await asyncStack.EnqueueAsync(2);

            // Act
            await asyncStack.ClearAsync();

            // Assert
            Assert.Equal(0, asyncStack.Count);
            Assert.False(asyncStack.HasItems);
        }
    }
}
