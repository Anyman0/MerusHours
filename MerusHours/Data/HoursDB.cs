using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using MerusHours.Models;

namespace MerusHours.Data
{
    public class HoursDB
    {
        private readonly SQLiteAsyncConnection HoursDatabase;

        public HoursDB(string dbPath)
        {
            HoursDatabase = new SQLiteAsyncConnection(dbPath);
            HoursDatabase.CreateTableAsync<HoursModel>().Wait();
        }

        public Task<List<HoursModel>> GetHours()
        {
            // Get all hours
            return HoursDatabase.Table<HoursModel>().ToListAsync();
        }

        public Task<int> SaveHourAsync(HoursModel hour)
        {
            if(hour.HourID != 0)
            {
                // Update hour
                return HoursDatabase.UpdateAsync(hour);
            }
            else
            {
                // Save hour
                return HoursDatabase.InsertAsync(hour);
            }
        }

        public Task<int> DeleteHour(HoursModel hour)
        {
            // Delete hour
            return HoursDatabase.DeleteAsync(hour);
        }

        public Task<int> UpdateHour(HoursModel hour)
        {
            // Update hour
            return HoursDatabase.UpdateAsync(hour);
        }
    }
}
