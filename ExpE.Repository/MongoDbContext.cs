using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpE.Domain;
using ExpE.Domain.Models;
using ExpE.Repository.Interfaces;
using MongoDB.Driver;

namespace ExpE.Repository
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            if (client != null)
                _database = client.GetDatabase("expensesDB");
        }

        public IMongoCollection<MyForm> Forms => _database.GetCollection<MyForm>("forms");

        public IMongoCollection<Record> Records => _database.GetCollection<Record>("records");

    }
}
