using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDiary.Model
{
    public class PageItems
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string extra1 { get; set; }

        public bool extra2 { get; set; }
    }
}
