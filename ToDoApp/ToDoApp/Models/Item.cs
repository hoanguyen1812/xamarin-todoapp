using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ToDoApp.Models
{
    public class Item
    {
        //[PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Notes { get; set; }
    }
}
