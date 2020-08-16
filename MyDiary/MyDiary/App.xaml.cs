using MyDiary.Data;
using MyDiary.View;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MyDiary
{
    public partial class App : Application
    {
        
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzAzMTgzQDMxMzgyZTMyMmUzMEdPRnFWVDlPcnlEUUJKK2txSEVQTXowNFNkcmwwNUo4eElXZDRzYmFSNDg9");
            InitializeComponent();


            if (Preferences.Get("FirstTime", true))
            {
                MainPage = new NavigationPage(new StartupPage());
            }
            else if(Preferences.ContainsKey("loginPin") || Preferences.ContainsKey("fingerPrint"))
            {
                MainPage = new NavigationPage(new LoginBackground());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
                

            //MainPage = new StartupPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        static DiaryDatabase database;
        public static DiaryDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new DiaryDatabase();
                }
                return database;
            }
        }
    }
}
