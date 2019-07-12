using Microsoft.Extensions.DependencyInjection;


namespace TomNet.Dependency
{
    /// <summary>
    /// <see cref="IServiceScope"/>工厂包装一下
    /// </summary>
    public interface IHybridServiceScopeFactory : IServiceScopeFactory
    { }
}