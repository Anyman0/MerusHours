using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MerusHours.Models
{
    public class HoursModel
    {
        [PrimaryKey, AutoIncrement]
        public int HourID { get; set; }
        public string WorkName { get; set; }
        public string ProjectName { get; set; }
        public string ActivityName { get; set; }
        public string Hours { get; set; }
        public string Day { get; set; }
        public string Date { get; set; }

        // Control fields
        public bool InProgress { get; set; }
        public string WorkStarted { get; set; }
        public string WorkEnded { get; set; }          
        
    }
}
