using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Educational.Tools
{
    public static class ExcelExporter
    {
        /// <summary>
        /// 将列表导出为 Excel 文件字节流
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="list">数据列表</param>
        /// <param name="sheetName">工作表名称</param>
        /// <param name="titleName">表头标题</param>
        /// <returns>Excel 文件字节流</returns>
        public static byte[] Export<T>(List<T> list, string sheetName, string titleName)
        {
            try
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(sheetName);

                // 标题
                IRow titleRow = sheet.CreateRow(0);
                ICell titleCell = titleRow.CreateCell(0);
                titleCell.SetCellValue(titleName);

                ICellStyle titleStyle = workbook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.Center;

                IFont titleFont = workbook.CreateFont();
                titleFont.FontHeightInPoints = 20;
                titleFont.FontName = "微软雅黑";
                titleStyle.SetFont(titleFont);

                titleCell.CellStyle = titleStyle;

                // 收集属性和DisplayName对应关系
                var propertyInfos = typeof(T)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => new {
                        Property = p,
                        DisplayName = p.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? p.Name
                    })
                    .ToList();


                // 合并标题单元格
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, propertyInfos.Count - 1));

                // 表头
                IRow headerRow = sheet.CreateRow(1);
                for (int i = 0; i < propertyInfos.Count; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(propertyInfos[i].DisplayName);
                }


                // 数据
                for (int i = 0; i < list.Count; i++)
                {
                    var dataRow = sheet.CreateRow(i + 2);
                    var item = list[i];

                    for (int j = 0; j < propertyInfos.Count; j++)
                    {
                        var value = propertyInfos[j].Property.GetValue(item);
                        dataRow.CreateCell(j).SetCellValue(value?.ToString() ?? "");
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                // 可改成日志记录
                throw new Exception("Excel 导出失败", ex);
            }
        }
    }
}
