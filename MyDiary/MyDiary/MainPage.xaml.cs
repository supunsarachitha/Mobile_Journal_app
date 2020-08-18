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


        List<string> Colors = new List<string>();
        public MainPage()
        {
            InitializeComponent();
            

            Colors.Add(Color.MediumAquamarine.ToHex().ToString());
            Colors.Add(Color.MediumBlue.ToHex().ToString());
            Colors.Add(Color.MediumOrchid.ToHex().ToString());
            Colors.Add(Color.MediumPurple.ToHex().ToString());
            Colors.Add(Color.MediumSeaGreen.ToHex().ToString());
            Colors.Add(Color.MediumSlateBlue.ToHex().ToString());
            Colors.Add(Color.MediumSpringGreen.ToHex().ToString());
            Colors.Add(Color.MediumTurquoise.ToHex().ToString());
            Colors.Add(Color.MediumVioletRed.ToHex().ToString());
            Colors.Add(Color.DarkMagenta.ToHex().ToString());
            Colors.Add(Color.DarkOliveGreen.ToHex().ToString());
            Colors.Add(Color.DarkOrange.ToHex().ToString());
            Colors.Add(Color.DarkOrchid.ToHex().ToString());
            Colors.Add(Color.DarkRed.ToHex().ToString());
            Colors.Add(Color.DarkSalmon.ToHex().ToString());
            Colors.Add(Color.HotPink.ToHex().ToString());
            Colors.Add(Color.Red.ToHex().ToString());
            Colors.Add(Color.Green.ToHex().ToString());
            Colors.Add(Color.Blue.ToHex().ToString());
            Colors.Add(Color.BlueViolet.ToHex().ToString());




            


            busyIndi.IsVisible = false;



        }

        private async void BindData()
        {
            //pageList.ItemsSource = await App.Database.GetItemsAsync();

            List<PageItems> items = await App.Database.GetItemsAsync();
            Random rnd = new Random();
            foreach (var item in items)
            {
                item.HeadingColor = Colors[rnd.Next(Colors.Count)] ;
            }
            
            pageList.ItemsSource = items;

            pageList.SelectionChanged += PageList_SelectionChanged;

        }

        private void PageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pageList.SelectedItem != null)
            {
                if (addnewclicked)
                {
                    return;
                }

                addnewclicked = true;
                busyIndi.IsVisible = true;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    
                    PageItems a = (PageItems)e.CurrentSelection.FirstOrDefault();

                    await Navigation.PushAsync(new NewPage(a.ID));
                    pageList.SelectedItem = null;
                    
                });
                
            }
            
        }


        bool addnewclicked = false;
        private async void btnAddNew_Clicked(object sender, EventArgs e)
        {

            if (addnewclicked)
            {
                return;
            }

            addnewclicked = true;


            Device.BeginInvokeOnMainThread(async () =>
            {
                busyIndi.IsVisible = true;
                await Navigation.PushAsync(new NewPage(0));
                
            });
            
        }



        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            busyIndi.IsVisible = false;
            
            return false;
        }

        protected override void OnAppearing()
        {



            

            busyIndi.IsVisible = false;
            addnewclicked = false;
            
            BindData();

            if (Preferences.Get("FirstTime", true))
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    AnimationView.Animation = "3152-star-success.json";
                    busyIndi.IsVisible = true;
                    Preferences.Set("FirstTime", false);
                    await Task.Delay(3000);
                    busyIndi.IsVisible = false;


                    PageItems items = new PageItems
                    {
                        Title = "Dear Diary Sample",
                        Content = MyDiary.Data.initData.sampleContent,
                        DateTime = DateTime.Today

                    };


                    var a = await App.Database.SaveItemAsync(items);
                    BindData();
                    AnimationView.Animation = "4887-book.json";
                });

            }


            base.OnAppearing();
        }


    }
}
