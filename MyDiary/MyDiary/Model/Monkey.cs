using System;
using System.Collections.Generic;
using System.Text;

namespace MyDiary.Model
{
    public class Monkey
    {
        public string Name { get; set; } = "";
        public string Location { get; set; } = "";
        public string Details { get; set; }
        public string ImageUrl { get; set; }

        public string animationSize { get; set; } = "200";

        public string buttonName { get; set; } = "";
        public bool ButtonVisible { get; set; } = false;


    }
}
