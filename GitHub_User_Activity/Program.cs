
using System.Net.Http.Headers;
using System.Text.Json;

HttpClient client = new HttpClient();

string url = "https://api.github.com/users/";

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

    Console.WriteLine(response);

    Console.WriteLine(responseBody);

}catch(Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}