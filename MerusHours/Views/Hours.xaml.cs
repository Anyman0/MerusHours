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
    public partial class Hours : ContentPage
    {
        public Command NewHourCommand { get; }
        public Command ModifyHourCommand { get; }
        public Command DeleteHourCommand { get; }

        public ObservableCollection<HoursModel> HourCollection;
        private ObservableCollection<string> WorkCollection;
        private ObservableCollection<string> ProjectCollection;
        private ObservableCollection<string> ActivityCollection;

        private int ChosenHourID;

        private string[] SearchPickerData;
        public Hours()
        {
            InitializeComponent();
            HourCollection = new ObservableCollection<HoursModel>();
            WorkCollection = new ObservableCollection<string>();
            ProjectCollection = new ObservableCollection<string>();
            ActivityCollection = new ObservableCollection<string>();
            NewHourCommand = new Command(NewHour);
            ModifyHourCommand = new Command(ModifyHour);
            DeleteHourCommand = new Command(DeleteHour);
            SearchPickerData = new string[4] { "Work", "Project", "Activity", "Date" };
            SearchByPicker.ItemsSource = SearchPickerData;           
            BindingContext = this;
        }

        private async void DeleteHour(object obj)
        {
            if(HoursCollection.SelectedItem != null)
            {
                DeleteHourConfirmation();
                HoursCollection.SelectedItem = null;
            }
            else
            {
                await DisplayAlert("Error!", "Choose hour log to delete.", "OK");
            }
        }

        private async void DeleteHourConfirmation()
        {
            if(await DisplayAlert("HOX!", "Are you sure you want to delete this hour log?", "Yes", "No"))
            {
                var hourModel = new HoursModel();
                foreach(var model in await App.HoursDatabase.GetHours())
                {
                    if(model.HourID == ChosenHourID)
                    {
                        hourModel = model;
                    }
                }
                await App.HoursDatabase.DeleteHour(hourModel);
                UpdateView();
            }
        }

        private async void ModifyHour(object obj)
        {
            if(HoursCollection.SelectedItem != null)
            {
                await Navigation.PushModalAsync(new ModifyHourPage(ChosenHourID));
                HoursCollection.SelectedItem = null;
            }
            else
            {
                await DisplayAlert("Error!", "Choose hour to modify", "OK");
            }
        }

        private async void NewHour(object obj)
        {
            await Navigation.PushModalAsync(new NewHourPage());
        }

        private void HoursCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(HoursCollection.SelectedItem != null)
            {
                ChosenHourID = (e.CurrentSelection.FirstOrDefault() as HoursModel).HourID;                
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateView();            
        }

        private async void UpdateView()
        {
            HourCollection.Clear();
            WorkCollection.Clear();
            ProjectCollection.Clear();
            ActivityCollection.Clear();
            HoursCollection.SelectedItem = null;
            var hours = await App.HoursDatabase.GetHours();
            var works = await App.WorkDatabase.GetWorks();
            var projects = await App.ProjectDatabase.GetProjects();
            var activities = await App.ActivityDatabase.GetActivities();

            foreach(var h in hours)
            {
                HourCollection.Add(h);
            }
            foreach(var w in works)
            {
                WorkCollection.Add(w.WorkName);
            }
            foreach(var p in projects)
            {
                ProjectCollection.Add(p.ProjectName);
            }
            foreach(var a in activities)
            {
                ActivityCollection.Add(a.ActivityName);
            }
            HoursCollection.ItemsSource = HourCollection;
        }

        private async void StartSearchButton_Clicked(object sender, EventArgs e)
        {
            HourCollection.Clear();
            var hours = await App.HoursDatabase.GetHours();
            string chosenValue;
            if(SearchByPicker.SelectedItem.ToString() != "Date")
            {
                chosenValue = SearchDefinitionPicker.SelectedItem.ToString();
            }
            else
            {
                chosenValue = SearchDatePicker.Date.ToString("dd/MM/yyyy");
            }

            foreach(var h in hours)
            {
               if(h.WorkName == chosenValue || h.ProjectName == chosenValue || h.ActivityName == chosenValue || h.Date == chosenValue)
               {
                    HourCollection.Add(h);
               }
            }
            HoursCollection.ItemsSource = HourCollection;
        }

        private void SearchByPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SearchDatePicker.IsVisible = false;
                if (SearchByPicker.SelectedItem.ToString() == "Work")
                {
                    SearchDefinitionPicker.IsVisible = true;
                    SearchDefinitionPicker.ItemsSource = WorkCollection;
                }
                else if (SearchByPicker.SelectedItem.ToString() == "Project")
                {
                    SearchDefinitionPicker.IsVisible = true;
                    SearchDefinitionPicker.ItemsSource = ProjectCollection;
                }
                else if (SearchByPicker.SelectedItem.ToString() == "Activity")
                {
                    SearchDefinitionPicker.IsVisible = true;
                    SearchDefinitionPicker.ItemsSource = ActivityCollection;
                }
                else if (SearchByPicker.SelectedItem.ToString() == "Date")
                {
                    SearchDefinitionPicker.IsVisible = false;
                    SearchDatePicker.IsVisible = true;
                }
            }
            catch
            {

            }
        }
    }
}