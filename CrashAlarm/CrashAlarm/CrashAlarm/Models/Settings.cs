using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CrashAlarm.Models
{
    public class Settings
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Units { get; set; }
        public string Accuracy { get; set; }
        public bool GSMNotificationToEmergency { get; set; }
        public bool GSMNotificationToFriends { get; set; }


    }
}
