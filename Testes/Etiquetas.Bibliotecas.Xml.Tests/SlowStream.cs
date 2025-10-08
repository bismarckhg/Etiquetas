using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Xml.Tests
{
    /// <summary>
    /// Um Decorator de Stream que introduz um atraso artificial em cada operação de leitura,
    /// simulando um stream lento (rede, disco) para permitir o teste de cancelamento.
    /// </summary>
    public class SlowStream : Stream
    {
        private readonly Stream _inner;
        private readonly int _delayMs;

        public SlowStream(Stream inner, int delayMs = 5) // Atraso padrão de 5ms
        {
            _inner = inner;
            _delayMs = delayMs;
        }

        public override bool CanRead => _inner.CanRead;
        public override bool CanSeek => _inner.CanSeek;
        public override bool CanWrite => _inner.CanWrite;
        public override long Length => _inner.Length;
        public override long Position { get => _inner.Position; set => _inner.Position = value; }
        public override void Flush() => _inner.Flush();
        public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);
        public override void SetLength(long value) => _inner.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => _inner.Write(buffer, offset, count);

        public override int Read(byte[] buffer, int offset, int count)
        {
            Thread.Sleep(_delayMs); // Atraso síncrono
            return _inner.Read(buffer, offset, count);
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            await Task.Delay(_delayMs, cancellationToken); // Atraso assíncrono
            return await _inner.ReadAsync(buffer, offset, count, cancellationToken);
        }
    }
}
