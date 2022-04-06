using MerusHours.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MerusHours.Data
{
    public class ProjectsDB
    {

        private readonly SQLiteAsyncConnection ProjectDatabase;

        public ProjectsDB(string dbPath)
        {
            ProjectDatabase = new SQLiteAsyncConnection(dbPath);
            ProjectDatabase.CreateTableAsync<ProjectsModel>().Wait();
        }

        public Task<List<ProjectsModel>> GetProjects()
        {
            // Get all projects
            return ProjectDatabase.Table<ProjectsModel>().ToListAsync();
        }

        public Task<int> SaveProjectAsync(ProjectsModel project)
        {
            if(project.ProjectID != 0)
            {
                // Update work
                return ProjectDatabase.UpdateAsync(project);
            }
            else
            {
                // Save new project
                return ProjectDatabase.InsertAsync(project);
            }
        }

        public Task<int> DeleteProjectAsync(ProjectsModel project)
        {
            // Delete project
            return ProjectDatabase.DeleteAsync(project);
        }

        public Task<int> UpdateProjectAsync(ProjectsModel project)
        {
            // Update values
            return ProjectDatabase.UpdateAsync(project);
        }

    }
}
