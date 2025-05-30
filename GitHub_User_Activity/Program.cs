
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

HttpClient client = new HttpClient();

string url = "https://api.github.com/users/";


while (true)
{

    Console.WriteLine("Insert username");
    var user = Console.ReadLine();

    url = url + user + "/events";
    try
    {
        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
        client.DefaultRequestHeaders.Add("User-Agent", "MiAppConsole");

        //client.DefaultRequestHeaders.Add("per_page", "30");
        //client.DefaultRequestHeaders.Add("Page", "1");

        HttpResponseMessage response = client.GetAsync(url).Result;


        Console.WriteLine(response);

        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        //Console.WriteLine(responseBody);

        List<Root> myDeserializedClass = JsonSerializer.Deserialize<List<Root>>(responseBody);

        //Root root1 = JsonSerializer.Deserialize<Root>(responseBody);
        //Console.WriteLine(root1.repo.name);

        //var cantComits = 0;
        foreach (var item in myDeserializedClass)
        {

            //Console.WriteLine(item.payload.commits);
            //Console.WriteLine("***");
            if (item.payload.commits != null)
            {
                foreach (var commit in item.payload.commits)
                {
                    Console.WriteLine(commit.message);
                    Console.WriteLine(commit.url);
                }
            }

        }
        //foreach (Root root in myDeserializedClass)
        //{
        //    Console.WriteLine(root.id);
        //    Console.WriteLine(root.repo.name);
        //    //Console.WriteLine($"Pushed {} commits to {}");
        //}

        //List<string> output = JsonSerializer.Deserialize<string>(response.Content);


        //Console.WriteLine(output);

        //Console.WriteLine(response);

        //Console.WriteLine(responseBody);


        var eventos = JsonSerializer.Deserialize<List<Root>>(responseBody);

        foreach (var item in eventos)
        {
            string repo = item.repo.name;
            string mensaje = null;
            //if(item.payload?.commits?.Count != null)
            //{
            //    Console.WriteLine($"Pushed {item.payload?.commits?.Count} commits to {repo}");
            //}

            switch (item.type)
            {
                case "PushEvent":
                    int cantCommits = item.payload?.commits?.Count ?? 0;
                    mensaje = $"- Pushed {cantCommits} commit(s) to {repo}";
                    break;
                case "IssuesEvent":
                    if (item.payload?.action == "opened")
                    {
                        mensaje = $"- Opened a new issue in {repo}";
                    }
                    break;

                case "WatchEvent":
                    if (item.payload?.action == "started")
                    {
                        mensaje = $"- Starred {repo}";
                    }
                    break;

                default:
                    break;
            }

            if (!string.IsNullOrEmpty(mensaje))
            {
                Console.WriteLine(mensaje);
            }

        }

        //foreach (var evento in eventos)
        //{
        //    string repo = evento.repo.name;
        //    string mensaje = evento.type switch
        //    {
        //        "PushEvent" => $"- Pushed {evento.payload?.commits?.Count ?? 0} commit(s) to {repo}",
        //        "IssuesEvent" when evento.payload?.action == "opened" => $"- Opened a new issue in {repo}",
        //        "WatchEvent" when evento.payload?.action == "started" => $"- Starred {repo}",
        //        _ => null // O ignora otros eventos
        //    };

        //    if (!string.IsNullOrEmpty(mensaje))
        //    {
        //        Console.WriteLine(mensaje);
        //    }
        //}
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

public class Actor
{
    public int id { get; set; }
    public string login { get; set; }
    public string display_login { get; set; }
    public string gravatar_id { get; set; }
    public string url { get; set; }
    public string avatar_url { get; set; }
}

public class Author
{
    public string email { get; set; }
    public string name { get; set; }
}

public class Commit
{
    public string sha { get; set; }
    public Author author { get; set; }
    public string message { get; set; }
    public bool distinct { get; set; }
    public string url { get; set; }
}

public class Payload
{
    public string @ref { get; set; }
    public string ref_type { get; set; }
    public string master_branch { get; set; }
    public string description { get; set; }
    public string pusher_type { get; set; }
    public int? repository_id { get; set; }
    public long? push_id { get; set; }
    public int? size { get; set; }
    public int? distinct_size { get; set; }
    public string head { get; set; }
    public string before { get; set; }
    public List<Commit> commits { get; set; }
    public string action { get; set; }
}

public class Repo
{
    public int id { get; set; }
    public string name { get; set; }
    public string url { get; set; }
}

public class Root
{
    public string id { get; set; }
    public string type { get; set; }
    public Actor actor { get; set; }
    public Repo repo { get; set; }
    public Payload payload { get; set; }
    public bool @public { get; set; }
    public DateTime created_at { get; set; }
}
