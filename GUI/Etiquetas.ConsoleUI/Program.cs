using System;
using System.Threading.Tasks;
using Etiqueta.Application.Mappers;
using Etiquetas.Application.Services;
using Etiquetas.DAL;
using Etiquetas.DAL.Data.Repositories;

namespace Etiquetas.ConsoleUI
{
    /// <summary>
    /// Ponto de entrada da aplicação console.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Ponto de entrada principal da aplicação.
        /// </summary>
        /// <param name="args">args de complemento de entrada principal.</param>
        /// <returns>Retorna uma tarefa.</returns>
        public static async Task Main(string[] args)
        {
            // await TesteEtiquetaPipeline.Teste();

            TesteEtiqueta.ProcessarEtiquetaZPL();

            Console.WriteLine("Teste concluído. Pressione qualquer tecla para sair.");
            Console.ReadKey();
        }
    }
}
