using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using CrashAlarm.Models;
using CrashAlarm.Resources;
using ImTools;
using Plugin.Messaging;
using Xamarin.Essentials;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace CrashAlarm.ViewModels
{
    public class MainTabPageViewModel : BindableBase
    {
        private Location _location;
        private string _longitude;
        private string _latitude;
        private int _numberOfContacts;

        public MainTabPageViewModel()
        {
            getLocationAsync();
            SendSMSCommand = new Command(async () => await CallForHelp());

        }

        public string Longitude
        {
            get => _longitude;
            set
            {
                _longitude = value;
                RaisePropertyChanged(nameof(Longitude));
            }
        }

        public string Latitude
        {
            get => _latitude;
            set
            {
                _latitude = value;
                RaisePropertyChanged(nameof(Latitude));
            }
        }
        public int NumberOfContacts
        {
            get => _numberOfContacts;
            set
            {
                _numberOfContacts = value;
                RaisePropertyChanged(nameof(NumberOfContacts));
            }
        }

        private async void getLocationAsync()
        {
            try
            {
                _location = await Geolocation.GetLastKnownLocationAsync();

                if (_location != null)
                {
                    Longitude = _location.Longitude.ToString();
                    Latitude = _location.Latitude.ToString();

                    Console.WriteLine($"Latitude: {_location.Latitude}, Longitude: {_location.Longitude}, Altitude: {_location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {

            }
            catch (FeatureNotEnabledException fneEx)
            {

            }
            catch (PermissionException pEx)
            {

            }
            catch (Exception ex)
            {

            }
        }

        public ICommand SendSMSCommand { get; set; }

        private async Task SendSms(string messageText, string[] recipients)
        {
            try
            {
                var smsMessenger = CrossMessaging.Current.SmsMessenger;
                if (smsMessenger.CanSendSms)
                {

                    //var message = new SmsMessage(messageText, recipients);
                    //await Sms.ComposeAsync(message);

                    foreach (var recipient in recipients)
                    {
                        //smsMessenger = 
                        smsMessenger.SendSmsInBackground(recipient, messageText);
                    }
                }
                //var message = new SmsMessage(messageText, recipients);
                //await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
        private async Task CallForHelp()
        {
            PermissionService _permissionService = new PermissionService();
            
            var stat = _permissionService.CheckAndRequestSMSPermission();
            if (stat.Result != PermissionStatus.Granted)
                return;

            if (_location != null)
            {
                var setting = App.DbRepository.GetSettingsAsync().Result;

                List<Contact> contactList = new List<Contact>();
                List<string> contactNumbers = new List<string>();


                if (setting.GSMNotificationToFriends)
                    contactList.AddRange(await App.DbRepository.GetFriendContactsAsync());

                if (setting.GSMNotificationToFamily)
                    contactList.AddRange(await App.DbRepository.GetFamilyContactsAsync());

                if (setting.GSMNotificationToEmergency)
                    contactList.AddRange(await App.DbRepository.GetEmergencyContactsAsync());

                NumberOfContacts = contactList.Count;

                if (NumberOfContacts == 0)
                {
                    UserDialogs.Instance.Toast("You dont have any contacts available.", TimeSpan.FromSeconds(5));
                    return;
                }

                contactNumbers.AddRange(contactList.Select(x => x.ContactNumber));
                string googleMapsUrlLink = $"https://www.google.com/maps/search/?api=1&query={Latitude},{Longitude}";
                string messageToSend =
                    setting.HelpMessage + $" My Location (Lon: {_location.Longitude}, Lat: {_location.Latitude}), {googleMapsUrlLink}";

                await SendSms(messageToSend, contactNumbers.ToArray());

                //var kuku = AppResource.ToastSendMessage;
                string toastMessage = "Your messages were send, number of messages: " + contactNumbers.Count.ToString();
                 UserDialogs.Instance.Toast(toastMessage, TimeSpan.FromSeconds(5));
                //UserDialogs.Instance.Toast("Text", TimeSpan.FromSeconds(3));
                //UserDialogs.Instance.Toast(new ToastConfig("text") {BackgroundColor = Color.Pink});

                //var res = await UserDialogs.Instance.ConfirmAsync("zprava", "TItle", "ok", "nic");

            }
        }

    }
}
