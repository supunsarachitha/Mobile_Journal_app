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
    public partial class PasswordPopup : PopupPage
    {
        public PasswordPopup()
        {
            InitializeComponent();
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {

            int i;
            

            if (!string.IsNullOrEmpty(txtPassword.Text) || !string.IsNullOrEmpty(txtPasswordRetype.Text))
            {
                if(txtPassword.Text == txtPasswordRetype.Text)
                {
                    
                        Preferences.Set("loginPin", txtPassword.Text);
                        Navigation.PopPopupAsync(true);

                }
                else
                {
                    DisplayAlert("", "Password mismatched", "Ok");
                }
            }
            else
            {
                DisplayAlert("", "invalid Password", "Ok");
            }

            
        }
    }
}