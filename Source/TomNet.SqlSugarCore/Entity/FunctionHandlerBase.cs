using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TomNet.Collections;
using TomNet.Core;
using TomNet.Core.Options;
using TomNet.Data;
using TomNet.Dependency;
using TomNet.Exceptions;
using TomNet.Reflection;


namespace TomNet.SqlSugarCore.Entity
{
    /// <summary>
    /// 功能信息处理基类
    /// </summary>
    public abstract class FunctionHandlerBase : IFunctionHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<Function> _functions = new List<Function>();

        /// <summary>
        /// 初始化一个<see cref="FunctionHandlerBase{TFunction}"/>类型的新实例
        /// </summary>
        protected FunctionHandlerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            options = serviceProvider.GetTomNetOptions();
            Logger = serviceProvider.GetLogger(GetType());
        }
        /// <summary>
        /// 配置信息
        /// </summary>
        protected TomNetOptions options;
        /// <summary>
        /// 获取 日志记录对象
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// 获取 功能类型查找器
        /// </summary>
        public abstract IFunctionTypeFinder FunctionTypeFinder { get; }

        /// <summary>
        /// 获取 功能方法查找器
        /// </summary>
        public abstract IMethodInfoFinder MethodInfoFinder { get; }

        /// <summary>
        /// 从程序集中获取功能信息（如MVC的Controller-Action）
        /// </summary>
        public void Initialize()
        {
            var auto = options.LocalOption.AutoFunctions;
            if (!auto)
            {
                Logger.LogInformation($"已经屏蔽功能信息初始化功能");
                return;
            }
            Check.NotNull(FunctionTypeFinder, nameof(FunctionTypeFinder));

            Type[] functionTypes = FunctionTypeFinder.FindAll(true);
            Function[] functions = GetFunctions(functionTypes);
            Logger.LogInformation($"功能信息初始化，共找到{functions.Length}个功能信息");

            _serviceProvider.ExecuteScopedWork(provider =>
            {
                SyncToDatabase(provider, functions);
            });

            RefreshCache();
        }

        /// <summary>
        /// 查找指定条件的功能信息
        /// </summary>
        /// <param name="area">区域</param>
        /// <param name="controller">控制器</param>
        /// <param name="action">功能方法</param>
        /// <returns>功能信息</returns>
        public Function GetFunction(string area, string controller, string action)
        {
            if (_functions.Count == 0)
            {
                RefreshCache();
            }
            return _functions.FirstOrDefault(m =>
                string.Equals(m.Area, area, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Controller, controller, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Action, action, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 刷新功能信息缓存
        /// </summary>
        public void RefreshCache()
        {
            _serviceProvider.ExecuteScopedWork(provider =>
            {
                _functions.Clear();
                _functions.AddRange(GetFromDatabase(provider));
            });
        }

        /// <summary>
        /// 清空功能信息缓存
        /// </summary>
        public void ClearCache()
        {
            _functions.Clear();
        }

        /// <summary>
        /// 从功能类型中获取功能信息
        /// </summary>
        /// <param name="functionTypes">功能类型集合</param>
        /// <returns></returns>
        protected virtual Function[] GetFunctions(Type[] functionTypes)
        {
            List<Function> functions = new List<Function>();
            foreach (Type type in functionTypes.OrderBy(m => m.FullName))
            {
                Function controller = GetFunction(type);
                if (controller == null)
                {
                    continue;
                }
                if (!HasPickup(functions, controller))
                {
                    functions.Add(controller);
                }
                MethodInfo[] methods = MethodInfoFinder.FindAll(type);
                foreach (MethodInfo method in methods)
                {
                    Function action = GetFunction(controller, method);
                    if (action == null)
                    {
                        continue;
                    }
                    if (IsIgnoreMethod(action, method, functions))
                    {
                        continue;
                    }
                    functions.Add(action);
                }
            }
            return functions.ToArray();
        }

        /// <summary>
        /// 重写以实现从功能类型创建功能信息
        /// </summary>
        /// <param name="type">功能类型</param>
        /// <returns></returns>
        protected abstract Function GetFunction(Type type);

        /// <summary>
        /// 重写以实现从方法信息中创建功能信息
        /// </summary>
        /// <param name="typeFunction">类功能信息</param>
        /// <param name="method">方法信息</param>
        /// <returns></returns>
        protected abstract Function GetFunction(Function typeFunction, MethodInfo method);

        /// <summary>
        /// 重写以判断指定功能信息是否已提取过
        /// </summary>
        /// <param name="functions">已提取功能信息集合</param>
        /// <param name="function">要判断的功能信息</param>
        /// <returns></returns>
        protected virtual bool HasPickup(List<Function> functions, Function function)
        {
            return functions.Any(m =>
                m.IsWebApi == function.IsWebApi
                && string.Equals(m.Area, function.Area, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Controller, function.Controller, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Action, function.Action, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Name, function.Name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 重写以实现功能信息查找
        /// </summary>
        /// <param name="functions">功能信息集合</param>
        /// <param name="area">区域名称</param>
        /// <param name="controller">类型名称</param>
        /// <param name="action">方法名称</param>
        /// <param name="name">功能名称</param>
        /// <returns></returns>
        protected virtual Function GetFunction(IEnumerable<Function> functions, bool iswebapi, string area, string controller, string action, string name)
        {
            return functions.FirstOrDefault(m =>
                m.IsWebApi == iswebapi
                && string.Equals(m.Area, area, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Controller, controller, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Action, action, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 重写以实现是否忽略指定方法的功能信息
        /// </summary>
        /// <param name="action">要判断的功能信息</param>
        /// <param name="method">功能相关的方法信息</param>
        /// <param name="functions">已存在的功能信息集合</param>
        /// <returns></returns>
        protected virtual bool IsIgnoreMethod(Function action, MethodInfo method, IEnumerable<Function> functions)
        {
            Function exist = GetFunction(functions, action.IsWebApi, action.Area, action.Controller, action.Action, action.Name);
            return exist != null;
        }

        /// <summary>
        /// 将从程序集获取的功能信息同步到数据库中
        /// </summary>
        /// <param name="scopedProvider">局部服务程序提供者</param>
        /// <param name="functions">程序集获取的功能信息</param>
        protected virtual void SyncToDatabase(IServiceProvider scopedProvider, Function[] functions)
        {
            Check.NotNull(functions, nameof(functions));
            if (functions.Length == 0)
            {
                return;
            }

            IRepository<Function, int> repository = scopedProvider.GetService<IRepository<Function, int>>();
            if (repository == null)
            {
                Logger.LogWarning("初始化功能数据时，IRepository<,>的服务未找到");
                return;
            }
            repository.UnitOfWork.BeginTran();

            try
            {
                var app = options.LocalOption.AppKey;
                Function[] dbItems = repository.Entities.Where(m => m.Source == app).ToArray();

                //删除的功能
                Function[] removeItems = dbItems.Except(functions,
                    EqualityHelper<Function>.CreateComparer(m => m.IsWebApi.ToString() + m.Area + m.Controller + m.Action)).ToArray();
                int removeCount = removeItems.Length;
                if (removeCount > 0)
                {
                    repository.Delete(removeItems.Select(m => m.Id).ToArray());
                }


                //新增的功能
                Function[] addItems = functions.Except(dbItems,
                    EqualityHelper<Function>.CreateComparer(m => m.IsWebApi.ToString() + m.Area + m.Controller + m.Action)).ToArray();
                int addCount = addItems.Length;
                if (addCount > 0)
                {
                    repository.Insert(addItems);
                }


                //更新的功能信息
                int updateCount = 0;
                Function[] updateItems = dbItems.Except(removeItems).ToArray();
                foreach (Function item in updateItems)
                {
                    bool isUpdate = false;
                    Function function;
                    try
                    {
                        function = functions.Single(m =>
                            m.IsWebApi == item.IsWebApi
                            && string.Equals(m.Area, item.Area, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(m.Controller, item.Controller, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(m.Action, item.Action, StringComparison.OrdinalIgnoreCase));
                    }
                    catch (InvalidOperationException)
                    {
                        throw new TomNetException($"发现多个“{item.Area}-{item.Controller}-{item.Action}”的功能信息，不允许重名");
                    }
                    if (function == null)
                    {
                        continue;
                    }
                    if (!string.Equals(item.Name, function.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        item.Name = function.Name;
                        isUpdate = true;
                    }

                    if (item.AccessType != function.AccessType)
                    {
                        item.AccessType = function.AccessType;
                        isUpdate = true;
                    }
                    if (isUpdate)
                    {
                        repository.Update(item);
                        updateCount++;
                        Logger.LogDebug($"更新功能“{function.Name}({function.Area}/{function.Controller}/{function.Action})”");
                    }
                }
                repository.UnitOfWork.Commit();
                if (removeCount + addCount + updateCount > 0)
                {
                    string msg = "刷新功能信息";
                    if (addCount > 0)
                    {
                        foreach (Function function in addItems)
                        {
                            Logger.LogDebug($"新增功能“{function.Name}({function.Area}/{function.Controller}/{function.Action})”");
                        }
                        msg += "，添加功能信息 " + addCount + " 个";
                    }
                    if (updateCount > 0)
                    {
                        msg += "，更新功能信息 " + updateCount + " 个";
                    }
                    if (removeCount > 0)
                    {
                        foreach (Function function in removeItems)
                        {
                            Logger.LogDebug($"更新功能“{function.Name}({function.Area}/{function.Controller}/{function.Action})”");
                        }
                        msg += "，移除功能信息 " + removeCount + " 个";
                    }
                    Logger.LogInformation(msg);
                }
            }
            catch (Exception ex)
            {
                repository.UnitOfWork.Rollback();
                Logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// 从数据库获取最新功能信息
        /// </summary>
        /// <returns></returns>
        protected virtual Function[] GetFromDatabase(IServiceProvider scopedProvider)
        {
            IRepository<Function, int> repository = scopedProvider.GetService<IRepository<Function, int>>();
            if (repository == null)
            {
                return new Function[0];
            }
            return repository.Entities.ToArray();
        }

    }
}