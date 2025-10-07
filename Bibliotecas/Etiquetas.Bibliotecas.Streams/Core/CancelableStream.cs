using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Streams.Core
{
    public sealed class CancelableStream : Stream
    {
        private readonly Stream _inner;
        private readonly CancellationToken _ct;
        private readonly bool _leaveOpen;

        public CancelableStream(Stream inner, CancellationToken ct, bool leaveOpen = true)
        { _inner = inner ?? throw new ArgumentNullException(nameof(inner)); _ct = ct; _leaveOpen = leaveOpen; }

        public override bool CanRead => _inner.CanRead;
        public override bool CanSeek => _inner.CanSeek;
        public override bool CanWrite => false;
        public override long Length => _inner.Length;
        public override long Position { get => _inner.Position; set { _ct.ThrowIfCancellationRequested(); _inner.Position = value; } }
        public override void Flush() => _inner.Flush();
        public override int Read(byte[] buffer, int offset, int count) { _ct.ThrowIfCancellationRequested(); return _inner.Read(buffer, offset, count); }
        public override int ReadByte() { _ct.ThrowIfCancellationRequested(); return _inner.ReadByte(); }
        public override Task<int> ReadAsync(byte[] b, int o, int c, CancellationToken t)
        { _ct.ThrowIfCancellationRequested(); t.ThrowIfCancellationRequested(); using (var l = CancellationTokenSource.CreateLinkedTokenSource(_ct, t)) return _inner.ReadAsync(b, o, c, l.Token); }
        public override long Seek(long offset, SeekOrigin origin) { _ct.ThrowIfCancellationRequested(); return _inner.Seek(offset, origin); }
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        protected override void Dispose(bool disposing) { if (disposing && !_leaveOpen) _inner.Dispose(); base.Dispose(disposing); }
    }
}
