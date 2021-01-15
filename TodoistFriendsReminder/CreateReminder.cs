using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TodoistFriendsReminder.Lib;

namespace TodoistFriendsReminder
{
    public static class CreateReminder
    {
        [FunctionName("CreateReminder")]
        public static void Run(
            [TimerTrigger("%REMINDER_SCHEDULE%")]TimerInfo myTimer,
            [Blob("friends/friends.json", FileAccess.Read)] TextReader friendsReader,
            ILogger log)
        {
            
            var random = new Random();
            TodoistAPI api = new Lib.TodoistAPI();

            List<Friend> friends = JsonConvert.DeserializeObject<List<Friend>>(friendsReader.ReadToEnd());
            int index = random.Next(friends.Count);
            api.CreateTask($"Contact {friends[index].name}");

        }
    }
}
