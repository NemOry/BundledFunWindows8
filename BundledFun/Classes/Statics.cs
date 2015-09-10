using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BundledFun.Classes
{
    static class Statics
    {
        public static List<Question> questions = new List<Question>();
        public static User currentUser = null;
        public static Group currentGroup = null;

        public static string hostName = "http://localhost/BundledFun";

        public static void shuffleList<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static async Task<User> authenticate(string username, string password)
        {
            string jsonString = await getJSONString(hostName + "/includes/jsons/auth_user.php?username="+ username +"&password=" + password);
            List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonString);

            if (users.ElementAt(0).id != 0)
            {
                currentUser = users.ElementAt(0);
            }

            return currentUser;
        }

        public static async Task<Group> getGroup(int group_id)
        {
            string jsonString = await getJSONString(hostName + "/includes/jsons/get_group.php?group_id=" + group_id);
            List<Group> groups = JsonConvert.DeserializeObject<List<Group>>(jsonString);

            if (groups.Count > 0)
            {
                currentGroup = groups[0];
            }

            return currentGroup;
        }

        public static async Task<List<Group>> getAllGroups()
        {
            string jsonString = await getJSONString(hostName + "/includes/jsons/get_groups.php");
            List<Group> groups = JsonConvert.DeserializeObject<List<Group>>(jsonString);
            return groups;
        }

        public static async Task<bool> bindQuestions(int group_id)
        {
            string jsonString = await getJSONString(hostName + "/includes/jsons/get_questions.php?group_id=" + group_id);
            questions = JsonConvert.DeserializeObject<List<Question>>(jsonString);
            return true;
        }

        public static async Task<string> getJSONString(String url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public static List<Question> getQuestionsByDifficulty(string difficulty)
        {
            List<Question> this_questions = new List<Question>();

            if (difficulty.ToLower().Equals("mixed".ToLower()))
            {
                foreach (Question q in Statics.questions)
                {
                    this_questions.Add(q);
                }

                return this_questions;
            }
            else
            {
                foreach (Question q in Statics.questions)
                {
                    if (q.difficulty.ToLower().Equals(difficulty.ToLower()))
                    {
                        this_questions.Add(q);
                    }
                }

                return this_questions;
            }
        }
    }
}
