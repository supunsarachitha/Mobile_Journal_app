using MyDiary.Interfaces;
using Syncfusion.XForms.RichTextEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyDiary.ViewModel
{

    public class NewPageViewModel : INotifyPropertyChanged
    {
        Stream boogalooFontStream, handleeFontStream, kaushanFontStream, pinyonFontStream;
        /// <summary>
        /// Font button command property to be executed when font button is clicked
        /// </summary>
        public ICommand FontButtonCommand { get; set; }
        public ICommand ImageInsertCommand { get; set; }


        public NewPageViewModel()
        {
            Assembly assembly = typeof(NewPage).GetTypeInfo().Assembly;
            boogalooFontStream = assembly.GetManifestResourceStream("MyDiary.Fonts.Boogaloo.ttf");
            handleeFontStream = assembly.GetManifestResourceStream("MyDiary.Fonts.Handlee.ttf");
            kaushanFontStream = assembly.GetManifestResourceStream("MyDiary.Fonts.Kaushan Script.ttf");
            pinyonFontStream = assembly.GetManifestResourceStream("MyDiary.Fonts.Pinyon Script.ttf");
            FontButtonCommand = new Command<object>(LoadFonts);

            ImageInsertCommand = new Command<object>(Load);
        }

        /// <summary>
        /// Loads the fonts to the font event args
        /// </summary>
        /// <param name="obj"></param>
        void LoadFonts(object obj)
        {
            FontButtonClickedEventArgs fontEventArgs = (obj as FontButtonClickedEventArgs);
            if (!fontEventArgs.FontStreamCollection.ContainsKey("Boogaloo"))
                fontEventArgs.FontStreamCollection.Add("Boogaloo", boogalooFontStream);
            if (!fontEventArgs.FontStreamCollection.ContainsKey("Handlee"))
                fontEventArgs.FontStreamCollection.Add("Handlee", handleeFontStream);
            if (!fontEventArgs.FontStreamCollection.ContainsKey("Kaushan Script"))
                fontEventArgs.FontStreamCollection.Add("Kaushan Script", kaushanFontStream);
            if (!fontEventArgs.FontStreamCollection.ContainsKey("Pinyon Script"))
                fontEventArgs.FontStreamCollection.Add("Pinyon Script", pinyonFontStream);
        }


        /// <summary>
        /// Creates a event args for Image Insert
        /// </summary>
        void Load(object obj)
        {
            ImageInsertedEventArgs imageInsertedEventArgs = (obj as ImageInsertedEventArgs);
            this.GetImage(imageInsertedEventArgs);
        }
        /// <summary>
        /// Gets image stream from picker using dependency service.
        /// </summary>
        /// <param name="imageInsertedEventArgs">Event args to be passed for dependency service</param>
        async void GetImage(ImageInsertedEventArgs imageInsertedEventArgs)
        {
            try
            {
                Stream imageStream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
                //Syncfusion.XForms.RichTextEditor.ImageSource imageSource = new Syncfusion.XForms.RichTextEditor.ImageSource();
                //imageSource.ImageStream = imageStream;
                //imageInsertedEventArgs.ImageSourceCollection.Add(imageSource);

                if (imageStream != null)
                {



                    Syncfusion.XForms.RichTextEditor.ImageSource imageSource = new Syncfusion.XForms.RichTextEditor.ImageSource
                    {
                        ImageStream = imageStream,
                        SaveOption = ImageSaveOption.Base64,
                        Width = 200,
                        
                    };

                    imageInsertedEventArgs.ImageSourceCollection.Add(imageSource);
                }
            }
            catch (Exception)
            {

                return;
            }
            
        }














        /// <summary>
        /// Represents the property changed property of <see cref="INotifyPropertyChanged"/>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Represents the property changed method of <see cref="INotifyPropertyChanged"/>
        /// </summary>
        /// <param name="propertyname"></param>
        public void RaisePropertyChange([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
