using ClosedXML.Excel;
using ExpE.Core.Interfaces;
using ExpE.Domain;
using ExpE.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpE.Core.Services
{
    public class ExcelExport : IExcelExport
    {
        public MemoryStream ExportSimpleExcel(MyForm form, IEnumerable<Record> records)
        {
            XLWorkbook workbook = new XLWorkbook();
            var memory = new MemoryStream();

            var ws = workbook.Worksheets.Add(form.Name);

            InsertExcelTable(1, ws, form, records);

            workbook.SaveAs(memory);
            memory.Position = 0;
            return memory;
        }

        private void InsertExcelTable(int startRow, IXLWorksheet ws, MyForm form, IEnumerable<Record> records)
        {
            ws.Cell(startRow, 1).SetValue("No");
            ws.Cell(startRow, 1).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            ws.Cell(startRow, 1).Style.Border.BottomBorderColor = XLColor.Black;
            ws.Cell(startRow, 1).Style.Font.Bold = true;
            ws.Cell(startRow, 1).Style.Font.FontSize = 12;

            var rowOfIds = ws.Cell(startRow + 1, 1).InsertData(Enumerable.Range(1, records.Count()));
            rowOfIds.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rowOfIds.Style.Border.OutsideBorderColor = XLColor.Black;
            rowOfIds.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            rowOfIds.Style.Border.InsideBorderColor = XLColor.Black;

            var column = -1;

            for (int i = 0; i < form.Items.Count(); i++)
            {
                var temp = form.Items.ElementAt(i);

                if (temp.TemplateOptions.IsExportable == true)
                {
                    column++;
                    var items = new List<object>();
                    var body = records.Select(s => s.Body);

                    foreach (var item in body)
                    {
                        var value = item.Where(w => w.Key == temp.Key).Select(w => w.Value).FirstOrDefault();

                        if (value == null)
                        {
                            value = "";
                        }
                        else if (temp.Type == "primeCalendar")
                        {
                            var local = (DateTime)value;
                            value = local.ToLocalTime();
                        }
                        else if (typeof(IList).IsAssignableFrom(value.GetType()))
                        {
                            var valu = (IList)value;
                            var val = valu.Cast<string>();
                            value = String.Join(',', val);
                        }

                        items.Add(value);
                    }
                    ws.Cell(startRow, column + 2).SetValue(temp.TemplateOptions.Label);
                    ws.Cell(startRow, column + 2).Style.Border.BottomBorder = XLBorderStyleValues.Thick;
                    ws.Cell(startRow, column + 2).Style.Border.BottomBorderColor = XLColor.Black;
                    ws.Cell(startRow, column + 2).Style.Font.Bold = true;
                    ws.Cell(startRow, column + 2).Style.Font.FontSize = 12;

                    var rangeOfTable = ws.Cell(startRow + 1, column + 2).InsertData(items.AsEnumerable());
                    rangeOfTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    rangeOfTable.Style.Border.OutsideBorderColor = XLColor.Black;
                    rangeOfTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    rangeOfTable.Style.Border.InsideBorderColor = XLColor.Black;

                    if (temp.Type == "primeCalendar")
                    {
                        rangeOfTable.Style.DateFormat.Format = temp.TemplateOptions.DateFormat;
                    }
                }
            }

            ws.Row(startRow).Height = 50;
            ws.Columns().AdjustToContents();
        }

        public MemoryStream ExportUsingTemplate(MemoryStream templateStream, MyForm form, IEnumerable<Record> records)
        {
            XLWorkbook workbook = new XLWorkbook(templateStream);
            var ws = workbook.Worksheet(1);

            var NumberOfLastRow = ws.LastRowUsed().RowNumber() + 2;

            InsertExcelTable(NumberOfLastRow, ws, form, records);

            workbook.SaveAs(templateStream);
            templateStream.Position = 0;
            return templateStream;
        }
    }
}
