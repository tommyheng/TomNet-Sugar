using SqlSugar;
using TomNet.SqlSugarCore.Entity;


namespace TomNet.App.Model.DB.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRoleMap : IdentityEntityBase<int>
    {
        /// <summary>
        /// 
        /// </summary>
        public UserRoleMap()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RoleId { get; set; }
    }
}