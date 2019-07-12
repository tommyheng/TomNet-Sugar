using System;

using TomNet.Dependency;
using TomNet.Finders;


namespace TomNet.Reflection
{
    /// <summary>
    /// 定义类型查找行为
    /// </summary>
    [IgnoreDependency]
    public interface ITypeFinder : IFinder<Type>
    { }
}