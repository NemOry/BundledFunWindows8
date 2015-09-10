using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundledFun.Classes
{
    class User
    {
        public int id { set; get; }
        public int group_id { set; get; }
        public string name { set; get; }
        public string username { set; get; }
        public int level { set; get; }
        public string access_token { set; get; }
        public string password { set; get; }
        public string picture { set; get; }
        public string email { set; get; }
        public int access { set; get; }

        public async Task<User> register()
        {
            string jsonString = await Statics.getJSONString(Statics.hostName + "/includes/functions/registrator.php?group_id=" + this.group_id + "&name=" + this.name + "&username=" + this.username + "&password=" + this.password + "&email=" + this.password);

            User user = null;

            if(!jsonString.Contains("exceptions"))
            {
                List<User> json = JsonConvert.DeserializeObject<List<User>>(jsonString);
                user = json[0];
            }

            return user;
        }

        public async Task<bool> saveScore(int score, int time_elapsed, int correct_answers)
        {
            string jsonString = await Statics.getJSONString(Statics.hostName + "/includes/jsons/save_score.php?user_id=" + this.id + "&user_access_token=" + this.access_token + "&score=" + score + "&time_elapsed=" + time_elapsed + "&correct_answers=" + correct_answers);

            List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonString);

            if (users.Count > 0)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
    }
}
