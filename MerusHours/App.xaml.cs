using MerusHours.Data;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MerusHours
{
    public partial class App : Application
    {
        private static WorkDB WorkData;
        private static HoursDB HoursData;
        private static ActivityDB ActivityData;
        private static ProjectsDB ProjectsData;

        // Create DB-connections as singleton
        public static WorkDB WorkDatabase
        {
            get
            {
                if (WorkData == null)
                {
                    WorkData = new WorkDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Work.db3"));
                }
                return WorkData;
            }
        }

        public static HoursDB HoursDatabase
        {
            get
            {
                if (HoursData == null)
                {
                    HoursData = new HoursDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Hours.db3"));
                }
                return HoursData;
            }
        }

        public static ActivityDB ActivityDatabase
        {
            get
            {
                if(ActivityData == null)
                {
                    ActivityData = new ActivityDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Activities.db3"));
                }
                return ActivityData;
            }
        }

        public static ProjectsDB ProjectDatabase
        {
            get
            {
                if(ProjectsData == null)
                {
                    ProjectsData = new ProjectsDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Projects.db3"));
                }
                return ProjectsData;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new MainPageShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
