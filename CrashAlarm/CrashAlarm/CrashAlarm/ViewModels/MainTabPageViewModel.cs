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
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using Prism.AppModel;

namespace CrashAlarm.ViewModels
{
    public class MainTabPageViewModel : BindableBase
    {
        private Location _location;
        private string _longitude;
        private string _latitude;
        private string _googleMapsLink;
        private int _numberOfContacts;

        public MainTabPageViewModel()
        {
            GoogleMapsLink = "";
            getLocationAsync();
            SendSMSCommand = new Command(async () => await CallForHelp());

        }
        public string GoogleMapsLink
        {
            get => _googleMapsLink;
            set
            {
                _googleMapsLink = value;
                RaisePropertyChanged(nameof(GoogleMapsLink));
            }
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

                    GoogleMapsLink =  $"https://www.google.com/maps/search/?api=1&query={Latitude},{Longitude}";
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

                List<Models.Contact> contactList = new List<Models.Contact>();
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
                
                string messageToSend = setting.HelpMessage + $" My Location (Lon: {_location.Longitude}, Lat: {_location.Latitude}), {GoogleMapsLink}";
                if(Device.RuntimePlatform == RuntimePlatform.UWP.ToString())
                {
                    // hlaska ze nejde poslat
                    UserDialogs.Instance.Toast("nejdou poslat", TimeSpan.FromSeconds(5));
                }
                else
                {
                    await SendSms(messageToSend, contactNumbers.ToArray());
                    string toastMessage = "Your messages were send, number of messages: " + contactNumbers.Count.ToString();
                    UserDialogs.Instance.Toast(toastMessage, TimeSpan.FromSeconds(5));
                }

                await SendTestEmail();

                //var kuku = AppResource.ToastSendMessage;
                
                
                //AppResource.Longitude
                
                
                //UserDialogs.Instance.Toast("Text", TimeSpan.FromSeconds(3));
                //UserDialogs.Instance.Toast(new ToastConfig("text") {BackgroundColor = Color.Pink});

                //var res = await UserDialogs.Instance.ConfirmAsync("zprava", "TItle", "ok", "nic");

            }
        }



        public ICommand OpenMapCommand => new Command<string>(async (url) =>
        {
            try
            {
                if (!string.IsNullOrEmpty(url))
                        await Browser.OpenAsync(new Uri(url), BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Prolem with open URL");
            }

        });



        static async Task SendTestEmail()
        {
            try
            {
                MailjetClient client = new MailjetClient("73f02d38e2aa5a178e4d23872938747d", "578abcbb2700e8081784749af4cd8302")
                {
                    Version = ApiVersion.V3_1,
                };
                MailjetRequest request = new MailjetRequest { Resource = Send.Resource, }
                    .Property(Send.Messages, new JArray {
                    new JObject {
                        {
                            "From",
                            new JObject {
                                {"Email", "hala.jiri@gmail.com"},
                                {"Name", "Jiri"}
                            }
                        }, {
                            "To",
                            new JArray {
                                new JObject {
                                    {
                                        "Email",
                                        "hala.jiri@gmail.com"
                                    }, {
                                        "Name",
                                        "Jiri"
                                    }
                                }
                            }
                        }, {
                            "Subject",
                            "Greetings from Mailjet."
                        }, {
                            "TextPart",
                            "My first Mailjet email"
                        }, {
                            "HTMLPart",
                            "<h3>Dear passenger 1, welcome to <a href='https://www.mailjet.com/'>Mailjet</a>!</h3><br />May the delivery force be with you!"
                        }, {
                            "CustomID",
                            "AppGettingStartedTest"
                        }
                    }
                    });
                MailjetResponse response = await client.PostAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                    Console.WriteLine(response.GetData());
                }
                else
                {
                    Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                    Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                    Console.WriteLine(response.GetData());
                    Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }


    }
}
