using System;
using System.Collections.Generic;
using System.Text;

namespace TomNet.App.Model.DTO
{
    public class RoleFunctionSetDto
    {
        public int RoleId { get; set; }
        public int[] FunctionIds { get; set; }
    }
}
