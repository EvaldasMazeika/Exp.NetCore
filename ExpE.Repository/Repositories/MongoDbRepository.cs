﻿using System;
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

        public async Task<MyForm> AddForm(MyForm form)
        {
            form.Id = ObjectId.GenerateNewId().ToString();

            await _context.Forms.InsertOneAsync(form);

            return await _context.Forms.Find(w => w.Id == form.Id).FirstOrDefaultAsync();
        }

        public async Task DeleteFormById(string id)
        {
            //deletes form
            var filterForm = Builders<MyForm>.Filter.Eq("_id", id);
            var result = await _context.Forms.DeleteOneAsync(filterForm);

            //deletes related records
            var filterRecords = Builders<Record>.Filter.Eq("FormId", id);
            await _context.Records.DeleteManyAsync(filterRecords);

            //delete related auto completes
            var filterAutoCompletes = Builders<AutoComplete>.Filter.Eq("FormId", id);
            await _context.AutoCompletes.DeleteManyAsync(filterAutoCompletes);
        }

        public bool ExistsFormName(string name)
        {
            var la = _context.Forms.Find(x => x.Name == name).CountDocuments();
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


        public async Task<IEnumerable<Record>> GetRecords(string id)
        {
            var records = await _context.Records.FindAsync(x => x.FormId == id);

            return await records.ToListAsync();
        }

        public async Task<Record> GetRecord(string id)
        {
            return await _context.Records.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Record> AddRecord(Record record)
        {
            record.Id = ObjectId.GenerateNewId().ToString();

            await _context.Records.InsertOneAsync(record);

            return await _context.Records.Find(w => w.Id == record.Id).FirstOrDefaultAsync();
        }

        public async Task DeleteRecord(string id)
        {
            var filter = Builders<Record>.Filter.Eq("_id", id);
            await _context.Records.DeleteOneAsync(filter);
        }

        public async Task<Record> UpdateRecord(Record record)
        {
           await _context.Records.ReplaceOneAsync(x => x.Id.Equals(record.Id), record, new UpdateOptions() { IsUpsert = true });
           return await _context.Records.Find(w => w.Id == record.Id).FirstOrDefaultAsync();
        }

        public async Task AddAutoCompletes(AutoCompleteList autoCompleteList)
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
                    var filter = Builders<AutoComplete>.Filter.Eq(s => s.Id, autoObject.Id);
                    var update = Builders<AutoComplete>.Update.AddToSet(s => s.Items, item.Value);
                    await _context.AutoCompletes.UpdateOneAsync(filter, update);
                }
            }
        }

        public async Task AddSelectList(string id, string key, IEnumerable<DropDownOptions> dropDown)
        {
            var ids = ObjectId.GenerateNewId().ToString();

            var res = new SelectList { Id = ids, FormId = id, PropertyKey = key, Items = dropDown };
            await _context.SelectLists.InsertOneAsync(res);
        }

        public async Task AddSelectItem(string id, string key, DropDownOptions dropDown)
        {
            var selectObject = await _context.SelectLists.Find(w => w.FormId == id && w.PropertyKey == key).SingleAsync();
            var filter = Builders<SelectList>.Filter.Eq(s => s.Id, selectObject.Id);
            var update = Builders<SelectList>.Update.AddToSet(s => s.Items, dropDown);
            await _context.SelectLists.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<DropDownOptions>> GetSelectList(string id, string key)
        {
            var items = await _context.SelectLists.Find(x => x.FormId == id && x.PropertyKey == key).SingleAsync();

            return items.Items.ToList();
        }

        public async Task DeleteProperty(string formId, string key)
        {

            var update = Builders<MyForm>.Update.PullFilter(p => p.Items, f => f.Key == key);
            var result = await _context.Forms.UpdateOneAsync(z => z.Id == formId, update);

            // if it is auto complete, delete it
            var filterAutoCompletes = Builders<AutoComplete>.Filter.Where(w => w.FormId == formId && w.PropertyKey == key);
            await _context.AutoCompletes.DeleteOneAsync(filterAutoCompletes);

            // if it is select list, delete entity
            var filterSelectList = Builders<SelectList>.Filter.Where(w => w.FormId == formId && w.PropertyKey == key);
            await _context.SelectLists.DeleteOneAsync(filterSelectList);

        }

        public async Task<Property> AddProperty(string formId, Property property)
        {
            var update = Builders<MyForm>.Update.AddToSet(p => p.Items, property);
            await _context.Forms.UpdateOneAsync(w => w.Id == formId, update);


            var res = await _context.Forms.Find(w => w.Id == formId).FirstOrDefaultAsync();

            return res.Items.Where(w => w.Key == property.Key).FirstOrDefault();
        }

        public async Task<Property> UpdateProperty(string formId, Property property)
        {
            var filter = Builders<MyForm>.Filter.Where(x => x.Id == formId && x.Items.Any(i => i.Key == property.Key));
            var update = Builders<MyForm>.Update.Set(w => w.Items.ElementAt(-1), property);
            await _context.Forms.UpdateOneAsync(filter, update);

            var res = await _context.Forms.Find(w => w.Id == formId).FirstOrDefaultAsync();

            return res.Items.SingleOrDefault(w => w.Key == property.Key);
        }

        public async Task UpdateSelectList(string formId, string propertyKey, IEnumerable<DropDownOptions> dropDown)
        {
            var first = Builders<SelectList>.Filter.Eq(w => w.FormId, formId);
            var second = Builders<SelectList>.Filter.Eq(w => w.PropertyKey, propertyKey);

            var filter = Builders<SelectList>.Filter.And(first, second);
            var update = Builders<SelectList>.Update.Set(x => x.Items, dropDown);

            await _context.SelectLists.UpdateOneAsync(filter, update);
        }

        public async Task<Record> GetLatestRecord(string formId)
        {
            var records = await _context.Records.Find(w => w.FormId == formId).ToListAsync();

            return records.LastOrDefault();
        }

        public async Task<Property> GetProperty(string formId, string key)
        {
            var result = await _context.Forms.Find(w => w.Id == formId).FirstOrDefaultAsync();

            return result.Items.Where(w => w.Key == key).FirstOrDefault();
        }
    }
}
