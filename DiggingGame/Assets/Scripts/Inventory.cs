using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Inventory : IEnumerable<ResourceEnum>
{
    private int _money;

    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            Events.OnInventoryChanged.Invoke(this);
        }
    }
    private readonly List<ResourceEnum> _items = new ();

    public void Add(ResourceEnum item)
    {
        _items.Add(item);
        Events.OnInventoryChanged.Invoke(this);
    }

    public void Remove(ResourceEnum item)
    {
        _items.Remove(item);
        Events.OnInventoryChanged.Invoke(this);
    }

    public void RemoveAll(IEnumerable<ResourceEnum> items)
    {
        foreach (var item in items)
        {
            _items.Remove(item);
        }
        Events.OnInventoryChanged.Invoke(this);
    }
    public void RemoveAll(ResourceEnum item)
    {
        _items.RemoveAll(x => x == item);
        Events.OnInventoryChanged.Invoke(this);
    }
    public int Count(ResourceEnum item) => _items.Count(x => x == item);

    public void AddRange(IEnumerable<ResourceEnum> items)
    {
        _items.AddRange(items);
        Events.OnInventoryChanged.Invoke(this);
    }

    public void Clear()
    {
        _items.Clear();
        Events.OnInventoryChanged.Invoke(null);
    }

    public bool Has(ResourceEnum item)
    {
        return _items.Contains(item);
    }
    public bool Has(List<ResourceEnum> items)
    {
        return items.All(item => Count(item) >= items.Count(x => x == item));
    }

    public IEnumerator<ResourceEnum> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }
}