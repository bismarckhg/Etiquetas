using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiteDB;

namespace Etiquetas.Core
{
    /// <summary>
    /// Interface para o contexto do LiteDB.
    /// </summary>
    public interface ILiteDbContext : IDisposable
    {
        /// <summary>
        /// Gets - Obtém a instância do banco de dados LiteDB.
        /// </summary>
        ILiteDatabase Database { get; }

        /// <summary>
        /// Garante que os índices necessários estejam criados no banco de dados.
        /// </summary>
        void EnsureIndices();
    }
}
