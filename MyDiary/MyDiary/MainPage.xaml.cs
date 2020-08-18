using MyDiary.Model;
using MyDiary.View;
using Syncfusion.XForms.RichTextEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyDiary
{

    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            busyIndi.IsVisible = false;


            if(Preferences.Get("FirstTime", true))
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    AnimationView.Animation = "3152-star-success.json";
                    busyIndi.IsVisible = true;
                    Preferences.Set("FirstTime", false);
                    await Task.Delay(3000);
                    busyIndi.IsVisible = false;

                });
                
            }
            else
            {
               
                BindData();
            }

            
        }

        private async void BindData()
        {
            pageList.ItemsSource = await App.Database.GetItemsAsync();

            pageList.SelectionChanged += PageList_SelectionChanged;

        }

        private void PageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pageList.SelectedItem != null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    busyIndi.IsVisible = true;
                    PageItems a = (PageItems)e.CurrentSelection.FirstOrDefault();
                    
                    Navigation.InsertPageBefore(new NewPage(a.ID), this);
                    pageList.SelectedItem = null;
                    await Navigation.PopAsync();
                });
                
            }
            
        }

        private async void btnAddNew_Clicked(object sender, EventArgs e)
        {
            busyIndi.IsVisible = true;
            Device.BeginInvokeOnMainThread(async () =>
            {
                
                await Navigation.PushAsync(new NewPage(0));
            });
            
        }



        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {

            Navigation.PopAsync();
            System.Environment.Exit(0);
            return true;
        }

        protected override void OnAppearing()
        {
            busyIndi.IsVisible = false;
            base.OnAppearing();
        }


    }
}
