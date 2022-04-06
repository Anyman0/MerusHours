using System;
using System.Collections.Generic;
using System.Text;
using MerusHours.Models;
using SQLite;
using System.Threading.Tasks;

namespace MerusHours.Data
{
    public class WorkDB
    {
        private readonly SQLiteAsyncConnection WorkDatabase;

        public WorkDB(string dbPath)
        {
            WorkDatabase = new SQLiteAsyncConnection(dbPath);
            WorkDatabase.CreateTableAsync<WorkModel>().Wait();
        }

        public Task<List<WorkModel>> GetWorks()
        {
            // Get all works
            return WorkDatabase.Table<WorkModel>().ToListAsync();
        }

        public Task<int> SaveWorkAsync(WorkModel work)
        {
            if(work.WorkID != 0)
            {
                // Update work
                return WorkDatabase.UpdateAsync(work);
            }
            else
            {
                // Save new work
                return WorkDatabase.InsertAsync(work);
            }
        }

        public Task<int> DeleteWorkAsync(WorkModel work)
        {
            // Delete work
            return WorkDatabase.DeleteAsync(work);
        }

        public Task<int> UpdateWorkAsync(WorkModel work)
        {
            // Update values
            return WorkDatabase.UpdateAsync(work);
        }

    }
}
