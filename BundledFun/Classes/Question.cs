using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundledFun.Classes
{
    public class Question
    {
        public int id { set; get; }
        public int group_id { set; get; }
        public string text { set; get; }
        public string difficulty { set; get; }
        public string answer { set; get; }
        public string choice_a { set; get; }
        public string choice_b { set; get; }
        public string choice_c { set; get; }
        public string file { set; get; }
        public string type { set; get; }
        public int points { set; get; }
        public int timer { set; get; }
    }
}
