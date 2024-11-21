using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using WebApp.DTOs;
using DotNetEnv;

namespace WebApp.Services
{
    public interface IOpenAIService
    {
        Task<string> GetRecommendationsWithHistoryAndMenuAsync(List<Message> conversation, string userInput);
    }

    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IMenuService _menuService;
        private readonly string _apiKey;

        public OpenAIService(HttpClient httpClient, IMenuService menuService)
        {
            _httpClient = httpClient;
            _menuService = menuService;
            // Environment.GetEnvironmentVariable("OPENAI")
            // Set your OpenAI API key here
            _apiKey = Environment.GetEnvironmentVariable("OPENAI")
                  ?? throw new Exception("OpenAI API key not found in environment variables.");
        }

        public async Task<string> GetRecommendationsWithHistoryAndMenuAsync(List<Message> conversation, string userInput)
        {
            // Fetch menu from MenuService
            var menuResponse = await _menuService.GetAllMenuItemsAsync();
            var menuItems = menuResponse?.Data;

            if (menuItems == null || menuItems.Count == 0)
            {
                return "Sorry, I couldn't find any items on the menu right now. Please try again later.";
            }

            // Create menu context
            var menuContext = string.Join(", ", menuItems.Select(item => $"{item.Name} (${item.Price})"));

            // Construct conversation history for OpenAI
            var messages = new List<dynamic>
            {
                new
                {
                    role = "system",
                    content = $"You are a virtual waiter for Dine Master. Here's the menu: {menuContext}. Respond politely and helpfully to the user. You should talk like waiter and even add a joke in between if needed. Make it sound as humanly as possible. If the user asks for something unavailable, counter push alternatives from the menu. Be concise but friendly. Your response shouldn't sound like you are taking the order because we don't have that functionality now. Highlight the food item in bold"
                }
            };

            messages.AddRange(conversation.Select(msg => new
            {
                role = msg.IsUser ? "user" : "assistant",
                content = msg.Content
            }));

            messages.Add(new { role = "user", content = userInput });

            // Prepare the OpenAI API request
            var request = new
            {
                model = "gpt-4o",
                messages = messages,
                temperature = 0.7
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                return result?.choices[0]?.message?.content?.ToString();
            }

            var errorDetails = await response.Content.ReadAsStringAsync();
            throw new Exception($"OpenAI API Error: {response.StatusCode} - {errorDetails}");
        }
    }
}
