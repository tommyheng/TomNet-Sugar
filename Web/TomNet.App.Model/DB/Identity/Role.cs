using SqlSugar;
using TomNet.SqlSugarCore.Entity;


namespace TomNet.App.Model.DB.Identity
{
    public class Role : IdentityEntityBase<int>
    {
        public string Name { get; set; }
        public bool IsLocked { get; set; }
    }
}