using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerusHours.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MerusHours.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyWorkPage : ContentPage
    {       
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private int ChosenWorkID;
        public ModifyWorkPage(int id)
        {
            InitializeComponent();
            SaveCommand = new Command(SaveChangesAsync);
            CancelCommand = new Command(Cancel);
            ChosenWorkID = id;
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetChosenWorkAsync(ChosenWorkID);
        }

        private async void Cancel(object obj)
        {
            // Return to previous page
            await Shell.Current.GoToAsync("..");
        }
        
        private async void SaveChangesAsync(object obj)
        {
            var workModel = new WorkModel();
            workModel.WorkID = ChosenWorkID;
            workModel.WorkName = WorkEntry.Text;
            workModel.WorkDescription = EditorEntry.Text;
            if(workModel.WorkID != 0)
            {
                await App.WorkDatabase.UpdateWorkAsync(workModel);                
            }
            else
            {
                await DisplayAlert("Error!", "No existing work to modify", "OK");
            }
            await Shell.Current.GoToAsync("..");
        }
        
        private async Task GetChosenWorkAsync(int id)
        {
            var works = await App.WorkDatabase.GetWorks();
            foreach(var w in works)
            {
                if(w.WorkID == id)
                {
                    WorkEntry.Text = w.WorkName;
                    EditorEntry.Text = w.WorkDescription;
                }
            }
        }
    }
}