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

        public Task<bool> AddExpense(Expense model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Expense>> GettAllExpenses()
        {
            throw new NotImplementedException();
        }

        public Task<Expense> GetExpenseById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
