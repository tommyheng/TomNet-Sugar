namespace TomNet.SqlSugarCore.Entity
{
    public class Function : IdentityEntityBase<int>
    {
        /// <summary>
        /// 数据来源，按Web程序区分
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 是否是WebApi
        /// </summary>
        public bool IsWebApi { get; set; }
        /// <summary>
        /// 是否是控制器
        /// </summary>
        public bool IsController { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// Action
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 访问限制
        /// </summary>
        public FunctionAccessType AccessType { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }
    }
}
