using Educational.Organization;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational
{
    /// <summary>
    /// 分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiPaging<T>
    {
        public int TotleCount { get; set; }
        public int TotlePage { get; set; }
        public T Data { get; set; }
    }
    public class Seach
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}

