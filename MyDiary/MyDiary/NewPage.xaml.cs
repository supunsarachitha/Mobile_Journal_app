using MyDiary.Data;
using MyDiary.Model;
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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyDiary
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPage : ContentPage
    {

        int PageId;
        public NewPage(int PId)
        {
            PageId = PId;
            InitializeComponent();
            busyIndi.IsVisible = true;



        }

        private  void getData(int PId)
        {

            Device.BeginInvokeOnMainThread(async() =>
            {
                busyIndi.IsVisible = true;
                lbldate.Text = DateTime.Today.ToString("dddd, dd MMMM yyyy");
                var list = await App.Database.GetItemAsync(PId);

                    Entertitle.Text = list.Title;
                    rte.Text = list.Content;
                    lbldate.Text = list.DateTime.ToString();
                    PageId = list.ID;
                await Task.Delay(4000);
                busyIndi.IsVisible = false;
            });
            
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new MainPage(), this);
            await Navigation.PopAsync();
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            rte.Unfocus();
            
            AnimationView.IsPlaying = false;
            AnimationView.Animation = "890-loading-animation.json";
            AnimationView.Loop = false;

            busyIndi.IsVisible = true;
            AnimationView.IsPlaying = true;


            string finalResult =rte.GetHtmlString();

            PageItems items = new PageItems
            {
                Title = Entertitle.Text,
                Content = finalResult,
                
                
            };

            if (PageId > 0)
            {
                items.ID = PageId;
            }
            else
            {
                items.DateTime = DateTime.Today;
              
            }

            var a = await App.Database.SaveItemAsync(items);
            PageId = items.ID;
            await Task.Delay(3000);
            AnimationView.IsPlaying = false;
            AnimationView.Animation = "3152-star-success.json";
            await Task.Delay(3000);
            AnimationView.IsPlaying = true;

            mainLayout.IsEnabled = false;
            btnDelete.IsVisible = true;
            btnSave.IsVisible = false;
            btnEdit.IsVisible = true;

            busyIndi.IsVisible = false;
            

        }

        

        private void rte_Focused(object sender, EventArgs e)
        {

        }

        private void rte_Unfocused(object sender, EventArgs e)
        {

        }

        private void rte_TextChanged(object sender, Syncfusion.XForms.RichTextEditor.TextChangedEventArgs e)
        {

        }


        protected override void OnAppearing()
        {

            if (PageId > 0)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    
                    
                    var list = await App.Database.GetItemAsync(PageId);

                    Entertitle.Text = list.Title;
                    rte.Text = list.Content;
                    lbldate.Text = list.DateTime.ToString();
                    PageId = list.ID;
                    await Task.Delay(5000);
                    busyIndi.IsVisible = false;

                    btnEdit.IsVisible = true;
                    btnSave.IsVisible = false;
                    btnDelete.IsVisible = true;
                    mainLayout.IsEnabled = false;
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    lbldate.Text = DateTime.Today.ToString("dddd dd MMMM yyyy");
                    await Task.Delay(3000);
                    busyIndi.IsVisible = false;
                    btnSave.IsVisible = true;
                    btnDelete.IsVisible = false;
                    mainLayout.IsEnabled = true;
                    btnEdit.IsVisible = false;
                });
               
            }

                

            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.InsertPageBefore(new MainPage(), this);
            Navigation.PopAsync();

            return base.OnBackButtonPressed();
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {

            bool res = await DisplayAlert("Warning", "Confirm Delete?", "No", "Yes");
            if (!res)
            {
                await App.Database.DeleteItemByIDAsync(PageId);
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();

            }


        }

        private void btnEdit_Clicked(object sender, EventArgs e)
        {
            mainLayout.IsEnabled = true;
            btnDelete.IsVisible = false;
            btnEdit.IsVisible = false;
            btnSave.IsVisible = true;

        }
    }


}