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
    public partial class StartupPage : ContentPage
    {
        bool isFingerprintAvailable = false;
        public StartupPage()
        {
            InitializeComponent();

            Device.BeginInvokeOnMainThread(async () =>
            {
                isFingerprintAvailable = await CrossFingerprint.Current.IsAvailableAsync(false);

            });
        }

        private void btnNext_Clicked(object sender, EventArgs e)
        {
            if (isFingerprintAvailable)
            {
                if (startupCarousel.Position == 4)
                {
                    Navigation.InsertPageBefore(new MainPage(), this);
                    Navigation.PopAsync();
                    return;
                }

            }
            else
            {
                if (startupCarousel.Position == 3)
                {
                    Navigation.InsertPageBefore(new MainPage(), this);
                    Navigation.PopAsync();
                    return;
                }
            }


            startupCarousel.Position = startupCarousel.Position + 1;



            if (isFingerprintAvailable)
            {
                if (startupCarousel.Position == 4)
                {
                    btnNext.Text = "Finish";
                }
                else
                {
                    btnNext.Text = "Next";
                }
            }
            else
            {
                if (startupCarousel.Position == 3)
                {
                    btnNext.Text = "Finish";
                }
                else
                {
                    btnNext.Text = "Next";
                }
            }

        }


        private void btnOption_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (startupCarousel.Position == 2)
                {

                    await Navigation.PushPopupAsync(new PasswordPopup());


                }
                else if (startupCarousel.Position == 3 && isFingerprintAvailable)
                {
                    AuthenticationRequestConfiguration conf = new AuthenticationRequestConfiguration("Authentication", "Authenticate access");

                    var authResult = await CrossFingerprint.Current.AuthenticateAsync(conf);
                    if (authResult.Authenticated)
                    {
                        //Success  
                        await DisplayAlert("Success", "Authentication succeeded", "OK");
                        Preferences.Set("fingerPrint", true);
                    }
                    else
                    {
                        await DisplayAlert("Error", "Authentication failed", "OK");
                    }
                }
            });
        }
    }
}