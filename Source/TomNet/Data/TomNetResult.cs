using System;

using TomNet.Extensions;


namespace TomNet.Data
{
    /// <summary>
    /// TomNet结果基类
    /// </summary>
    /// <typeparam name="TResultType"></typeparam>
    public abstract class TomNetResult<TResultType> : TomNetResult<TResultType, object>, ITomNetResult<TResultType>
    {
        /// <summary>
        /// 初始化一个<see cref="TomNetResult{TResultType}"/>类型的新实例
        /// </summary>
        protected TomNetResult()
            : this(default(TResultType))
        { }

        /// <summary>
        /// 初始化一个<see cref="TomNetResult{TResultType}"/>类型的新实例
        /// </summary>
        protected TomNetResult(TResultType type)
            : this(type, null, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="TomNetResult{TResultType}"/>类型的新实例
        /// </summary>
        protected TomNetResult(TResultType type, string message)
            : this(type, message, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="TomNetResult{TResultType}"/>类型的新实例
        /// </summary>
        protected TomNetResult(TResultType type, string message, object data)
            : base(type, message, data)
        { }
    }


    /// <summary>
    /// TomNet结果基类
    /// </summary>
    /// <typeparam name="TResultType">结果类型</typeparam>
    /// <typeparam name="TData">结果数据类型</typeparam>
    public abstract class TomNetResult<TResultType, TData> : ITomNetResult<TResultType, TData>
    {
        /// <summary>
        /// 内部消息
        /// </summary>
        protected string _message;

        /// <summary>
        /// 初始化一个<see cref="TomNetResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected TomNetResult()
            : this(default(TResultType))
        { }

        /// <summary>
        /// 初始化一个<see cref="TomNetResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected TomNetResult(TResultType type)
            : this(type, null, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="TomNetResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected TomNetResult(TResultType type, string message)
            : this(type, message, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="TomNetResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected TomNetResult(TResultType type, string message, TData data)
        {
            if (message == null && typeof(TResultType).IsEnum)
            {
                message = (type as Enum)?.ToDescription();
            }
            ResultType = type;
            _message = message;
            Data = data;
        }

        /// <summary>
        /// 获取或设置 结果类型
        /// </summary>
        public TResultType ResultType { get; set; }

        /// <summary>
        /// 获取或设置 返回消息
        /// </summary>
        public virtual string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 获取或设置 结果数据
        /// </summary>
        public TData Data { get; set; }
    }
}