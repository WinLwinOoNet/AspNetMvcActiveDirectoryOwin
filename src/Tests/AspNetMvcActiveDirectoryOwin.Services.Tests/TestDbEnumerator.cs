using System.Collections;
using System.Collections.Generic;

namespace AspNetMvcActiveDirectoryOwin.Services.Tests
{
    public class TestDbEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestDbEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public T Current => _inner.Current;

        object IEnumerator.Current => _inner.Current;

        public void Dispose()
        {
            // Do not dispose the inner enumerator, since it might be enumerated again, reset it instead.
            _inner.Reset();
        }

        public bool MoveNext()
        {
            return _inner.MoveNext();
        }

        public void Reset()
        {
            _inner.Reset();
        }
    }
}
