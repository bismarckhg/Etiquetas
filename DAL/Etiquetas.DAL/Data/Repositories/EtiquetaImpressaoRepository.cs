using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Core;
using Etiquetas.Core.Interfaces;
using Etiquetas.Domain.Entities;
using LiteDB;

namespace Etiquetas.DAL.Data.Repositories
{
    /// <summary>
    /// Implementação do repositório de EtiquetaImpressao.
    /// </summary>
    public class EtiquetaImpressaoRepository : IEtiquetaImpressaoRepository
    {
        /// <summary>
        /// Instância do contexto do LiteDB.
        /// </summary>
        private readonly ILiteDbContext privLiteDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="EtiquetaImpressaoRepository"/> class.
        /// Inicializa uma nova instancia da classe <see cref="EtiquetaImpressaoRepository"/>.
        /// </summary>
        /// <param name="context">instancia do contexto do LiteDB.</param>
        public EtiquetaImpressaoRepository(ILiteDbContext context)
        {
            privLiteDbContext = context;
        }

        /// <inheritdoc/>
        public Task InsertAsync(IEtiquetaImpressao etiqueta)
        {
            return Task.Run(() =>
            {
                var col = privLiteDbContext.Database.GetCollection<IEtiquetaImpressao>("EtiquetaImpressao");
                col.Insert(etiqueta);
            });
        }

        /// <inheritdoc/>
        public Task UpdateAsync(IEtiquetaImpressao etiqueta)
        {
            return Task.Run(() =>
            {
                var col = privLiteDbContext.Database.GetCollection<IEtiquetaImpressao>("EtiquetaImpressao");
                col.Update(etiqueta);
            });
        }

        /// <inheritdoc/>
        public Task DeleteAsync(long id)
        {
            return Task.Run(() =>
            {
                var col = privLiteDbContext.Database.GetCollection<IEtiquetaImpressao>("EtiquetaImpressao");
                col.Delete(id);
            });
        }

        /// <inheritdoc/>
        public Task<IEtiquetaImpressao> GetByIdAsync(long id)
        {
            return Task.Run(() =>
            {
                var col = privLiteDbContext.Database.GetCollection<IEtiquetaImpressao>("EtiquetaImpressao");
                return col.FindById(id);
            });
        }

        /// <inheritdoc/>
        public Task<IEnumerable<IEtiquetaImpressao>> GetByPeriodAsync(DateTime start, DateTime end)
        {
            return Task.Run(() =>
            {
                var col = privLiteDbContext.Database.GetCollection<IEtiquetaImpressao>("EtiquetaImpressao");
                return col.Query().Where(x => x.DataHoraInicio >= start && x.DataHoraFim <= end).ToEnumerable();
            });
        }

        /// <inheritdoc/>
        public Task<IEnumerable<IEtiquetaImpressao>> GetAllAsync()
        {
            return Task.Run(() =>
            {
                var col = privLiteDbContext.Database.GetCollection<IEtiquetaImpressao>("EtiquetaImpressao");
                return col.FindAll();
            });
        }

        /// <inheritdoc/>
        public Task<IEtiquetaImpressao> FindByJobNameAsync(string jobName)
        {
            return Task.Run(() =>
            {
                var col = privLiteDbContext.Database.GetCollection<IEtiquetaImpressao>("EtiquetaImpressao");
                var found = col.Query().Where(x => x.CodigoBarras == jobName || x.Lote == jobName || (x.DescricaoMedicamento ?? string.Empty).Contains(jobName)).FirstOrDefault();
                return found;
            });
        }

        /// <inheritdoc/>
        public Task<IEtiquetaImpressao> FindByCodigoBarrasAsync(string codigoBarras)
        {
            return Task.Run(() =>
            {
                var col = privLiteDbContext.Database.GetCollection<IEtiquetaImpressao>("EtiquetaImpressao");
                return col.FindOne(x => x.CodigoBarras == codigoBarras);
            });
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}
