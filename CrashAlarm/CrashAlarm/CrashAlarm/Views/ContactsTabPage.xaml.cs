using System;
using CrashAlarm.Models;
using Xamarin.Forms;

namespace CrashAlarm.Views
{
    public partial class ContactsTabPage : ContentPage
    {
        public ContactsTabPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await App.DbRepository.GetAllContactsAsync();
        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameEntry.Text) && !string.IsNullOrWhiteSpace(NumberEntry.Text))
            {
                await App.DbRepository.SaveContactAsync(new Contact
                {
                    ContactName = NameEntry.Text,
                    ContactNumber = NumberEntry.Text,
                    TypeOfContact = "Friend"
                });

                NameEntry.Text = NumberEntry.Text = string.Empty;
                listView.ItemsSource = await App.DbRepository.GetAllContactsAsync();
            }
        }
    }

}
