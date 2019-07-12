using SqlSugar;
using TomNet.SqlSugarCore.Entity;


namespace TomNet.App.Model.DB.Security
{
    public class RoleFunctionMap : IdentityEntityBase<int>
    {
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
    }
}