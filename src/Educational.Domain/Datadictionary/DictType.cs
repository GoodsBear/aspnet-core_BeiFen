using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Educational.Datadictionary
{
    public class DictType : FullAuditedEntity<long>
    {
        public string Code { get; set; }  // 字典类型编码
        public string Name { get; set; }  // 字典类型名称
        public string Description { get; set; }  // 描述
        public bool IsEnabled { get; set; } = true;  // 是否启用
    }

    public class DictItem : FullAuditedEntity<long>
    {
        public long DictTypeId { get; set; }  // 外键
        public string Code { get; set; }  // 数据项编码
        public string Name { get; set; }  // 数据项名称
        public int SortOrder { get; set; }  // 排序
        public bool IsEnabled { get; set; } = true;  // 是否启用

        public virtual DictType DictType { get; set; }
    }
}
