using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CrashAlarm.Models
{
    public class Contact
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string TypeOfContact { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
    }
}
