using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpE.Domain;
using ExpE.Domain.Models;

namespace ExpE.Repository.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<MyForm>> GetAllForms();
        Task<bool> AddForm(MyForm form);
        bool ExistsFormName(string name);
        Task<MyForm> GetFormById(string id);
        Task<bool> UpdateForm(MyForm form);
        Task<bool> DeleteFormById(string id);
        Task<IEnumerable<MyForm>> GetFormByName(string name);

        Task<List<Record>> GetRecords(string id);
        Task<Record> GetRecord(string id);
        Task<bool> AddRecord(Record record);
        Task<bool> DeleteRecord(string id);
        Task<bool> UpdateRecord(Record record);
    }
}
