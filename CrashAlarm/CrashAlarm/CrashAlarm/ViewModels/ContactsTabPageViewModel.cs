using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CrashAlarm.Models;
using CrashAlarm.ViewModels;
using Xamarin.Forms;

namespace CrashAlarm.ViewModels
{
    public class ContactsTabPageViewModel : BindableBase
    {
        readonly DbRepository _db;
        public ContactsTabPageViewModel()
        {
            _db = new DbRepository();
            List<Contact> result = _db.GetAllContactsAsync().Result;
            foreach (var item in result)
            {
                Contacts.Add(item);   
            }

        }

        public ObservableCollection<Contact> Contacts { get; } = new ObservableCollection<Contact>();

        public ICommand DeleteCommand => new Command<Contact>((Contact item) =>
        { // nefunkcni
            System.Console.WriteLine($"Delete command was called on: {item.ContactName}");
        });

        public Command<Contact> RemoveCommand
        {
            get
            {
                return new Command<Contact>((Contact) => {
                    Contacts.Remove(Contact);
                });
            }
        }

        //App.Current.MainPage.DisplayAlert("Alert", "There is no item in the list", "OK");
    }
}
