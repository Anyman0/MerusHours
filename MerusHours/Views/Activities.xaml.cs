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
    public partial class Activities : ContentPage
    {
        public ObservableCollection<ActivitiesModel> ActivitiesCollection;
        public Command NewActivityCommand { get; }
        public Command ModifyActivityCommand { get; }
        public Command DeleteActivityCommand { get; }

        private int ChosenActivityID;
        private string ChosenActivityName;
        public Activities()
        {
            InitializeComponent();
            ActivitiesCollection = new ObservableCollection<ActivitiesModel>();
            NewActivityCommand = new Command(NewActivity);
            ModifyActivityCommand = new Command(ModifyActivity);
            DeleteActivityCommand = new Command(DeleteActivity);
            BindingContext = this;
        }

        private async void DeleteActivity(object obj)
        {
            if(ActivityCollection.SelectedItem != null)
            {
                DeleteActivityConfirmation(ChosenActivityName);
                ActivityCollection.SelectedItem = null;
            }
            else
            {
                await DisplayAlert("Error!", "Choose activity to delete.", "OK");
            }
        }

        private async void DeleteActivityConfirmation(string activity)
        {
            if(await DisplayAlert("HOX", "Are you sure you want to delete activity " + activity + "?", "Yes", "No"))
            {
                var activityModel = new ActivitiesModel();
                foreach(var model in await App.ActivityDatabase.GetActivities())
                {
                    if(model.ActivityID == ChosenActivityID)
                    {
                        activityModel = model;
                    }
                }
                await App.ActivityDatabase.DeleteActivityAsync(activityModel);
                UpdateView();
            }
        }

        private async void ModifyActivity(object obj)
        {
            if(ActivityCollection.SelectedItem != null)
            {
                await Navigation.PushModalAsync(new ModifyActivityPage(ChosenActivityID));
                ActivityCollection.SelectedItem = null;
            }
            else
            {
                await DisplayAlert("Error!", "Choose activity to modify", "OK");
            }
        }

        private async void NewActivity(object obj)
        {
            await Navigation.PushModalAsync(new NewActivityPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateView();
        }

        private void ActivityCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ActivityCollection.SelectedItem != null)
            {
                ChosenActivityID = (e.CurrentSelection.FirstOrDefault() as ActivitiesModel).ActivityID;
                ChosenActivityName = (e.CurrentSelection.FirstOrDefault() as ActivitiesModel)?.ActivityName;
            }
            else return;
        }

        private async void UpdateView()
        {
            ActivitiesCollection.Clear();
            ActivityCollection.SelectedItem = null;
            var activities = await App.ActivityDatabase.GetActivities();
            foreach(var am in activities)
            {
                ActivitiesCollection.Add(am);
            }
            ActivityCollection.ItemsSource = ActivitiesCollection;
        }

    }
}