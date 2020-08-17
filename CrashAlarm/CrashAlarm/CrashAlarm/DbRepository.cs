using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CrashAlarm.Models;
using SQLite;

namespace CrashAlarm
{
    public class DbRepository
    {
        readonly SQLiteAsyncConnection _database;

        public DbRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Contact>().Wait();
            _database.CreateTableAsync<Settings>().Wait();
        }

        public Task<List<Contact>> GetAllContactsAsync()
        {
            return _database.Table<Contact>().ToListAsync();
        }

        public Task<int> SaveContactAsync(Contact _contact)
        {
            return _database.InsertAsync(_contact);
        }

    }
}
