public class InMemoryCache : ICacheService
{
    #region ICacheService Members

    public T Get<T>(string cacheId, Func<T> getItemCallback) where T : class
    {
        var item = HttpRuntime.Cache.Get(cacheId) as T;
        if (item == null)
        {
            item = getItemCallback();
            HttpContext.Current.Cache.Insert(cacheId, item);
        }
        return item;
    }

    public void Clear(string cacheId)
    {
        HttpContext.Current.Cache.Remove(cacheId);
    }

    #endregion
}

public interface ICacheService
{
    T Get<T>(string cacheId, Func<T> getItemCallback) where T : class;
    void Clear(string cacheId);
}
