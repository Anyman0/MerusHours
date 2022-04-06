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
    public partial class NewWorkPage : ContentPage
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public NewWorkPage()
        {
            InitializeComponent();
            SaveCommand = new Command(SaveWork);
            CancelCommand = new Command(Cancel);
            BindingContext = this;
        }

        private async void Cancel(object obj)
        {
            // Return to previous page
            await Shell.Current.GoToAsync("..");
        }

        private async void SaveWork(object obj)
        {
            var model = new WorkModel();
            model.WorkName = WorkEntry.Text;            
            model.WorkDescription = EditorEntry.Text;

            if(model.WorkName != null)
            {
                await App.WorkDatabase.SaveWorkAsync(model);
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Save failed!", "Work must have a name.", "OK");
            }           
        }
    }
}