using System;
using System.Collections.Generic;
using System.Text;
using TomNet.SqlSugarCore.Entity;

namespace TomNet.App.Model.DB.Identity
{
    public class UserInfo : IdentityEntityBase<int>
    {
        public UserInfo()
        {
        }
        public int UserId { get; set; }
        public string NickName { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string MobilePhone { get; set; }
        public string IdNumber { get; set; }
        public bool Sex { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
    }
}
