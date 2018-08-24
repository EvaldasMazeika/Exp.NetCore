using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpE.Domain;
using ExpE.Domain.Models;
using ExpE.Repository.Interfaces;

namespace ExpE.Repository.Repositories
{
    public class SQLRepository : IRepository
    {
        public Task<IEnumerable<MyForm>> GetAllForms()
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddForm(MyForm form)
        {
            throw new NotImplementedException();
        }

        public bool ExistsFormName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<MyForm> GetFormById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateForm(MyForm form)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteFormById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MyForm>> GetFormByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<Record>> GetRecords(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Record> GetRecord(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddRecord(Record record)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRecord(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRecord(Record record)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAutoCompletes(AutoCompleteList autoCompleteList)
        {
            throw new NotImplementedException();
        }

        public Task<AutoComplete> GetAutoComplete(string formId, string propertyKey)
        {
            throw new NotImplementedException();
        }

        public Task AddWordsToAutos(AutoCompleteWords words)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddSelectList(string id, string key, IEnumerable<DropDownOptions> dropDown)
        {
            throw new NotImplementedException();
        }

        public Task AddSelectItem(string id, string key, DropDownOptions dropDown)
        {
            throw new NotImplementedException();
        }

        public Task<List<DropDownOptions>> GetSelectList(string id, string key)
        {
            throw new NotImplementedException();
        }
    }
}
