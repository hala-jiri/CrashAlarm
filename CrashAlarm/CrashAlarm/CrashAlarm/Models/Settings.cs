using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CrashAlarm.Models
{
    public class Settings
    {
        private bool _gsmNotificationToEmergency;
        private bool _gsmNotificationToFriends;
        private bool _gsmNotificationToFamily;
        private string _helpMessage;
        private DateTime _lastUpdate;

       [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime LastUpdate 
        {
            get => _lastUpdate;
            set
            {
                if (_lastUpdate != value)
                {
                    _lastUpdate = value;
                    App.DbRepository.SaveSettingsAsync(this);
                }
            }
        }
        public string HelpMessage
        {
            get => _helpMessage;
            set
            {
                if (_helpMessage != value)
                {
                    _helpMessage = value;
                    App.DbRepository.SaveSettingsAsync(this);
                }
            }
        }
       
        public bool GSMNotificationToEmergency
        {
            get => _gsmNotificationToEmergency;
            set
            { 
                if (_gsmNotificationToEmergency != value)
                {
                    _gsmNotificationToEmergency = value;
                    App.DbRepository.SaveSettingsAsync(this);
                }
            }
        }

        public bool GSMNotificationToFriends
        {
            get => _gsmNotificationToFriends;
            set
            {
                if (_gsmNotificationToFriends != value)
                {
                    _gsmNotificationToFriends = value;
                    App.DbRepository.SaveSettingsAsync(this);
                }
            }
        }

        public bool GSMNotificationToFamily
        {
            get => _gsmNotificationToFamily;
            set
            {
                if (_gsmNotificationToFamily != value)
                {
                    _gsmNotificationToFamily = value;
                    App.DbRepository.SaveSettingsAsync(this);
                }
            }
        }
    }
}
