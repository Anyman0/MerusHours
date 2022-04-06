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
    public partial class Projects : ContentPage
    {
        public ObservableCollection<ProjectsModel> ProjectCollection;
        public Command NewProjectCommand { get; }
        public Command ModifyProjectCommand { get; }
        public Command DeleteProjectCommand { get; }

        private int ChosenProjectID;
        private string ChosenProjectName;
        public Projects()
        {
            InitializeComponent();
            ProjectCollection = new ObservableCollection<ProjectsModel>();
            NewProjectCommand = new Command(NewProject);
            ModifyProjectCommand = new Command(ModifyProject);
            DeleteProjectCommand = new Command(DeleteProject);
            BindingContext = this;
        }

        private async void DeleteProject(object obj)
        {
            if(ProjectsCollection.SelectedItem != null)
            {
                DeleteProjectConfirmation(ChosenProjectName);
                ProjectsCollection.SelectedItem = null;
            }
            else
            {
                await DisplayAlert("Error!", "Choose project to delete", "OK");
            }
        }

        private async void DeleteProjectConfirmation(string project)
        {
            if(await DisplayAlert("HOX", "Are you sure you want to delete " + project + "?", "Yes", "No"))
            {
                var projectModel = new ProjectsModel();
                foreach(var model in await App.ProjectDatabase.GetProjects())
                {
                    if(model.ProjectID == ChosenProjectID)
                    {
                        projectModel = model;
                    }
                }
                await App.ProjectDatabase.DeleteProjectAsync(projectModel);
                UpdateView();
            }
            else
            {

            }
        }

        private async void ModifyProject(object obj)
        {
            if(ProjectsCollection.SelectedItem != null)
            {
                await Navigation.PushModalAsync(new ModifyProjectPage(ChosenProjectID));
                ProjectsCollection.SelectedItem = null;
            }
            else
            {
                await DisplayAlert("Error!", "Choose project to modify", "OK");
            }
        }

        private async void NewProject(object obj)
        {
            await Navigation.PushModalAsync(new NewProjectPage()); 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateView();
        }

        private async void UpdateView()
        {
            ProjectCollection.Clear();
            ProjectsCollection.SelectedItem = null;
            var projects = await App.ProjectDatabase.GetProjects();
            foreach(var pm in projects)
            {
                ProjectCollection.Add(pm);
            }
            ProjectsCollection.ItemsSource = ProjectCollection;
        }

        private void ProjectsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProjectsCollection.SelectedItem != null)
            {
                ChosenProjectID = (e.CurrentSelection.FirstOrDefault() as ProjectsModel).ProjectID;
                ChosenProjectName = (e.CurrentSelection.FirstOrDefault() as ProjectsModel)?.ProjectName;
            }
            else return;
        }
    }
}

