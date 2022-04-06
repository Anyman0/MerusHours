using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MerusHours.Models
{
    public class WorkModel
    {
        [PrimaryKey, AutoIncrement]
        public int WorkID { get; set; }
        public string WorkName { get; set; }
        public string WorkDescription { get; set; }
    }
}
