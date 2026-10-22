namespace TaskManager.Adapters.ExternalServices.AI
{
    public class OllamaSettings
    {
        public string BaseUrl { get; set; } = "http://localhost:11434/";
        public string Model { get; set; } = "qwen3:8b";
        public int TimeoutSeconds { get; set; } = 300;   // HttpClient padrão é 100s: pouco para 8B em CPU
        public string KeepAlive { get; set; } = "30m";   // mantém o modelo na memória (evita cold start a cada chamada)
        public int NumCtx { get; set; } = 8192;          // padrão do Ollama é baixo e trunca o prompt em silêncio
        public double Temperature { get; set; } = 0.2;   // saída estruturada: menos criatividade
    }
}