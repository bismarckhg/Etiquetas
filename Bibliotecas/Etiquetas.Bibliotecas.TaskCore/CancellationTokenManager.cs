using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TaskCore
{
    public class CancellationTokenManager : CancellationTokenSource, IDisposable
    {
        protected ConcurrentBag<CancellationTokenManager> LinkedCancellationSources;

        public CancellationTokenManager() : base()
        {
            this.Token.Register(() => CancelLinkedTokens().Wait());
            LinkedCancellationSources = new ConcurrentBag<CancellationTokenManager>();
        }

        public CancellationTokenManager(TimeSpan delay) : base(delay)
        {
            this.Token.Register(() => CancelLinkedTokens().Wait());
            LinkedCancellationSources = new ConcurrentBag<CancellationTokenManager>();
        }

        public CancellationTokenManager(int millisecondsDelay) : base(millisecondsDelay)
        {
            this.Token.Register(() => CancelLinkedTokens().Wait());
            LinkedCancellationSources = new ConcurrentBag<CancellationTokenManager>();
        }

        public async Task CreateLinkedTokensAsync(params CancellationTokenManager[] cancellationTokenSources)
        {
            // Adiciona os novos tokens à lista, se não estiverem cancelados
            await Task.Run(() =>
            {
                foreach (var cts in cancellationTokenSources)
                {
                    if (cts.IsCancellationRequested)
                    {
                        continue;
                    }

                    LinkedCancellationSources.Add(cts);
                }
            }).ConfigureAwait(false);


            //await Task.Run(() =>
            //{
            //    CheckAndCancelTokens();
            //    CleanCanceledTokens();
            //});
        }

        private async Task CancelLinkedTokens()
        {
            await Task.Run(() =>
            {
                if (LinkedCancellationSources == null)
                {
                    return;
                }

                foreach (var linkedCancellation in LinkedCancellationSources)
                {
                    if (!linkedCancellation.IsCancellationRequested)
                    {
                        linkedCancellation.Cancel();
                    }
                }
            }).ConfigureAwait(false);
        }

        private void CheckAndCancelTokens()
        {
            foreach (var linkedCancellation in LinkedCancellationSources)
            {
                if (linkedCancellation.IsCancellationRequested)
                {
                    linkedCancellation.Cancel();
                }
            }
        }

        private void CleanCanceledTokens()
        {
            // Cria uma nova coleção temporária e copia apenas tokens ativos
            var activeTokens = new ConcurrentBag<CancellationTokenManager>(
                LinkedCancellationSources.Where(cts => !cts.IsCancellationRequested)
            );

            // Substitui a bag original pela nova coleção de tokens ativos
            LinkedCancellationSources = activeTokens;
        }
    }

    //public class CancellationTokenManager : CancellationTokenSource, IDisposable
    //{
    //    protected ConcurrentBag<CancellationTokenManager> LinkedCancellationSources;

    //    public CancellationTokenManager() : base()
    //    {
    //        this.Token.Register(CancelLinkedTokens);
    //        LinkedCancellationSources = new ConcurrentBag<CancellationTokenManager>();
    //    }

    //    public CancellationTokenManager(TimeSpan delay) : base(delay)
    //    {
    //        this.Token.Register(CancelLinkedTokens);
    //        LinkedCancellationSources = new ConcurrentBag<CancellationTokenManager>();
    //    }

    //    public CancellationTokenManager(int millisecondsDelay) : base(millisecondsDelay)
    //    {
    //        this.Token.Register(CancelLinkedTokens);
    //        LinkedCancellationSources = new ConcurrentBag<CancellationTokenManager>();
    //    }

    //    public void CreateLinkedTokens(params CancellationTokenManager[] cancellationTokenSources)
    //    {
    //        foreach (var cts in cancellationTokenSources)
    //        {
    //            if (!cts.IsCancellationRequested)
    //            {
    //                this.LinkedCancellationSources.Add(cts);
    //            }
    //        }

    //        foreach (var linkedCancellation in this.LinkedCancellationSources)
    //        {
    //            if (linkedCancellation.IsCancellationRequested)
    //            {
    //            }
    //        }
    //    }

    //    private void CancelLinkedTokens()
    //    {
    //        foreach (var linkedCancellation in LinkedCancellationSources)
    //        {
    //            if (!linkedCancellation.IsCancellationRequested)
    //            {
    //                linkedCancellation.Cancel();
    //            }
    //        }
    //    }
    //}
}
