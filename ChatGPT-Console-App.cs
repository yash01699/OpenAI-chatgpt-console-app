using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

string openAIEndpoint = "https://api.openai.com/v1/completions";
var openAIKey = "YOUR-API-KEY"; //add your personal API key here

using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAIKey);

string prompt = generatePrompt();
while(!prompt.Equals("stop",StringComparison.OrdinalIgnoreCase))
{
    var requestBody = new { prompt = prompt, max_tokens = 100, temperature = 0.5, model = "text-davinci-002" }; // you can change the model and the temperature also according to your preference
    var requestBodyJson = JsonConvert.SerializeObject(requestBody);
    var content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");
    
    var response = await httpClient.PostAsync(openAIEndpoint, content);
    response.EnsureSuccessStatusCode();

    var responseBodyJson = await response.Content.ReadAsStringAsync();
    var openAIResponse = JsonConvert.DeserializeObject<OpenAIResponse>(responseBodyJson);

    Console.WriteLine(openAIResponse.Choices[0].Text);
    prompt = generatePrompt();
}

string generatePrompt()
{
    string result = "stop";
    Console.WriteLine("Enter your prompt:");
    result = Console.ReadLine();
    return result;
}

public class OpenAIResponse
{
    public List<Choice> Choices { get; set; }
    public string ErrorMessage { get; set; }

    public class Choice
    {
        public string Text { get; set; }
        public double Score { get; set; }
    }
}
