using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Provider;
using Lottie.Forms.Droid;
using Plugin.Fingerprint;

namespace MyDiary.Droid
{
    [Activity(Label = "MyDiary", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {

            CrossFingerprint.SetCurrentActivityResolver(() => this);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            Instance = this;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            AnimationViewRenderer.Init();
            LoadApplication(new App());
            Window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    Bitmap rowImage = BitmapFactory.DecodeStream((stream));
                    Bitmap resizedBitmap = rowImage; 
                    if (rowImage.Width > 500)
                    {
                        resizedBitmap = ScaleDownBitmap(rowImage, 500, true);
                    }


                    using (MemoryStream memory = new MemoryStream())
                    {
                        resizedBitmap.Compress(Bitmap.CompressFormat.Webp, 100, memory);

                        memory.Position = 0;
                        PickImageTaskCompletionSource.SetResult(memory);
                        memory.Flush();
                        
                    }

                    // Set the Stream as the completion of the Task
                    
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }



        public static Bitmap ScaleDownBitmap(Bitmap originalImage, float maxImageSize, bool filter)
        {
            float ratio = Math.Min((float)maxImageSize / originalImage.Width, (float)maxImageSize / originalImage.Height);
            int width = (int)Math.Round(ratio * (float)originalImage.Width);
            int height = (int)Math.Round(ratio * (float)originalImage.Height);

            Bitmap newBitmap = Bitmap.CreateScaledBitmap(originalImage, width, height, filter);
            return newBitmap;
        }

        public static Bitmap ScaleBitmap(Bitmap originalImage, int wantedWidth, int wantedHeight)
        {
            Bitmap output = Bitmap.CreateBitmap(wantedWidth, wantedHeight, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(output);
            Matrix m = new Matrix();
            m.SetScale((float)wantedWidth / originalImage.Width, (float)wantedHeight / originalImage.Height);
            canvas.DrawBitmap(originalImage, m, new Paint());
            return output;
        }
    }
}