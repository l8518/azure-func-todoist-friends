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
        public static async void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer,
            [Blob("friends/friends.json", FileAccess.Write)] TextWriter friendsOutput, ILogger log)
        {
            // Basic API-key Based Todoist API
            TodoistAPI tapi = new TodoistAPI();

            List<TodoistAPI.TaskModel> friends = await tapi.fetch();

            // Write To JSON
            foreach (TodoistAPI.TaskModel task in friends)
            {
                log.LogInformation($"todoist task: {task.content}");
            }

            // Push To Blob?
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // Should push to blob storage.
            string json = JsonConvert.SerializeObject(friends.ToArray());
            friendsOutput.WriteLine(json);
        }

    }
}
