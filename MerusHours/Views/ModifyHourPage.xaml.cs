using MerusHours.Models;
using MerusHours.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MerusHours.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyHourPage : ContentPage
    {
        public Command CancelCommand { get; }
        public Command SaveCommand { get; }

        private int ChosenHourID;
        //private string[] ListOfHours;
        public ModifyHourPage(int HourID)
        {
            InitializeComponent();
            CancelCommand = new Command(Cancel);
            SaveCommand = new Command(SaveChanges);
            BindingContext = this;
            ChosenHourID = HourID;
        }

        private async void SaveChanges(object obj)
        {
            var hoursModel = new HoursModel();
            hoursModel.HourID = ChosenHourID;
            hoursModel.WorkName = WorkEntry.Text;
            hoursModel.ProjectName = ProjectEntry.Text;
            hoursModel.ActivityName = ActivityEntry.Text;
            hoursModel.Hours = HoursPicker.SelectedItem.ToString();
            hoursModel.Day = DatePicker.Date.ToString("dddd");
            hoursModel.Date = DatePicker.Date.ToString("dd/MM/yyyy");
            if(hoursModel.HourID != 0)
            {
                await App.HoursDatabase.UpdateHour(hoursModel);
            }
            else
            {
                await DisplayAlert("Error!", "No existing hour to modify", "OK");
            }
            await Shell.Current.GoToAsync("..");
        }

        private async void Cancel(object obj)
        {
            // Return to previous page
            await Shell.Current.GoToAsync("..");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            /*ListOfHours = new string[30] {"0.25", "0.5", "0.75" , "1" , "1.25" , "1.5" , "1.75" , "2" , "2.25" , "2.5", "2.75", "3" , "3.25" , "3.5" , "3.75",
                                           "4", "4.25", "4.5", "4.75", "5", "5.25", "5.5", "5.75", "6", "6.25", "6.5", "6.75", "7", "7.25", "7.5"};*/
            HoursPicker.ItemsSource = SharedResources.Hours.Split(',');
            await GetChosenHour(ChosenHourID);
        }

        private async Task GetChosenHour(int id)
        {
            var hours = await App.HoursDatabase.GetHours();
            foreach(var h in hours)
            {
                if(h.HourID == id)
                {
                    WorkEntry.Text = h.WorkName;
                    ProjectEntry.Text = h.ProjectName;
                    ActivityEntry.Text = h.ActivityName;
                    HoursPicker.SelectedItem = h.Hours;                                      
                    DatePicker.Date = DateTime.ParseExact(h.Date, "dd/MM/yyyy", new CultureInfo("fi-FI", true));
                }
            }
        }
    }
}