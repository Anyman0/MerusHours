using MerusHours.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MerusHours.Data
{
    public class ActivityDB
    {
        private readonly SQLiteAsyncConnection ActivityDatabase;

        public ActivityDB(string dbPath)
        {
            ActivityDatabase = new SQLiteAsyncConnection(dbPath);
            ActivityDatabase.CreateTableAsync<ActivitiesModel>().Wait();
        }

        public Task<List<ActivitiesModel>> GetActivities()
        {
            // Get all activities
            return ActivityDatabase.Table<ActivitiesModel>().ToListAsync();
        }

        public Task<int> SaveActivityAsync(ActivitiesModel activity)
        {
            if(activity.ActivityID != 0)
            {
                // Update activity
                return ActivityDatabase.UpdateAsync(activity);
            }
            else
            {
                // Save new activity
                return ActivityDatabase.InsertAsync(activity);
            }
        }

        public Task<int> DeleteActivityAsync(ActivitiesModel activity)
        {
            // Delete activity
            return ActivityDatabase.DeleteAsync(activity);
        }

        public Task<int> UpdateActivityAsync(ActivitiesModel activity)
        {
            // Update values
            return ActivityDatabase.UpdateAsync(activity);
        }
    }
}
