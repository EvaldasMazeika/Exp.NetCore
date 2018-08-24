using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpE.Domain;
using ExpE.Domain.Models;
using MongoDB.Driver;

namespace ExpE.Repository.Interfaces
{
    public interface IMongoDbContext
    {
        IMongoCollection<MyForm> Forms { get; }
        IMongoCollection<Record> Records { get; }
        IMongoCollection<AutoComplete> AutoCompletes { get; }
        IMongoCollection<SelectList> SelectLists { get; }
    }
}
