using System.Reflection;
using System.Runtime.InteropServices;

namespace SmartHome.BusinessLogic.AssemblyManagement;

public sealed class LoadAssembly<TInterface>(string path)
    where TInterface : class
{
    private readonly DirectoryInfo _directory = new(path);
    private List<Type> _implementations = [];

    public List<string> GetImplementations()
    {
        var files = _directory
            .GetFiles("*.dll")
            .ToList();

        _implementations = [];
        files.ForEach(file =>
        {
            var assemblyLoaded = Assembly.LoadFile(file.FullName);
            var loadedTypes = assemblyLoaded
                .GetTypes()
                .Where(t => t.IsClass && typeof(TInterface).IsAssignableFrom(t))
                .ToList();

            if (loadedTypes.Count == 0)
            {
                throw new AssemblyException(
                    $"Interface not implemented: {typeof(TInterface).Name} in the assembly: {file.FullName}");
            }

            _implementations = _implementations
                .Union(loadedTypes)
                .ToList();
        });

        return _implementations.ConvertAll(t => t.Name);
    }

    public TInterface GetImplementationByGuid(Guid id, params object[] args)
    {
        Type? type = _implementations.FirstOrDefault(t => GetGuid(t) == id);

        if (type == null)
        {
            throw new ArgumentException($"No implementation found with GUID: {id}");
        }

        return Activator.CreateInstance(type, args) as TInterface;
    }

    public Guid GetImplementationIdByName(string name)
    {
        Type? type = _implementations.FirstOrDefault(t => t.Name == name);

        if (type == null)
        {
            throw new ArgumentException($"No implementation found with name: {name}");
        }

        return GetGuid(type);
    }

    private static Guid GetGuid(Type type)
    {
        var attribute = (GuidAttribute?)type.GetCustomAttributes(typeof(GuidAttribute), true).FirstOrDefault();
        return attribute != null ? new Guid(attribute.Value) : Guid.Empty;
    }
}
