using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpE.Domain;
using ExpE.Domain.Models;
using ExpE.Repository.Interfaces;
using MongoDB.Bson;
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




        public async Task<List<Record>> GetRecords(string id)
        {
            var records = await _context.Records.FindAsync(x => x.FormId == id);

            return await records.ToListAsync();
        }

        public async Task<Record> GetRecord(string id)
        {
            return await _context.Records.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> AddRecord(Record record)
        {
            try
            {
                await _context.Records.InsertOneAsync(record);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteRecord(string id)
        {
            var filter = Builders<Record>.Filter.Eq("_id", id);
            var result = await _context.Records.DeleteOneAsync(filter);

            return result.DeletedCount != 0;
        }

        public async Task<bool> UpdateRecord(Record record)
        {
            var actionResult = await _context.Records.ReplaceOneAsync(x => x.Id.Equals(record.Id), record,
    new UpdateOptions() { IsUpsert = true });

            return actionResult.IsAcknowledged
                   && actionResult.ModifiedCount > 0;
        }

        public async Task<bool> AddAutoCompletes(AutoCompleteList autoCompleteList)
        {
            try
            {
                foreach (var item in autoCompleteList.Properties)
                {
                    var record = await _context.AutoCompletes.Find(x => x.FormId == autoCompleteList.FormId && x.PropertyKey == item).FirstOrDefaultAsync();
                    if (record == null)
                    {
                        var id = ObjectId.GenerateNewId().ToString();
                        var res = new AutoComplete { Id = id, FormId = autoCompleteList.FormId, PropertyKey = item, Items = new List<string>() };
                        await _context.AutoCompletes.InsertOneAsync(res);
                    }
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<AutoComplete> GetAutoComplete(string formId, string propertyKey)
        {
            var result = await _context.AutoCompletes.FindAsync(w => w.FormId == formId && w.PropertyKey == propertyKey);
            return await result.SingleAsync();
        }

        public async Task AddWordsToAutos(AutoCompleteWords words)
        {
            foreach (var item in words.Words)
            {
                var autoObject = await _context.AutoCompletes.Find(w => w.FormId == words.FormId && w.PropertyKey == item.Key).SingleAsync();
                if (!autoObject.Items.Contains(item.Value))
                {
                  //  autoObject.Items.Append(item.Value);

                    var filter = Builders<AutoComplete>.Filter.Eq(s => s.Id, autoObject.Id);
                    var update = Builders<AutoComplete>.Update.AddToSet(s => s.Items, item.Value);
                    await _context.AutoCompletes.UpdateOneAsync(filter, update);
                }
            }
        }
    }
}
