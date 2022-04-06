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
    public partial class ModifyProjectPage : ContentPage
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private int ChosenProjectID;
        public ModifyProjectPage(int id)
        {
            InitializeComponent();
            SaveCommand = new Command(SaveChanges);
            CancelCommand = new Command(Cancel);
            ChosenProjectID = id;
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetChosenProject(ChosenProjectID);
            
        }
        private async void Cancel(object obj)
        {
            // Return to previous page
            await Shell.Current.GoToAsync("..");
        }

        private async void SaveChanges(object obj)
        {
            var projectModel = new ProjectsModel();
            projectModel.ProjectID = ChosenProjectID;
            projectModel.ProjectName = ProjectEntry.Text;
            projectModel.ProjectDescription = EditorEntry.Text;
            if(projectModel.ProjectID != 0)
            {
                await App.ProjectDatabase.UpdateProjectAsync(projectModel);
            }
            else
            {
                await DisplayAlert("Error!", "No existing project to modify", "OK");
            }
            await Shell.Current.GoToAsync("..");
        }

        private async Task GetChosenProject(int id)
        {
            var projects = await App.ProjectDatabase.GetProjects();
            foreach(var p in projects)
            {
                if(p.ProjectID == id)
                {
                    ProjectEntry.Text = p.ProjectName;
                    EditorEntry.Text = p.ProjectDescription;
                }
            }
        }
    }
}