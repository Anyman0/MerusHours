using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MerusHours.Models
{
    public class ActivitiesModel
    {
        [PrimaryKey, AutoIncrement]
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string ActivityDescription { get; set; }
    }
}
