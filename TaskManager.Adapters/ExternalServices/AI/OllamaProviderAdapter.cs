using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.AI;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.ExternalServices.AI
{
    public class OllamaProviderAdapter : IOllamaProviderPort
    {
        private readonly HttpClient _http;
        private readonly OllamaSettings _settings;
        private readonly ILogger<OllamaProviderAdapter> _logger;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            // LLM às vezes devolve números como string ("1")
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        private static readonly Regex ThinkBlock = new(@"<think>.*?</think>", RegexOptions.Singleline | RegexOptions.Compiled);

        public OllamaProviderAdapter(HttpClient http, IOptions<OllamaSettings> settings, ILogger<OllamaProviderAdapter> logger)
        {
            _http = http;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<ResponseModel<T>> GenerateAsync<T>(string prompt)
        {
            var responseModel = new ResponseModel<T>();

            if (string.IsNullOrWhiteSpace(prompt))
            {
                responseModel.Status = ResponseStatusEnum.Error;
                responseModel.Message = "O prompt não pode ser nulo ou vazio.";
                return responseModel;
            }

            var request = new
            {
                model = _settings.Model,
                prompt,
                stream = false,
                format = "json",
                think = false,                    // qwen3 "pensando" + format=json costuma quebrar/atrasar a saída
                keep_alive = _settings.KeepAlive,
                options = new { temperature = _settings.Temperature, num_ctx = _settings.NumCtx }
            };

            try
            {
                using var httpResponse = await _http.PostAsJsonAsync("api/generate", request);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorBody = await httpResponse.Content.ReadAsStringAsync();
                    _logger.LogError("Ollama retornou {Status}: {Body}", (int)httpResponse.StatusCode, errorBody);

                    responseModel.Status = ResponseStatusEnum.CriticalError;
                    responseModel.Message = $"Ocorreu um erro ao se comunicar com o Ollama ({(int)httpResponse.StatusCode} - {httpResponse.ReasonPhrase}).";
                    return responseModel;
                }

                var ollamaResponse = await httpResponse.Content.ReadFromJsonAsync<OllamaDTO<string>>(JsonOptions);

                if (ollamaResponse is null || string.IsNullOrWhiteSpace(ollamaResponse.Response))
                {
                    responseModel.Status = ResponseStatusEnum.Error;
                    responseModel.Message = "O Ollama retornou uma resposta vazia.";
                    return responseModel;
                }

                var json = ExtractJson(ollamaResponse.Response);

                // T = string => devolve o texto limpo, sem tentar desserializar (antes: sempre JsonException)
                if (typeof(T) == typeof(string))
                {
                    responseModel.Content = (T)(object)json;
                    responseModel.Status = ResponseStatusEnum.Success;
                    return responseModel;
                }

                T? content;
                try
                {
                    content = JsonSerializer.Deserialize<T>(json, JsonOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "JSON inválido da IA: {Json}", json);
                    responseModel.Status = ResponseStatusEnum.Error;
                    responseModel.Message = "A IA retornou um JSON inválido. Tente novamente.";
                    return responseModel;
                }

                if (content is null)
                {
                    responseModel.Status = ResponseStatusEnum.Error;
                    responseModel.Message = "Não foi possível converter a resposta da IA para o tipo esperado.";
                    return responseModel;
                }

                responseModel.Status = ResponseStatusEnum.Success;
                responseModel.Content = content;
                return responseModel;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Falha de comunicação com o Ollama.");
                responseModel.Status = ResponseStatusEnum.CriticalError;
                responseModel.Message = "Falha de comunicação com o Ollama. Verifique se o serviço está acessível na rede.";
                return responseModel;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Timeout aguardando o Ollama.");
                responseModel.Status = ResponseStatusEnum.CriticalError;
                responseModel.Message = "Tempo limite excedido ao aguardar a resposta da IA.";
                return responseModel;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Falha ao interpretar o envelope do Ollama.");
                responseModel.Status = ResponseStatusEnum.CriticalError;
                responseModel.Message = "Falha ao interpretar a resposta do Ollama.";
                return responseModel;
            }
        }

        /// <summary>Remove blocos &lt;think&gt;, cercas ```json e texto antes/depois do JSON.</summary>
        internal static string ExtractJson(string raw)
        {
            var text = ThinkBlock.Replace(raw, string.Empty).Trim();
            var start = text.IndexOfAny(new[] { '{', '[' });
            var end = text.LastIndexOfAny(new[] { '}', ']' });
            return start >= 0 && end > start ? text[start..(end + 1)] : text;
        }
    }
}