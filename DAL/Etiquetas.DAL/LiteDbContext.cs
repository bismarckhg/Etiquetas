using System;
using Etiquetas.Core;
using Etiquetas.Core.Interfaces;
using LiteDB;

namespace Etiquetas.DAL
{
    /// <summary>
    /// Implementação do contexto do LiteDB.
    /// </summary>
    public class LiteDbContext : ILiteDbContext
    {
        /// <summary>
        /// Instância do LiteDB.
        /// </summary>
        private readonly LiteDatabase privLiteDB;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteDbContext"/> class.
        /// Inicializa uma nova instancia da classe <see cref="LiteDbContext"/>.
        /// </summary>
        /// <param name="connectionString">parametro de conexão string do Banco de dados do liteDB</param>
        public LiteDbContext(string connectionString)
        {
            privLiteDB = new LiteDatabase(connectionString);
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        public ILiteDatabase Database
        {
            get
            {
                return privLiteDB;
            }
        }

        /// <summary>
        /// Ensure the indices.
        /// </summary>
        public void EnsureIndices()
        {
            var etiquetaCol = privLiteDB.GetCollection<IEtiquetaImpressao>("EtiquetaImpressao");
            etiquetaCol.EnsureIndex(x => x.Id);
            etiquetaCol.EnsureIndex(x => x.StatusEtiqueta);
            etiquetaCol.EnsureIndex(x => x.Lote);
            etiquetaCol.EnsureIndex(x => x.CodigoMaterial);
            etiquetaCol.EnsureIndex(x => x.CodigoBarras);

            var faltaCol = privLiteDB.GetCollection<IFaltaImprimir>("FaltaImprimir");
            faltaCol.EnsureIndex(x => x.IdEtiquetaImpressao);
        }

        /// <summary>
        /// Disposes the LiteDB instance.
        /// </summary>
        public void Dispose()
        {
            try
            {
                privLiteDB?.Dispose();
            }
            catch
            {
            }
        }
    }
}
