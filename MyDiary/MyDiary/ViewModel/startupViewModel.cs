using MyDiary.Model;
using Plugin.Fingerprint;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyDiary.ViewModel
{
    public class startupViewModel : INotifyPropertyChanged
    {
        readonly IList<Monkey> source;

        public ObservableCollection<Monkey> Monkeys { get; private set; }
        public IList<Monkey> EmptyMonkeys { get; private set; }

        public Monkey PreviousMonkey { get; set; }
        public Monkey CurrentMonkey { get; set; }
        public Monkey CurrentItem { get; set; }
        public int PreviousPosition { get; set; }
        public int CurrentPosition { get; set; }
        public int Position { get; set; }

        public ICommand FilterCommand => new Command<string>(FilterItems);
        public ICommand ItemChangedCommand => new Command<Monkey>(ItemChanged);
        public ICommand PositionChangedCommand => new Command<int>(PositionChanged);

        public startupViewModel()
        {
            source = new List<Monkey>();
            CreateMonkeyCollection();

            CurrentItem = Monkeys.Skip(3).FirstOrDefault();
            OnPropertyChanged("CurrentItem");
            Position = 3;
            OnPropertyChanged("Position");
        }

        async void CreateMonkeyCollection()
        {
            source.Add(new Monkey
            {
                Name = "Hi..!!",
                Location = "",
                Details = "Warmly Welcome to your personal journal and diary . Click next button to continue",
                ImageUrl = "28651-back-to-scool.json"
            });

            source.Add(new Monkey
            {
                Name = "",
                Location = "",
                Details = "In this app you can plan, organize and list down things what ever you want and can be use as a journal or a diary",
                ImageUrl = "16546-waiting-for-ideas.json"
            });

            source.Add(new Monkey
            {
                Name = "Your secrets safe with you!",
                Location = "",
                Details = "If you want you can Protect your jounal by a password",
                ImageUrl = "14214-unlock.json",
                buttonName = "Add password now",
                ButtonVisible = true,
                animationSize = "100"

            });

            bool isFingerprintAvailable = await CrossFingerprint.Current.IsAvailableAsync(false);
            if (isFingerprintAvailable)
            { 
                source.Add(new Monkey
                {
                    Name = "For your fingertips!",
                    Location = "",
                    Details = "You can unlock journal by your fingerprint",
                    ImageUrl = "11637-fingerprint-scan.json",
                    buttonName = "Enable Fingerprint unlock",
                    ButtonVisible = true,
                    animationSize = "200"

                });
            }

            source.Add(new Monkey
            {
                Name = "Yey!",
                Location = "",
                Details = "Lets Begin..",
                ImageUrl = "26182-happy-star.json",
                buttonName = "Enable Fingerprint unlock",
                ButtonVisible = false,
                animationSize = "200"

            });


            Monkeys = new ObservableCollection<Monkey>(source);
        }

        void FilterItems(string filter)
        {
            var filteredItems = source.Where(monkey => monkey.Name.ToLower().Contains(filter.ToLower())).ToList();
            foreach (var monkey in source)
            {
                if (!filteredItems.Contains(monkey))
                {
                    Monkeys.Remove(monkey);
                }
                else
                {
                    if (!Monkeys.Contains(monkey))
                    {
                        Monkeys.Add(monkey);
                    }
                }
            }
        }

        void ItemChanged(Monkey item)
        {
            PreviousMonkey = CurrentMonkey;
            CurrentMonkey = item;
            OnPropertyChanged("PreviousMonkey");
            OnPropertyChanged("CurrentMonkey");
        }

        void PositionChanged(int position)
        {
            PreviousPosition = CurrentPosition;
            CurrentPosition = position;
            OnPropertyChanged("PreviousPosition");
            OnPropertyChanged("CurrentPosition");
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
