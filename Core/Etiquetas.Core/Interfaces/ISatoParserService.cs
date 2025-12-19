using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Etiqueta.Application.DTOs;

namespace Etiquetas.Core
{
    /// <summary>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
    /// Interface para o serviço de parser Sato.    
    /// </summary>
    public interface ISatoParserService
    {
        /// <summary>
        /// Faz o parser do pacote Sato e retorna uma lista de DTOs.
        /// </summary>
        /// <param name="package">Pacoye de leitura da Impressora Sato.</param>
        /// <returns>Lista de Pacotes formatados da impressora sato.</returns>
        System.Collections.Generic.List<ISatoDto> Parse(byte[] package);
    }
}
