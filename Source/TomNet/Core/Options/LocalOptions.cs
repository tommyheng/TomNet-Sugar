using System;
using System.Collections.Generic;
using System.Text;

namespace TomNet.Core.Options
{
    public class LocalOptions
    {
        /// <summary>
        /// 获取或设置站点的标识
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 获取或设置 是否自动采集访问权限
        /// </summary>
        public bool AutoFunctions { get; set; }
    }
}
