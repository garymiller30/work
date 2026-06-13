using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Interfaces.FileBrowser;

namespace JobSpace.UserForms.PDF
{
    public class PdfAssistantService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<string> AskAssistantAsync(
            PdfAssistantSettings settings,
            string userMessage,
            List<ToolInfo> availableTools,
            List<string> selectedFiles)
        {
            var systemPrompt = new StringBuilder();
            systemPrompt.AppendLine("Ти — розумний асистент препрес-додатку, який допомагає користувачеві підібрати правильну утиліту для обробки PDF.");
            systemPrompt.AppendLine("Ось список усіх доступних утиліт у програмі:");
            
            foreach (var tool in availableTools)
            {
                string desc = string.IsNullOrEmpty(tool.Meta.Description) ? "Опис відсутній" : tool.Meta.Description;
                systemPrompt.AppendLine($"- Назва: \"{tool.Meta.Name}\", Опис: \"{desc}\", ID: \"{tool.ToolType.FullName}\"");
            }
            
            systemPrompt.AppendLine();
            systemPrompt.AppendLine("Користувач виділив у файловому браузері наступні файли:");
            foreach (var file in selectedFiles)
            {
                systemPrompt.AppendLine($"- {System.IO.Path.GetFileName(file)}");
            }
            
            systemPrompt.AppendLine();
            systemPrompt.AppendLine("Твоє завдання:");
            systemPrompt.AppendLine("1. Проаналізуй запит користувача.");
            systemPrompt.AppendLine("2. Порадь найкращу утиліту зі списку вище та коротко поясни чому.");
            systemPrompt.AppendLine("3. В КІНЦІ своєї відповіді ОБОВ'ЯЗКОВО додай рядок у форматі: [SUGGESTED_TOOL: <ID>]");
            systemPrompt.AppendLine("   Де <ID> — це повний ID обраної утиліти (наприклад: [SUGGESTED_TOOL: JobSpace.Static.Pdf.Merge.PdfMerger]).");
            systemPrompt.AppendLine("4. Якщо під запит користувача жоден інструмент не підходить, просто поясни це та не додавай тег [SUGGESTED_TOOL].");

            var requestBody = new
            {
                model = settings.ModelName,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt.ToString() },
                    new { role = "user", content = userMessage }
                },
                temperature = 0.2
            };

            var requestContent = new StringContent(
                JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Clear();
            if (!string.IsNullOrEmpty(settings.ApiKey))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", settings.ApiKey);
            }

            try
            {
                var response = await _httpClient.PostAsync(settings.ApiUrl, requestContent);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                
                dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                string reply = result.choices[0].message.content;
                return reply.Trim();
            }
            catch (Exception ex)
            {
                return $"Помилка підключення до ШІ: {ex.Message}";
            }
        }
    }
}
