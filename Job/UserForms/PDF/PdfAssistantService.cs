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

            // ОБОВ'ЯЗКОВО для OpenRouter (захищає від 403/401 помилок на деяких моделях)
            _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "https://localhost");
            _httpClient.DefaultRequestHeaders.Add("X-Title", "PrepressJobSpaceAssistant");

            try
            {
                // Переконайтеся, що settings.ApiUrl закінчується на /chat/completions
                var response = await _httpClient.PostAsync(settings.ApiUrl, requestContent);

                // Зчитуємо сирий текст відповіді ДО перевірки на помилку status code
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // Якщо OpenRouter повернув помилку (наприклад, 400 чи 401), ми побачимо її JSON-опис
                    return $"Помилка API ({response.StatusCode}): {jsonResponse}";
                }

                // Якщо все успішно, парсимо JSON
                dynamic result = JsonConvert.DeserializeObject(jsonResponse);

                if (result?.choices == null || result.choices.Count == 0)
                {
                    return "Помилка: OpenRouter повернув порожню відповідь (choices відсутні).";
                }

                string reply = result.choices[0].message.content;
                return reply.Trim();
            }
            catch (JsonReaderException)
            {
                return $"Помилка: Сервер повернув HTML замість JSON. Перевірте, чи правильний API URL вказано: {settings.ApiUrl}";
            }
            catch (Exception ex)
            {
                return $"Помилка підключення до ШІ: {ex.Message}";
            }
        }

        /// <summary>
        /// Sends a WAV audio file to a Whisper-compatible transcription endpoint
        /// and returns the recognised text.
        /// </summary>
        public async Task<string> TranscribeAudioAsync(PdfAssistantSettings settings, string wavFilePath)
        {
            var url = string.IsNullOrWhiteSpace(settings.SttUrl)
                ? settings.ApiUrl.Replace("/chat/completions", "/audio/transcriptions")
                : settings.SttUrl;

            using (var content = new MultipartFormDataContent())
            {
                var fileBytes = System.IO.File.ReadAllBytes(wavFilePath);
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType =
                    System.Net.Http.Headers.MediaTypeHeaderValue.Parse("audio/wav");
                content.Add(fileContent, "file", "audio.wav");
                content.Add(new StringContent("whisper-1"), "model");

                _httpClient.DefaultRequestHeaders.Clear();
                if (!string.IsNullOrEmpty(settings.ApiKey))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", settings.ApiKey);
                }

                try
                {
                    var response = await _httpClient.PostAsync(url, content);
                    var json = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        return $"Помилка транскрипції ({response.StatusCode}): {json}";

                    dynamic result = JsonConvert.DeserializeObject(json);
                    string text = result.text;
                    return text?.Trim() ?? string.Empty;
                }
                catch (Exception ex)
                {
                    return $"Помилка транскрипції аудіо: {ex.Message}";
                }
            }
        }
    }
}
