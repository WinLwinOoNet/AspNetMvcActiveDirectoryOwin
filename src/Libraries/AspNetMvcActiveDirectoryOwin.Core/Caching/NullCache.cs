namespace AspNetMvcActiveDirectoryOwin.Core.Caching
{
    /// <summary>
    /// Caches nothing
    /// </summary>
    public partial class NullCache : ICacheService
    {
        public virtual T Get<T>(string key)
        {
            return default(T);
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
        }

        public bool IsSet(string key)
        {
            return false;
        }

        public virtual void Remove(string key)
        {
        }

        public virtual void RemoveByPattern(string pattern)
        {
        }

        public virtual void Clear()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}
