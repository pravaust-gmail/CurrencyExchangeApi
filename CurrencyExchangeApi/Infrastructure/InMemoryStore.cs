using System.Collections.Concurrent;
using CurrencyExchangeApi.Models.Responses;

namespace CurrencyExchangeApi.Infrastructure;

public interface IStore<T>
{
    void Save(T item);
    T? Get(Guid id);
}

public class InMemoryStore<T> : IStore<T> where T : class
{
    private readonly ConcurrentDictionary<Guid, T> _store = new();

    private readonly Func<T, Guid> _getId;

    public InMemoryStore(Func<T, Guid> getId)
    {
        _getId = getId;
    }

    public void Save(T item)
    {
        var id = _getId(item);
        _store[id] = item;
    }

    public T? Get(Guid id)
    {
        _store.TryGetValue(id, out var item);
        return item;
    }
}