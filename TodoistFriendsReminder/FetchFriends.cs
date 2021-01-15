using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.WebJobs;
using System.Net.Http.Formatting;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System.IO;
using TodoistFriendsReminder.Lib;

namespace TodoistFriendsReminder
{
    public static class FetchFriends
    {
        [FunctionName("FetchFriends")]
        public static async void Run([TimerTrigger("%FETCH_SCHEDULE%")] TimerInfo myTimer,
            [Blob("friends/friends.json", FileAccess.Write)] TextWriter friendsOutput, ILogger log)
        {
            // Basic API-key Based Todoist API
            TodoistAPI tapi = new TodoistAPI();

            List<TodoistAPI.TaskModel> tasks = await tapi.fetch();
            List<Friend> friends = new List<Friend>();
            // Write To JSON
            foreach (TodoistAPI.TaskModel task in tasks)
            {
                if (task.content.StartsWith("* ")) {
                    var friend = new Friend(task.content.Remove(0, 2));
                    friends.Add(friend);
                }
            }

            // Write to JSON
            string json = JsonConvert.SerializeObject(friends.ToArray());
            friendsOutput.WriteLine(json);
        }

    }
}
