using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyDiary.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginBackground : ContentPage
    {
        public LoginBackground()
        {
            InitializeComponent();


            if (Preferences.Get("fingerPrint", false))
            {

                Device.BeginInvokeOnMainThread(async () =>
                {

                    bool isFingerprintAvailable = await CrossFingerprint.Current.IsAvailableAsync(false);
                    if (!isFingerprintAvailable)
                    {
                        return;
                    }

                    AuthenticationRequestConfiguration conf = new AuthenticationRequestConfiguration("Authentication", "Authenticate access");

                    var authResult = await CrossFingerprint.Current.AuthenticateAsync(conf);
                    if (authResult.Authenticated)
                    {
                        //Success  
                        //await DisplayAlert("Success", "Authentication succeeded", "OK");

                        await Navigation.PushAsync(new MainPage(), true);
                        //await Navigation.PopPopupAsync();

                    }
                    else
                    {
                        await DisplayAlert("Error", "Authentication failed", "OK");
                    }
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushPopupAsync(new Login(), true);
                });
            }

            
        }
    }
}