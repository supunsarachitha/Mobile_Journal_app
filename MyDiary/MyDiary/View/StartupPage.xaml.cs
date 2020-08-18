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



            MessagingCenter.Subscribe<PasswordPopup>(this, "Password", (sender) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Delay(1000);
                    NextFunction();
                });
                
            });
        }

        private void btnNext_Clicked(object sender, EventArgs e)
        {
            NextFunction();

        }

        private async void NextFunction()
        {
            if (isFingerprintAvailable)
            {
                if (startupCarousel.Position == 4)
                {
                    Navigation.InsertPageBefore(new MainPage(), this);
                    await Navigation.PopAsync();
                    return;
                }

            }
            else
            {
                if (startupCarousel.Position == 3)
                {
                    Navigation.InsertPageBefore(new MainPage(), this);
                    await Navigation.PopAsync();
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
                        await DisplayAlert("Success", "Next time use fingerprint to unlock journal", "OK");
                        Preferences.Set("fingerPrint", true);
                        NextFunction();
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