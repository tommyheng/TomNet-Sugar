using System;
using System.Collections.Generic;
using System.Text;
using TomNet.SqlSugarCore.Entity;

namespace TomNet.App.Model.DTO
{
    public class RoleFunctionViewDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string area { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public FunctionAccessType accesstype { get; set; }
        public bool islocked { get; set; }
        public bool iswebapi { get; set; }
        public bool iscontroller { get; set; }
        //public int count { get; set; }
        public string source { get; set; }
        public bool LAY_CHECKED { get; set; }
    }
}
