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
        Task<MyForm> AddForm(MyForm form);
        bool ExistsFormName(string name);
        Task<MyForm> GetFormById(string id);
        Task DeleteFormById(string id);

        Task<List<Record>> GetRecords(string id);
        Task<Record> GetRecord(string id);
        Task<Record> AddRecord(Record record);
        Task DeleteRecord(string id);
        Task<Record> UpdateRecord(Record record);
        Task AddAutoCompletes(AutoCompleteList autoCompleteList);
        Task<AutoComplete> GetAutoComplete(string formId, string propertyKey);
        Task AddWordsToAutos(AutoCompleteWords words);
        Task AddSelectList(string id, string key, IEnumerable<DropDownOptions> dropDown);
        Task AddSelectItem(string id, string key, DropDownOptions dropDown);
        Task<List<DropDownOptions>> GetSelectList(string id, string key);
        Task DeleteProperty(string formId, string key);
        Task<Property> AddProperty(string formId, Property property);
        Task<Property> GetProperty(string formId, string key);
        Task<Property> UpdateProperty(string formId, Property property);
        Task UpdateSelectList(string formId, string propertyKey, IEnumerable<DropDownOptions> dropDown);
        Task<Record> GetLatestRecord(string formId);
    }
}
