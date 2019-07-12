using System.Reflection;

using TomNet.Dependency;
using TomNet.Finders;


namespace TomNet.Reflection
{
    /// <summary>
    /// 定义程序集查找器
    /// </summary>
    [IgnoreDependency]
    public interface IAssemblyFinder : IFinder<Assembly>
    { }
}