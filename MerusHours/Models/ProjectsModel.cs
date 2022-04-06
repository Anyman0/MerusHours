using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MerusHours.Models
{
    public class ProjectsModel
    {
        [PrimaryKey, AutoIncrement]
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
    }
}
