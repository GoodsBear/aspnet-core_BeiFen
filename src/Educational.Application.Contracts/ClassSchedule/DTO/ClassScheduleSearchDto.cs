using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.ClassSchedule.DTO
{
    public class ClassScheduleSearchDto:Seach
    {
        //分校（下拉）
        public Guid? organizationId {  get; set; }
        //班级（下拉）
        public Guid? ClassId {  get; set; }
        //课程名称
        public string? CourseName {  get; set; } 
    }   
}
