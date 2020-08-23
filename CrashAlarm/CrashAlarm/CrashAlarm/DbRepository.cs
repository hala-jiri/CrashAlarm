using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CrashAlarm.Models;
using SQLite;

namespace CrashAlarm
{
    public class DbRepository
    {
        readonly SQLiteAsyncConnection _database;

        private readonly string _dbPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CrashAlarm.db3");

        public DbRepository()
        {
            _database = new SQLiteAsyncConnection(_dbPath);
            //_database.DropTableAsync<Contact>().Wait();
            //_database.DropTableAsync<Settings>().Wait();

            if (!dbTableExist("Contact"))
            { 
                _database.CreateTableAsync<Contact>().Wait();
                _database.InsertAsync(new Contact()
                {
                    ContactNumber = "+42077998855",
                    ContactName = "Jirka",
                    TypeOfContact = "familyRestroom.png"
                }).Wait();

            }

            if (!dbTableExist("Settings"))
            {
                _database.CreateTableAsync<Settings>().Wait();
                _database.InsertAsync(new Settings()
                {
                    GSMNotificationToFriends = true,
                    GSMNotificationToEmergency = true,
                    GSMNotificationToFamily = false,
                    HelpMessage = "Pomoc, ztratil jsem se, Jirka.",
                    LastUpdate = DateTime.Now
                }).Wait();
            }
            else
            {
                if (this.GetCountOfSettingsAsync().Result != 1)
                {
                    this._database.DropTableAsync<Settings>().Wait();
                    this._database.CreateTableAsync<Settings>().Wait();
                    _database.InsertAsync(new Settings()
                    {
                        GSMNotificationToFriends = true,
                        GSMNotificationToEmergency = true,
                        GSMNotificationToFamily = false,
                        HelpMessage = "Pomoc, ztratil jsem se, Jirka.",
                        LastUpdate = DateTime.Now
                    }).Wait();
                }

                
            }
        }

        private bool dbTableExist(string tableName)
        {
            var connect = _database.GetConnection();
            string cmdQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
            var cmd = connect.CreateCommand(cmdQuery, tableName);
            return cmd.ExecuteScalar<string>() != null;
        }

        public DbRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            //_database.CreateTableAsync<Contact>().Wait();
           //_database.CreateTableAsync<Settings>().Wait();
        }

        public Task<List<Contact>> GetAllContactsAsync()
        {
            return _database.Table<Contact>().ToListAsync();
        }

        public Task<List<Contact>> GetEmergencyContactsAsync()
        {
            return _database.Table<Contact>().Where(t=>t.TypeOfContact == "localHospital.png").ToListAsync();
        }
        public Task<List<Contact>> GetFriendContactsAsync()
        {
            return _database.Table<Contact>().Where(t => t.TypeOfContact == "groups.png").ToListAsync();
        }
        public Task<List<Contact>> GetFamilyContactsAsync()
        {
            return _database.Table<Contact>().Where(t => t.TypeOfContact == "familyRestroom.png").ToListAsync();
        }
        
        public Task<List<Contact>> GetNoSpecifiedContactsAsync()
        {
            return _database.Table<Contact>().Where(t => t.TypeOfContact == "personAdd.png").ToListAsync();
        }

        public Task<int> SaveContactAsync(Contact _contact)
        {
            return _database.InsertAsync(_contact);
        }
        public Task<int> DeleteContactAsync(Contact _contact)
        {
            return _database.DeleteAsync(_contact);
        }

        public Task<Settings> GetSettingsAsync()
        {
            return _database.Table<Settings>().FirstOrDefaultAsync();
        }
        public Task<int> GetCountOfSettingsAsync()
        {
            return _database.Table<Settings>().CountAsync();
        }

        public Task<int> InsertSettingsAsync(Settings _settings)
        {
            return _database.InsertAsync(_settings);
        }

        public Task<int> DeleteAllSettingsAsync()
        {
            return _database.ExecuteAsync("Delete from Contact");
        }

        public Task<int> SaveSettingsAsync(Settings _settings)
        {
            return _database.UpdateAsync(_settings);
        }

    }
}
