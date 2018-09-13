using ExpE.Domain;
using ExpE.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpE.Core.Interfaces
{
    public interface IExcelExport
    {
        MemoryStream ExportSimpleExcel(MyForm form, IEnumerable<Record> records);
        MemoryStream ExportUsingTemplate(MemoryStream templateStream, MyForm form, IEnumerable<Record> records);
    }
}
