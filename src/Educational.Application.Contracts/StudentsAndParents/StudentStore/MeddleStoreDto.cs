using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.StudentsAndParents.StudentStore
{
    /// <summary>
    /// 学生积分中间表
    /// </summary>
    public class MeddleStoreDto
    {
        public Guid StudentId { get; set; }

        public Guid StoreId { get; set; }

    }
}
