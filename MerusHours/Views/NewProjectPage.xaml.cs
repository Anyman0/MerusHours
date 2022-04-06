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
    public partial class NewProjectPage : ContentPage
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public NewProjectPage()
        {
            InitializeComponent();
            SaveCommand = new Command(SaveProject);
            CancelCommand = new Command(Cancel);
            BindingContext = this;
        }

        private async void Cancel(object obj)
        {
            // Return to previous page
            await Shell.Current.GoToAsync("..");
        }

        private async void SaveProject(object obj)
        {
            var model = new ProjectsModel();
            model.ProjectName = ProjectEntry.Text;
            model.ProjectDescription = EditorEntry.Text;
            if(model.ProjectName != null)
            {
                await App.ProjectDatabase.SaveProjectAsync(model);
            }
            else
            {
                await DisplayAlert("Save failed!", "Project must have a name", "OK");
            }
            await Shell.Current.GoToAsync("..");
        }
    }
}