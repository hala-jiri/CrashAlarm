using System;
using CrashAlarm.Models;
using CrashAlarm.ViewModels;
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
            listView.ItemsSource = await App.DbRepository.GetAllContactsAsync();
            //listViewFirst.ItemsSource = await App.DbRepository.GetAllContactsAsync();
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

                await App.DbRepository.SaveContactAsync(new Contact
                {
                    ContactName = NameEntry.Text,
                    ContactNumber = NumberEntry.Text,
                    TypeOfContact = pickerTypeOfContact.ItemsSource[selectedTypeIndex].ToString()
                });

                NameEntry.Text = NumberEntry.Text = string.Empty;
                listView.ItemsSource = await App.DbRepository.GetAllContactsAsync();
            }
        }
        public void OnDelete(object sender, EventArgs e)
        {
            var menuItem = ((MenuItem)sender);
            //DisplayAlert("Delete Context Action", menuItem.CommandParameter + " delete context action", "OK");
            var result = App.DbRepository.DeleteContactAsync((Contact)menuItem.BindingContext);
            
        }
        public void Remove_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var contactItem = button.BindingContext as Contact;
            var vm = BindingContext as ContactsTabPageViewModel;
            vm.RemoveCommand.Execute(contactItem);
        }
    }

}
