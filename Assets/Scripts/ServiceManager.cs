using System.Collections.Generic;

public class ServiceManager : Singleton<ServiceManager>
{
    private readonly List<object> _services = new();
    public T Get<T>() where T : class, new()
    {
        var srv = _services.Find(x => x is T);
        if (srv is not null) return srv as T;
        
        srv = new T();
        _services.Add(srv);
        return (T)srv;
    }
}