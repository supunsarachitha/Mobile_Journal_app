using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
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
    public partial class Login : PopupPage
    {
        public Login()
        {
            InitializeComponent();


            
            

            


        }

        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                if (Preferences.Get("loginPin", "") == txtPassword.Text)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        
                        await Navigation.PushAsync(new MainPage(), true);
                        
                        await Navigation.PopPopupAsync();

                        
                        
                    });

                }
                else
                {
                    DisplayAlert("", "invalid Password", "Ok");
                }
            }
        }

        private void PopupPage_BackgroundClicked(object sender, EventArgs e)
        {
            return;
        }


    }
}