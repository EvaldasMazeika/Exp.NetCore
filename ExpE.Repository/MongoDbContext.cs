using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using ExpE.Domain;
using ExpE.Domain.Models;
using ExpE.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ExpE.Repository
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly IConfiguration _configuration;

        public MongoDbContext(IConfiguration configuration)
        {
            _configuration = configuration;

            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(_configuration["ConnectionString"]));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var client = new MongoClient(settings);
            if (client != null)
                _database = client.GetDatabase("expensesDB");
        }

        public IMongoCollection<MyForm> Forms => _database.GetCollection<MyForm>("forms");

        public IMongoCollection<Record> Records => _database.GetCollection<Record>("records");

        public IMongoCollection<AutoComplete> AutoCompletes => _database.GetCollection<AutoComplete>("autoCompletes");

        public IMongoCollection<SelectList> SelectLists => _database.GetCollection<SelectList>("selectList");
    }
}
