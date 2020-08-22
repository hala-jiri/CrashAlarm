using System;
using CrashAlarm.Models;
using CrashAlarm.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CrashAlarm.Views
{
    public partial class ContactsTabPage : ContentPage
    {
        public ContactsTabPage()
        {
            InitializeComponent();
            BindingContext = new ContactsTabPageViewModel();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            collectionView.ItemsSource = await App.DbRepository.GetAllContactsAsync();
        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameEntry.Text) && !string.IsNullOrWhiteSpace(NumberEntry.Text))
            {
                int selectedTypeIndex;
                if (pickerTypeOfContact.SelectedIndex == -1)
                    selectedTypeIndex = 0;
                else
                    selectedTypeIndex = pickerTypeOfContact.SelectedIndex;

                var pickedItemString = pickerTypeOfContact.ItemsSource[selectedTypeIndex].ToString();
                string  pickedTypeOfContactFileName;
                switch (pickedItemString)
                {
                    case "No specific":
                        pickedTypeOfContactFileName = "personAdd.png";
                        break;

                    case "Emergency":
                        pickedTypeOfContactFileName = "localHospital.png";
                        break;

                    case "Family":
                        pickedTypeOfContactFileName = "familyRestroom.png";
                        break;

                    case "Friends":
                        pickedTypeOfContactFileName = "groups.png";
                        break;

                    default:
                        pickedTypeOfContactFileName = "personAdd.png";
                        break;
                }


                await App.DbRepository.SaveContactAsync(new Contact
                {
                    ContactName = NameEntry.Text,
                    ContactNumber = NumberEntry.Text,
                    TypeOfContact = pickedTypeOfContactFileName
                });

                NameEntry.Text = NumberEntry.Text = string.Empty;
                collectionView.ItemsSource = await App.DbRepository.GetAllContactsAsync();
            }
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var menuItem = ((MenuItem) sender);
            bool confimResult = false;

            //    bool r = await App.Current.MainPage.DisplayAlert("Delete confirm", "Delete item?", "ok", "cancel");



            //Device.BeginInvokeOnMainThread(async()=>
            //{
            //    confimResult = await DisplayAlert("Delete confirm", "Delete item?", "ok", "cancel");
            //    if (confimResult)
            //    {
            //        int result = await App.DbRepository.DeleteContactAsync((Contact)menuItem.BindingContext);
            //    }
            //});
            int result = await App.DbRepository.DeleteContactAsync((Contact) menuItem.BindingContext);
            collectionView.ItemsSource = await App.DbRepository.GetAllContactsAsync();
        }
    }

}
