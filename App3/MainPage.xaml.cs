using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        private PersonManager DataManager;

        private ObservableCollection<Person> _Persons;
        public ObservableCollection<Person> Persons
        {
            get { return _Persons; }
            set { Set(ref _Persons, value); }
        }

        public MainPage()
        {
            this.InitializeComponent();
            DataManager = new PersonManager();
            Persons = new ObservableCollection<Person>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetPersonsView();
        }

        private void SetPersonsView()
        {
            Persons.Clear();
            var persons = DataManager.GetPersons();
            persons.ForEach(Persons.Add);
            SetSelected();
            AddNewPersons();

        }

        private void AddNewPersons()
        {
            var newPersons = DataManager.GetNewPersons();
            for (var i = 0; i < newPersons.Count; i++)
            {
                Persons.Insert(i, newPersons[i]);
            }
        }

        private void Click_Click(object sender, RoutedEventArgs e)
        {
            SetSelected();
        }

        private void SetSelected()
        {
            var random = new Random();
            var selected = Persons.FirstOrDefault(per => per.IsSelected == true);
            if (selected != null)
                selected.IsSelected = false;
            int num = random.Next(0, Persons.Count - 1);
            Persons[num].IsSelected = true;
            PersonView.ScrollIntoView(Persons[num], ScrollIntoViewAlignment.Leading);
        }

        private async void PersonView_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
