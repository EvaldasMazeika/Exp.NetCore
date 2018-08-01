using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpE.Domain;
using ExpE.Domain.Models;
using ExpE.Repository.Interfaces;
using MongoDB.Driver;

namespace ExpE.Repository.Repositories
{
    public class MongoDbRepository : IRepository
    {
        private readonly IMongoDbContext _context;

        public MongoDbRepository(IMongoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddForm(MyForm form)
        {
            try
            {
                await _context.Forms.InsertOneAsync(form);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> UpdateForm(MyForm form)
        {
            var actionResult = await _context.Forms.ReplaceOneAsync(x => x.Id.Equals(form.Id), form,
                new UpdateOptions() {IsUpsert = true});

            return actionResult.IsAcknowledged
                   && actionResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteFormById(string id)
        {
            var filter = Builders<MyForm>.Filter.Eq("_id", id);
            var result = await _context.Forms.DeleteOneAsync(filter);

            return result.DeletedCount != 0;
        }

        public async Task<IEnumerable<MyForm>> GetFormByName(string name)
        {
            var filter = Builders<MyForm>.Filter.Eq("Name", name);
            var result = await _context.Forms.Find(filter).ToListAsync();

            return result;
        }

        public async Task<bool> AddExpense(Expense model)
        {
            try
            {
                await _context.Expenses.InsertOneAsync(model);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Expense>> GettAllExpenses()
        {
            return await _context.Expenses.Find(_ => true).ToListAsync();
        }

        public async Task<Expense> GetExpenseById(string id)
        {
            return await _context.Expenses.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public bool ExistsFormName(string name)
        {
            var la =  _context.Forms.Find(x => x.Name == name).CountDocuments();
            return la > 0;
        }

        public async Task<MyForm> GetFormById(string id)
        {
            return await _context.Forms.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MyForm>> GetAllForms()
        {
            return await _context.Forms.Find(_ => true).ToListAsync();
        }
    }
}
