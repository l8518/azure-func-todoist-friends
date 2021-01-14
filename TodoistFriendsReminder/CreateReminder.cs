using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace TodoistFriendsReminder
{
    public static class CreateReminder
    {
        [FunctionName("CreateReminder")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            TodoistFriendsReminder.Lib.TodoistAPI api = new Lib.TodoistAPI();

            api.CreateTask();
        }
    }
}
