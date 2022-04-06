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
    public partial class Works : ContentPage
    {
        public ObservableCollection<WorkModel> WorkCollection;
        public Command NewWorkCommand { get; }
        public Command ModifyWorkCommand { get; }
        public Command DeleteWorkCommand { get; }

        private int ChosenWorkID;
        private string ChosenWorkName;

        public Works()
        {
            InitializeComponent();
            WorkCollection = new ObservableCollection<WorkModel>();
            NewWorkCommand = new Command(NewItem);
            ModifyWorkCommand = new Command(ModifyWork);
            DeleteWorkCommand = new Command(DeleteWork);
            BindingContext = this;           
        }

        private async void DeleteWork(object obj)
        {
            if(WorksCollection.SelectedItem != null)
            {
                DeleteWorkConfirmation(ChosenWorkName);
                WorksCollection.SelectedItem = null;
            }
            else
            {
                await DisplayAlert("Error!", "Choose work to delete", "OK");
            }
        }

        private async void DeleteWorkConfirmation(string work)
        {
            if(await DisplayAlert("HOX", "Are you sure you want to delete work " + work + "?", "Yes", "No"))
            {
                var workModel = new WorkModel(); 
                foreach(var model in await App.WorkDatabase.GetWorks())
                {
                    if(model.WorkID == ChosenWorkID)
                    {
                        workModel = model;
                    }
                }
                await App.WorkDatabase.DeleteWorkAsync(workModel);
                UpdateView();
            }
            else
            {

            }

        }
        private async void ModifyWork(object obj)
        {
            if(WorksCollection.SelectedItem != null)
            {
                await Navigation.PushModalAsync(new ModifyWorkPage(ChosenWorkID));
                WorksCollection.SelectedItem = null;
            }
            else
            {
                await DisplayAlert("Error!", "Choose work to modify", "OK");                              
            }
        }

        private async void NewItem(object obj)
        {
            await Navigation.PushModalAsync(new NewWorkPage());                                                      
        }

        protected override void OnAppearing() 
        {
            base.OnAppearing();
            UpdateView();
        }

        private async void UpdateView()
        {
            WorkCollection.Clear();
            WorksCollection.SelectedItem = null;
            var works = await App.WorkDatabase.GetWorks();
            foreach (var wm in works)
            {
                WorkCollection.Add(wm);
            }
            WorksCollection.ItemsSource = WorkCollection;            
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WorksCollection.SelectedItem != null)
            {
                ChosenWorkID = (e.CurrentSelection.FirstOrDefault() as WorkModel).WorkID;
                ChosenWorkName = (e.CurrentSelection.FirstOrDefault() as WorkModel)?.WorkName;
            }
            else return;
        }
        
    }
}