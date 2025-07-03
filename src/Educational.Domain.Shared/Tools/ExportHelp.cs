using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="list">数据列表</param>
        /// <param name="sheetName">Sheet名称</param>
        /// <param name="titleName">表头标题</param>
        /// <returns>Excel 文件字节流</returns>
        public static byte[] Export<T>(List<T> list, string sheetName, string titleName)
        {
            try
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(sheetName);

                // 反射获取属性及DisplayName
                var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => new
                    {
                        Property = p,
                        DisplayName = p.GetCustomAttribute<DisplayAttribute>()?.Name
                            ?? p.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                            ?? p.Name
                    })
                    .ToList();

                int colCount = propertyInfos.Count;

                // 标题行
                IRow titleRow = sheet.CreateRow(0);
                ICell titleCell = titleRow.CreateCell(0);
                titleCell.SetCellValue(titleName);

                // 合并标题单元格
                if (colCount > 1)
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, colCount - 1));

                // 标题样式
                var titleStyle = workbook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.Center;
                var titleFont = workbook.CreateFont();
                titleFont.FontHeightInPoints = 20;
                titleFont.FontName = "微软雅黑";
                titleStyle.SetFont(titleFont);
                titleCell.CellStyle = titleStyle;

                // 表头行
                IRow headerRow = sheet.CreateRow(1);
                for (int i = 0; i < colCount; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(propertyInfos[i].DisplayName);
                }

                // 数据行
                for (int i = 0; i < list.Count; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 2);
                    var item = list[i];
                    for (int j = 0; j < colCount; j++)
                    {
                        var value = propertyInfos[j].Property.GetValue(item, null);
                        dataRow.CreateCell(j).SetCellValue(value?.ToString() ?? "");
                    }
                }

                // 自动列宽
                for (int i = 0; i < colCount; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                // 可改为日志记录
                throw new Exception("Excel 导出失败", ex);
            }
        }
    }
}
