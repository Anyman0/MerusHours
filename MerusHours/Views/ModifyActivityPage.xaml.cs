using MerusHours.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MerusHours.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyActivityPage : ContentPage
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private int ChosenActivityID;
        public ModifyActivityPage(int id)
        {
            InitializeComponent();
            SaveCommand = new Command(SaveChanges);
            CancelCommand = new Command(Cancel);
            ChosenActivityID = id;
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetChosenActivity(ChosenActivityID);
        }
        private async void Cancel(object obj)
        {
            // Return to previous page
            await Shell.Current.GoToAsync("..");
        }

        private async void SaveChanges(object obj)
        {
            var activityModel = new ActivitiesModel();
            activityModel.ActivityID = ChosenActivityID;
            activityModel.ActivityName = ActivityEntry.Text;
            activityModel.ActivityDescription = EditorEntry.Text;
            if(activityModel.ActivityID != 0)
            {
                await App.ActivityDatabase.UpdateActivityAsync(activityModel);
            }
            else
            {
                await DisplayAlert("Error!", "No existing activity to modify", "OK");
            }
            await Shell.Current.GoToAsync("..");
        }

        private async Task GetChosenActivity(int id)
        {
            var activities = await App.ActivityDatabase.GetActivities();
            foreach(var a in activities)
            {
                if(a.ActivityID == id)
                {
                    ActivityEntry.Text = a.ActivityName;
                    EditorEntry.Text = a.ActivityDescription;
                }
            }
        }
    }
}