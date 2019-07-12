using SqlSugar;
using TomNet.SqlSugarCore.Entity;


namespace TomNet.App.Model.DB.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class UserLogin : IdentityEntityBase<int>
    {
        /// <summary>
        /// 
        /// </summary>
        public UserLogin()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsLocked { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime CreateTime { get; set; }
    }
}