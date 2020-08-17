using System;
using System.IO;
using Prism;
using Prism.Ioc;
using CrashAlarm.ViewModels;
using CrashAlarm.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;

namespace CrashAlarm
{
    public partial class App
    {
        private static DbRepository _db;

        public static DbRepository DbRepository
        {
            get
            {
                if(_db == null)
                    _db = new DbRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"CrashAlarm.db3"));
                return _db;
            }
        }
        

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainTabbedPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<MainTabbedPage, MainTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsTabPage, SettingsTabPageViewModel>();
            containerRegistry.RegisterForNavigation<MainTabPage, MainTabPageViewModel>();
            containerRegistry.RegisterForNavigation<HistoryTabPage, HistoryTabPageViewModel>();
            containerRegistry.RegisterForNavigation<AboutTabPage, AboutTabPageViewModel>();
            containerRegistry.RegisterForNavigation<ContactsTabPage, ContactsTabPageViewModel>();
        }
    }
}
