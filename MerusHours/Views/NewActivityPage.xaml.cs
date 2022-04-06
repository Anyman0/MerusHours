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
    public partial class NewActivityPage : ContentPage
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public NewActivityPage()
        {
            InitializeComponent();
            SaveCommand = new Command(SaveActivity);
            CancelCommand = new Command(Cancel);
            BindingContext = this;
        }

        private async void Cancel(object obj)
        {
            // Return to previous page
            await Shell.Current.GoToAsync("..");
        }

        private async void SaveActivity(object obj)
        {
            var model = new ActivitiesModel();
            model.ActivityName = ActivityEntry.Text;
            model.ActivityDescription = EditorEntry.Text;
            if(model.ActivityName != null)
            {
                await App.ActivityDatabase.SaveActivityAsync(model);
            }
            else
            {
                await DisplayAlert("Save failed!", "Activity must have a name", "OK");
            }
            await Shell.Current.GoToAsync("..");
        }
    }
}