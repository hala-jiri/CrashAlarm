using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CrashAlarm.Models
{
    public class Contact
    {
        private string _typeOfContact;
        private string _contactName;
        private string _contactNumber;

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string TypeOfContact 
        {
            get => _typeOfContact;
            set
            {
                if (_typeOfContact != value)
                {
                    _typeOfContact = value;
                    //App.DbRepository.SaveContactAsync(this);
                }
            }
        }
        

        public string ContactName
        {
            get => _contactName;
            set
            {
                if (_contactName != value)
                {
                    _contactName = value;
                    //App.DbRepository.SaveContactAsync(this);
                }
            }
        }

        public string ContactNumber
        {
            get => _contactNumber;
            set
            {
                if (_contactNumber != value)
                {
                    _contactNumber = value;
                    //App.DbRepository.SaveContactAsync(this);
                }
            }
        }
    }
}
