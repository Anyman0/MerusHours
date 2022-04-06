using MerusHours.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MerusHours.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewHourPage : ContentPage
    {
        public ObservableCollection<string> WorkCollection;
        public ObservableCollection<string> ProjectCollection;
        public ObservableCollection<string> ActivityCollection;
        private string[] hourList;
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public NewHourPage()
        {
            InitializeComponent();
            WorkCollection = new ObservableCollection<string>();
            ProjectCollection = new ObservableCollection<string>();
            ActivityCollection = new ObservableCollection<string>();
            hourList = new string[32] {"0.25", "0.5", "0.75" , "1" , "1.25" , "1.5" , "1.75" , "2" , "2.25" , "2.5", "2.75", "3" , "3.25" , "3.5" , "3.75",
                                           "4", "4.25", "4.5", "4.75", "5", "5.25", "5.5", "5.75", "6", "6.25", "6.5", "6.75", "7", "7.25", "7.5", "7.75", "8"};
            SaveCommand = new Command(SaveHour);
            CancelCommand = new Command(Cancel);
            BindingContext = this;
        }

        private async void Cancel(object obj)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void SaveHour(object obj)
        {
            var hourModel = new HoursModel();
            hourModel.WorkName = WorkPicker.SelectedItem.ToString();
            hourModel.ProjectName = ProjectPicker.SelectedItem.ToString();
            hourModel.ActivityName = ActivityPicker.SelectedItem.ToString();
            hourModel.Hours = HoursPicker.SelectedItem.ToString();
            hourModel.Date = DatePicker.Date.ToString("dd/MM/yyyy");
            hourModel.Day = DatePicker.Date.ToString("dddd");
            hourModel.InProgress = false;

            if(hourModel.WorkName != null || hourModel.ProjectName != null)
            {
                await App.HoursDatabase.SaveHourAsync(hourModel);
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error!", "You must choose atleast work or project", "OK");
            }            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateView();
        }

        private async void UpdateView()
        {
            WorkCollection.Clear();
            ProjectCollection.Clear();
            ActivityCollection.Clear();
            var works = await App.WorkDatabase.GetWorks();
            var projects = await App.ProjectDatabase.GetProjects();
            var activities = await App.ActivityDatabase.GetActivities();
            
            foreach(var wm in works)
            {
                WorkCollection.Add(wm.WorkName);
            }
            foreach(var pm in projects)
            {
                ProjectCollection.Add(pm.ProjectName);
            }
            foreach(var am in activities)
            {
                ActivityCollection.Add(am.ActivityName);
            }
            WorkPicker.ItemsSource = WorkCollection;
            ProjectPicker.ItemsSource = ProjectCollection;
            ActivityPicker.ItemsSource = ActivityCollection;
            HoursPicker.ItemsSource = hourList;
        }
    }
}