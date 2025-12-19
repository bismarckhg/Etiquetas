using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Core;
using Etiquetas.Core.Interfaces;
using LiteDB;

namespace Etiquetas.DAL.Data.Repositories
{
    /// <summary>
    /// Implementação do repositório de FaltaImprimir.
    /// </summary>
    public class FaltaImprimirRepository : IFaltaImprimirRepository
    {
        /// <summary>
        /// Instância do contexto do LiteDB.
        /// </summary>
        private readonly ILiteDbContext privLiteDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="FaltaImprimirRepository"/> class.
        /// Inicializa uma nova instancia da classe <see cref="FaltaImprimirRepository"/>.
        /// </summary>
        /// <param name="context">parametro com instancia do contexto do LiteDB.</param>
        public FaltaImprimirRepository(ILiteDbContext context)
        {
            privLiteDbContext = context;
        }

        /// <inheritdoc/>
        public Task<IEnumerable<IFaltaImprimir>> GetByEtiquetaImpressaoIdAsync(string idEtiqueta)
        {
            return Task.Run(() => {
                var col = privLiteDbContext.Database.GetCollection<IFaltaImprimir>("FaltaImprimir");
                return col.Query().Where(x => x.IdEtiquetaImpressao == idEtiqueta).ToEnumerable(); });
        }

        /// <inheritdoc/>
        public Task InsertAsync(IFaltaImprimir faltaImprimir)
        {
            return Task.Run(() =>
            {
                var col = privLiteDbContext.Database.GetCollection<IFaltaImprimir>("FaltaImprimir");
                col.Insert(faltaImprimir);
            });
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public Task<IFaltaImprimir> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
