using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
namespace Device_Management_System_Backend.Services
{
    public class AIDescriptionService : IAIDescriptionService
    {
        private readonly ChatClient _client;

        public AIDescriptionService(IConfiguration config)
        {
            var apiKey = Environment.GetEnvironmentVariable("Hf_Api_Key");

            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri("https://router.huggingface.co/v1/")
            };

            var client = new OpenAIClient(new ApiKeyCredential(apiKey!), options);

            _client = client.GetChatClient("meta-llama/Llama-3.1-8B-Instruct");
        }

        public async Task<string> GenerateDeviceDescriptionAsync(string name, string manufacturer, string type, string os, string ram, string processor)
        {
            string prompt = $"Generate a one-sentence professional description for a {manufacturer} {name} ({type}) with {ram} RAM and {processor} running {os}. Return only the description.";

            try
            {
                ChatCompletion completion = await _client.CompleteChatAsync(prompt);
                return completion.Content[0].Text.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "A high-performance device tailored for professional use.";
            }
        }
    }
}
