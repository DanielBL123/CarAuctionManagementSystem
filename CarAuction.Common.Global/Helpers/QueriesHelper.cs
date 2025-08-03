using System.Reflection;

namespace CarAuction.Common.Global.Helpers;

public static class QueriesHelper
{
    public static Dictionary<string, Type> GetVehicleTypes(string assemblyName, string baseClassName)
    {
        var assembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == assemblyName);

        if (assembly == null)
            throw new ArgumentException($"Assembly '{assemblyName}' not found.");

        var baseType = assembly.GetTypes()
            .FirstOrDefault(t => t.Name == baseClassName && t.IsClass || t.IsAbstract);

        if (baseType == null)
            throw new ArgumentException($"Base type '{baseClassName}' not found in assembly '{assemblyName}'.");

        return assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t))
            .ToDictionary(t => t.Name.ToLower(), t => t);
    }
}
