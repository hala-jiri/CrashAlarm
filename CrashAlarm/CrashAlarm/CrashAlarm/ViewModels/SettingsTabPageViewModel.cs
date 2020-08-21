using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using CrashAlarm.Models;

namespace CrashAlarm.ViewModels
{
    public class SettingsTabPageViewModel : BindableBase
    {
        readonly DbRepository _db;

        public SettingsTabPageViewModel()
        {
            _db = new DbRepository();
            //GetAllSettings();
            GetPreferenceSettings();
        }
        
        private Settings settings;
        public Settings Settings
        {
            get { return settings; }
            set
            {
                if (settings != value)
                {
                    settings = value;
                    RaisePropertyChanged(nameof(Settings));
                    _db.SaveSettingsAsync(settings);
                }
            }
        }


        //public async Task GetAllSettings()
        //{
        //    await SaveExample();
        //    GetPreferenceSettings();
        //}

        private void GetPreferenceSettings()
        {
            this.Settings = _db.GetSettingsAsync().Result;
        }
        //private async Task SaveExample()
        //{
        //    await _db.SaveSettingsAsync(new Settings
        //    {
        //        Accuracy = "High", GSMNotificationToEmergency = true, GSMNotificationToFriends = true,
        //        LastUpdate = DateTime.Now, Units = "Mph"
        //    });
        //}
        //private async Task GetRemoteConferences()
        //{
        //    var remoteClient = new TekConfClient();
        //    var conferences = await remoteClient.GetConferences().ConfigureAwait(false);
        //    await _db.SaveAll(conferences).ConfigureAwait(false);
        //}
    }
}
