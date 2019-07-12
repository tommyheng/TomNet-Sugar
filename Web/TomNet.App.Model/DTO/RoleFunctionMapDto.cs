using System;
using System.Collections.Generic;
using System.Text;

namespace TomNet.App.Model.DTO
{
    public class RoleFunctionMapDto
    {
        public string Source { get; set; }
        public int RoleId { get; set; }
        //public string RoleName { get; set; }
        public bool IsWebApi { get; set; }

        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }
        public bool IsLocked { get; set; }
    }
}
