using System;
using System.Linq;

using TomNet.Reflection;

namespace TomNet.Core.Packs
{
    /// <summary>
    /// TomNet模块类型查找器
    /// </summary>
    public class TomNetPackTypeFinder : BaseTypeFinderBase<TomNetPack>, ITomNetPackTypeFinder
    {
        /// <summary>
        /// 初始化一个<see cref="TomNetPackTypeFinder"/>类型的新实例
        /// </summary>
        public TomNetPackTypeFinder(IAllAssemblyFinder allAssemblyFinder)
            : base(allAssemblyFinder)
        { }

        /// <summary>
        /// 重写以实现所有项的查找
        /// </summary>
        /// <returns></returns>
        protected override Type[] FindAllItems()
        {
            //排除被继承的Pack实类
            Type[] types = base.FindAllItems();
            Type[] basePackTypes = types.Select(m => m.BaseType).Where(m => m != null && m.IsClass && !m.IsAbstract).ToArray();
            return types.Except(basePackTypes).ToArray();
        }
    }
}
