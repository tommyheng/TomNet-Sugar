using System;
using System.Linq;
using System.Reflection;
using TomNet.Finders;
using TomNet.Reflection;

namespace TomNet.AspNetCore.Mvc
{
    /// <summary>
    /// MVC控制器类型查找器
    /// </summary>
    public class MvcControllerTypeFinder : FinderBase<Type>, IFunctionTypeFinder
    {
        private readonly IAllAssemblyFinder _allAssemblyFinder;

        /// <summary>
        /// 初始化一个<see cref="MvcControllerTypeFinder"/>类型的新实例
        /// </summary>
        public MvcControllerTypeFinder(IAllAssemblyFinder allAssemblyFinder)
        {
            _allAssemblyFinder = allAssemblyFinder;
        }

        /// <summary>
        /// 重写以实现所有项的查找
        /// </summary>
        /// <returns></returns>
        protected override Type[] FindAllItems()
        {
            Assembly[] assemblies = _allAssemblyFinder.FindAll(true);
            return assemblies.SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsController()).ToArray();
        }
    }
}