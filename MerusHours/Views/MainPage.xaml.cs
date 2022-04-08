using MerusHours.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace MerusHours.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        // Collections
        public ObservableCollection<string> WorkCollection;
        public ObservableCollection<string> ProjectCollection;
        public ObservableCollection<string> ActivityCollection;
        public ObservableCollection<HoursModel> HoursCollection;
        private double[] hourList;
        // Picker data
        //private string PickerWorkName;
        //private string PickerProjectName;
        //private string PickerActivityName;
       
        public Command BeginCommand { get; }
        public Command EndCommand { get; }
        public Command ScanCommand { get; }
        public Command FinishCommand { get; }
        public string SelectedWork { get; }
        public MainPage()
        {
            InitializeComponent();
            WorkCollection = new ObservableCollection<string>();
            ProjectCollection = new ObservableCollection<string>();
            ActivityCollection = new ObservableCollection<string>();
            HoursCollection = new ObservableCollection<HoursModel>();
            BeginCommand = new Command(BeginWork);
            EndCommand = new Command(EndWork);
            ScanCommand = new Command(ScanWork);
            FinishCommand = new Command(FinishDay);
            hourList = new double[32] {0.25, 0.5, 0.75 , 1 , 1.25 , 1.5 , 1.75 , 2 , 2.25 , 2.5, 2.75, 3 , 3.25 , 3.5 , 3.75,
                                           4, 4.25, 4.5, 4.75, 5, 5.25, 5.5, 5.75, 6, 6.25, 6.5, 6.75, 7, 7.25, 7.5, 7.75, 8.0};
            BindingContext = this;
        }

        private async void ScanWork(object obj)
        {
            // Start work by scanning QR code
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();
            if(result != null)
            {
                if(!CheckStatus(await App.HoursDatabase.GetHours()))
                {
                    var model = new HoursModel();
                    try
                    {
                        var fields = result.Text.Split('.');
                        if (fields[0] != null || fields[1] != null)
                        {
                            if (fields[0] != null) model.WorkName = fields[0];
                            if (fields[1] != null) model.ProjectName = fields[1];
                            if (fields[2] != null) model.ActivityName = fields[2];
                            model.Day = DateTime.Now.DayOfWeek.ToString();
                            model.Date = DateTime.Now.ToString("dd/MM/yyyy");
                            model.InProgress = true;
                            model.WorkStarted = DateTime.Now.ToString("HH.mm");

                            await App.HoursDatabase.SaveHourAsync(model);
                            BeginWorkButton.Text = "End Work";
                            BeginWorkButton.BackgroundColor = Color.IndianRed;
                            UpdateView();
                        }
                        
                    }     
                    catch
                    {
                        await DisplayAlert("Start failed!", "You must scan a valid QR code", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Start failed!", "There's already a work in progress. You must end that first.", "OK");
                }
            }
            else
            {
                
            }
        }

        private async void EndWork(object obj)
        {
            var hours = await App.HoursDatabase.GetHours();
            foreach(var h in hours)
            {
                if(h.InProgress)
                {
                    var startTimes = h.WorkStarted.Split('.');
                    var endTimes = DateTime.Now.ToString("HH.mm").Split('.');
                    var endMinutes = (int.Parse(endTimes[0]) * 60) + int.Parse(endTimes[1]);
                    var startMinutes = (int.Parse(startTimes[0]) * 60) + int.Parse(startTimes[1]);
                    var hoursDouble = Math.Ceiling((endMinutes - startMinutes) / 15.0) / 4.0;
                    h.Hours = hoursDouble.ToString();
                    h.InProgress = false;
                    if (hoursDouble <= 7)
                    {
                        h.Hours = "0";                        
                        await DisplayAlert("Finished!", "Work " + h.WorkName + " ended. You spent less than minimum 7 minutes on it. Will not save this.", "Close");
                        await App.HoursDatabase.DeleteHour(h);
                        UpdateView();
                        break;
                    }                     
                    await App.HoursDatabase.UpdateHour(h);                    
                    await DisplayAlert("Finished!", "Work " + h.WorkName + " ended. You spent " + h.Hours + "h on it.", "Close");
                    UpdateView();
                    break;
                }
            }
        }

        private async void BeginWork(object obj)
        {
            if (BeginWorkButton.Text == "Start Work")
            {
                if (!CheckStatus(await App.HoursDatabase.GetHours()))
                {
                    var model = new HoursModel();
                    
                    if (WorkPicker.SelectedItem != null || ProjectPicker.SelectedItem != null)
                    {                        
                        if(WorkPicker.SelectedItem != null) model.WorkName = WorkPicker.SelectedItem.ToString();
                        if(ProjectPicker.SelectedItem != null) model.ProjectName = ProjectPicker.SelectedItem.ToString();
                        if(ActivityPicker.SelectedItem != null) model.ActivityName = ActivityPicker.SelectedItem.ToString();
                        model.Day = DateTime.Now.DayOfWeek.ToString();
                        model.Date = DateTime.Now.ToString("dd/MM/yyyy");
                        model.InProgress = true;
                        model.WorkStarted = DateTime.Now.ToString("HH.mm");
                                               
                        await App.HoursDatabase.SaveHourAsync(model);
                        BeginWorkButton.Text = "End Work";
                        BeginWorkButton.BackgroundColor = Color.IndianRed;
                        UpdateView();
                    }
                    else
                    {
                        await DisplayAlert("Start failed!", "You must choose atleast work or project to begin.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error!", "There's already a work in progress. You must end that first.", "OK");
                }
            }
            else if(BeginWorkButton.Text == "End Work")
            {
                var hours = await App.HoursDatabase.GetHours();
                foreach (var h in hours)
                {
                    if (h.InProgress)
                    {
                        var startTimes = h.WorkStarted.Split('.');
                        var endTimes = DateTime.Now.ToString("HH.mm").Split('.');
                        var endMinutes = (int.Parse(endTimes[0]) * 60) + int.Parse(endTimes[1]);
                        var startMinutes = (int.Parse(startTimes[0]) * 60) + int.Parse(startTimes[1]);                        
                        
                        var hoursFloat = ((float)endMinutes - (float)startMinutes) / 15f / 4f;                        
                        var closestValue = FindClosest(hourList, hoursFloat);
                        h.Hours = closestValue.ToString();
                        h.InProgress = false;
                        if (endMinutes - startMinutes < 7)
                        {
                            h.Hours = "0";
                            await DisplayAlert("Finished!", "Work " + h.WorkName + " ended. You spent less than minimum 7 minutes on it. Will not save this.", "Close");
                            await App.HoursDatabase.DeleteHour(h);
                            BeginWorkButton.Text = "Start Work";
                            BeginWorkButton.BackgroundColor = Color.ForestGreen;
                            UpdateView();
                            break;
                        }
                        await App.HoursDatabase.UpdateHour(h);                       
                        await DisplayAlert("Finished!", "Work " + h.WorkName + " ended. You spent " + h.Hours + "h on it.", "Close");
                        BeginWorkButton.Text = "Start Work";
                        BeginWorkButton.BackgroundColor = Color.ForestGreen;
                        UpdateView();
                        break;
                    }
                }
            }
        }        

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateView();
        }

        private async void UpdateView()
        {
            WorkCollection.Clear();
            ProjectCollection.Clear();
            ActivityCollection.Clear();
            HoursCollection.Clear();           
            var works = await App.WorkDatabase.GetWorks();
            var projects = await App.ProjectDatabase.GetProjects();
            var activities = await App.ActivityDatabase.GetActivities();
            var hours = await App.HoursDatabase.GetHours();
            foreach (var wm in works)
            {
                WorkCollection.Add(wm.WorkName);
            }
            foreach(var pm in projects)
            {
                ProjectCollection.Add(pm.ProjectName);
            }
            foreach(var am in activities)
            {
                ActivityCollection.Add(am.ActivityName);
            }
            foreach(var h in hours)
            {
                if(h.InProgress)
                {
                    BeginWorkButton.Text = "End Work";
                    BeginWorkButton.BackgroundColor = Color.IndianRed;
                    HoursCollection.Add(h);
                }
            }
            WorkPicker.ItemsSource = WorkCollection;
            ProjectPicker.ItemsSource = ProjectCollection;
            ActivityPicker.ItemsSource = ActivityCollection;
            InProgressCollection.ItemsSource = HoursCollection;
        }

        // returns 'False' if we can begin new work, true if not.
        private bool CheckStatus(List<HoursModel> model)
        {
            foreach(var h in model)
            {
                if(h.InProgress)
                {
                    return true;
                }
            }
            return false;
        }
        

        private async void FinishDay()
        {
            if (await DisplayAlert("HOX!", "Are you sure you want to finish day? This will automatically populate +/- hours on this day if there is any.", "Yes", "No"))
            {
                var hours = await App.HoursDatabase.GetHours();
                int totalhours = 0;
                var model = new HoursModel();
                foreach (var h in hours)
                {
                    if (h.Date == DateTime.Now.ToString("dd/MM/yyyy"))
                    {
                        totalhours += int.Parse(h.Hours);
                    }
                }
                if (totalhours > 7.5)
                {
                    model.WorkName = "W02447";
                    model.ProjectName = "GE024";
                    model.ActivityName = "+/-";
                    model.Day = DateTime.Now.DayOfWeek.ToString();
                    model.Date = DateTime.Now.ToString("dd/MM/yyyy");
                    model.InProgress = false;
                    model.Hours = "-" + (totalhours - 7.5).ToString();
                }
                else if(totalhours < 7.5)
                {
                    model.WorkName = "W02447";
                    model.ProjectName = "GE024";
                    model.ActivityName = "+/-";
                    model.Day = DateTime.Now.DayOfWeek.ToString();
                    model.Date = DateTime.Now.ToString("dd/MM/yyyy");
                    model.InProgress = false;
                    model.Hours = "+" + (7.5 - totalhours).ToString();
                }
                await App.HoursDatabase.SaveHourAsync(model);
            }           
        }

        // Returns element closest
        // to target in arr[]
        public static double FindClosest(double[] arr,
                                      double target)
        {
            int n = arr.Length;

            // Corner cases
            if (target <= arr[0])
                return arr[0];
            if (target >= arr[n - 1])
                return arr[n - 1];

            // Doing binary search
            int i = 0, j = n, mid = 0;
            while (i < j)
            {
                mid = (i + j) / 2;

                if (arr[mid] == target)
                    return arr[mid];

                /* If target is less
                than array element,
                then search in left */
                if (target < arr[mid])
                {

                    // If target is greater
                    // than previous to mid,
                    // return closest of two
                    if (mid > 0 && target > arr[mid - 1])
                        return GetClosest(arr[mid - 1],
                                     arr[mid], target);

                    /* Repeat for left half */
                    j = mid;
                }

                // If target is
                // greater than mid
                else
                {
                    if (mid < n - 1 && target < arr[mid + 1])
                        return GetClosest(arr[mid],
                             arr[mid + 1], target);
                    i = mid + 1; // update i
                }
            }

            // Only single element
            // left after search
            return arr[mid];
        }

        public static double GetClosest(double val1, double val2,
                                 double target)
        {
            if (target - val1 >= val2 - target)
                return val2;
            else
                return val1;
        }
    }
}