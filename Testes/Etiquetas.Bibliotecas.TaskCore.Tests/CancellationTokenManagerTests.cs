using System;
using System.Threading.Tasks;
using Xunit;
using Etiquetas.Bibliotecas.TaskCore;

namespace Etiqueta.Bibliotecas.TaskCore.Tests
{
    public class CancellationTokenManagerTests
    {
        [Fact]
        public async Task Cancel_WhenMainSourceIsCanceled_ShouldCancelLinkedSources()
        {
            // Arrange
            var mainManager = new CancellationTokenManager();
            var linkedManager1 = new CancellationTokenManager();
            var linkedManager2 = new CancellationTokenManager();

            await mainManager.CreateLinkedTokensAsync(linkedManager1, linkedManager2);

            // Act
            mainManager.Cancel();
            await Task.Delay(100); // Give time for the registered action to fire

            // Assert
            Assert.True(mainManager.IsCancellationRequested);
            Assert.True(linkedManager1.IsCancellationRequested);
            Assert.True(linkedManager2.IsCancellationRequested);
        }

        [Fact]
        public async Task CreateLinkedTokensAsync_ShouldNotAddAlreadyCanceledTokens()
        {
            // Arrange
            var mainManager = new CancellationTokenManager();
            var nonCanceledManager = new CancellationTokenManager();
            var canceledManager = new CancellationTokenManager();
            canceledManager.Cancel();

            // Act
            await mainManager.CreateLinkedTokensAsync(nonCanceledManager, canceledManager);
            mainManager.Cancel();
            await Task.Delay(100);

            // Assert
            Assert.True(nonCanceledManager.IsCancellationRequested); // Should be canceled
            // We can't directly check the contents of the ConcurrentBag,
            // but we can infer. If the bug was present, nonCanceledManager would not be linked and thus not canceled.
        }

        [Fact]
        public async Task TimedConstructor_ShouldCancelAfterDelay()
        {
            // Arrange
            var timedManager = new CancellationTokenManager(TimeSpan.FromMilliseconds(50));
            var linkedManager = new CancellationTokenManager();
            await timedManager.CreateLinkedTokensAsync(linkedManager);

            // Act
            await Task.Delay(150); // Wait for the timeout

            // Assert
            Assert.True(timedManager.IsCancellationRequested);
            Assert.True(linkedManager.IsCancellationRequested);
        }
    }
}
