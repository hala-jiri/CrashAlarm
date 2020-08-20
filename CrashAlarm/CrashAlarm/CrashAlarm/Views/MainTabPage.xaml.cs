using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using CrashAlarm.Models;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace CrashAlarm.Views
{
    public partial class MainTabPage : ContentPage
    {
        private Location location;


        public MainTabPage()
        {
            InitializeComponent();
            getLocationAsync();
            

        }

        async void clickedOnHelpButton(object sender, EventArgs e)
        {
            if (location != null)
            {
                var setting = await App.DbRepository.GetSettingsAsync();

                List<Contact> contactList = new List<Contact>();
                List<string> contactNumbers = new List<string>();

                if (setting.GSMNotificationToFriends)
                    contactList.AddRange(await App.DbRepository.GetFriendContactsAsync());

                if (setting.GSMNotificationToEmergency)
                    contactList.AddRange(await App.DbRepository.GetEmergencyContactsAsync());

                var contactListCount = contactList.Count;

                contactNumbers.AddRange(contactList.Select(x=>x.ContactNumber));

                string messageToSend =
                    setting.Units + $" My Location (Lon: {location.Longitude}, Lat: {location.Latitude}";


                await SendSms(messageToSend, contactNumbers.ToArray());

               

            }
            // check location
            // check settings
            // check contact list
            // send all messages

            // show notification
        }

        public async Task SendSms(string messageText, string[] recipients)
        {
            try
            {
                var message = new SmsMessage(messageText, recipients);
                await Sms.ComposeAsync(message);
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

        async void getLocationAsync()
        {
            try
            {
                //var accessStatus = GeolocationRequest;

                location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    latLabel.Text = location.Latitude.ToString();
                    longLabel.Text = location.Longitude.ToString();
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
    }
}
