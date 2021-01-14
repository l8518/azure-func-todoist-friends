using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TodoistFriendsReminder.Lib
{
    public class TodoistAPI
    {
        public class TaskModel
        {
            public long id { get; set; }
            public long project_id { get; set; }
            public long section_id { get; set; }
            public long parent_id { get; set; }
            public string content { get; set; }
            public long comment_count { get; set; }
            public long order { get; set; }
            public long priority { get; set; }
            public string url { get; set; }
        }

        // TODO: Make this the main object
        public class TaskList
        {
            public List<TaskModel> tasklist { get; set; }
        }

        private string APIKEY;

        public TodoistAPI()
        {

            this.APIKEY = Helpers.GetEnvironmentVariable("TAPIKEY");
        }

        public async void CreateTask()
        {
            var builder = new UriBuilder("https://api.todoist.com/rest/v1/tasks");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var request = new HttpRequestMessage(new HttpMethod("POST"), builder.ToString().Replace("\r\n", string.Empty)))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {this.APIKEY}");
                    request.Headers.Add("X-Request-Id", $"{Guid.NewGuid()}");

                    request.Content = new StringContent("{\"content\": \"Appointment with Maria\"}",Encoding.UTF8,
                                    "application/json");
                    // TODO: Other paramters:
                    // \"due_string\": \"tomorrow at 12:00\", \"due_lang\": \"en\", \"priority\": 4

                    Task<HttpResponseMessage> response = httpClient.SendAsync(request);

                    System.Console.WriteLine(response.Result.StatusCode);
                }

            }


        }

        public async Task<List<TaskModel>> fetch()
        {
            var builder = new UriBuilder("https://api.todoist.com/rest/v1/tasks");

            var query = HttpUtility.ParseQueryString(builder.Query);
            query["filter"] = "#Personal Contacts";
            builder.Query = query.ToString();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var request = new HttpRequestMessage(new HttpMethod("GET"), builder.ToString().Replace("\r\n", string.Empty)))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {this.APIKEY}");

                    Task<HttpResponseMessage> response = httpClient.SendAsync(request);

                    var resp = await response.Result.Content.ReadAsStringAsync();

                    Console.WriteLine("DEBUG");

                    List<TaskModel> tasks = JsonConvert.DeserializeObject<List<TaskModel>>(resp);

                    return tasks;
                }
            }
        }
    }

}
