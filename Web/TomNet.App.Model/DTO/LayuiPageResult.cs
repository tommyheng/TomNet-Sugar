using System;
using System.Collections.Generic;
using System.Text;
using TomNet.Data;

namespace TomNet.App.Model.DTO
{
    public class LayuiPageResult<T> 
    {
        public int Code { get; set; }
        public int Count { get; set; }
        public string Msg { get; set; }
        public T[] Data { get; set; }
    }
}