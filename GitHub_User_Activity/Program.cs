
using System.Text.Json;

HttpClient client = new HttpClient();
client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
client.DefaultRequestHeaders.Add("User-Agent", "MiAppConsole");
string baseUrl = "https://api.github.com/users/";

while (true)
{
    try
    {
        Console.Write("> ");
        var input = Console.ReadLine();

        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2 || !parts[0].Equals("github-activity", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("You mean: github-activity <username>");
            continue;
        }

        string user = parts[1].Trim('<', '>');
        string url = baseUrl + user + "/events";

        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        var eventos = JsonSerializer.Deserialize<List<Root>>(json);

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
        
    }
    catch (HttpRequestException e)
    {
        //Console.WriteLine($"Error: {e.Message}");
        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine("User not found. Please check username");
        }
        else
        {
            Console.WriteLine($"HTTP error code: {(int?)e.StatusCode}");
        }
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
