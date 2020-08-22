using System;
using System.Collections.Generic;
using System.Text;
using CrashAlarm.Models;
using Xamarin.Forms;

namespace CrashAlarm
{
    class ContactTypeItemTemplateSelector: DataTemplateSelector
    {

        public DataTemplate FamilyTemplate { get; set; }
        public DataTemplate FriendsTemplate { get; set; }
        public DataTemplate EmergencyTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (((Contact) item).TypeOfContact == "Family")
                return FamilyTemplate;
            if (((Contact)item).TypeOfContact == "Friends")
                return FriendsTemplate;
            if (((Contact)item).TypeOfContact == "Emergency")
                return EmergencyTemplate;

            return EmergencyTemplate;

        }
    }
}
