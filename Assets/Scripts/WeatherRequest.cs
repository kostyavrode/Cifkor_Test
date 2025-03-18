using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class WeatherRequest : IRequest
    {
        private const string WeatherApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
        private WeatherData _weatherData;
        public UniTaskCompletionSource<bool> CompletionSource { get; } = new();

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            using var request = UnityWebRequest.Get(WeatherApiUrl);
            await request.SendWebRequest().WithCancellation(token);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"📡 Получены данные о погоде: {request.downloadHandler.text}");

                try
                {
                    var parsedData = JsonConvert.DeserializeObject<WeatherApiResponse>(request.downloadHandler.text);
                    _weatherData = new WeatherData
                    {
                        Temperature = parsedData?.Current?.TempC ?? 0,
                        Condition = parsedData?.Current?.Condition?.Text ?? "Неизвестно"
                    };

                    Debug.Log($"✅ Погода: {_weatherData.Temperature}°C, {_weatherData.Condition}");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"❌ Ошибка парсинга данных: {e.Message}");
                    _weatherData = new WeatherData();
                }
            }
            else
            {
                Debug.LogError($"❌ Ошибка загрузки погоды: {request.error}");
                _weatherData = new WeatherData();
            }

            CompletionSource.TrySetResult(true); // ✅ Сообщаем, что запрос выполнен
        }

        public UniTask<WeatherData> GetWeatherDataAsync()
        {
            return UniTask.FromResult(_weatherData);
        }
    }
    public class WeatherData
    {
        public float Temperature;
        public string Condition;
    }

    public class WeatherApiResponse
    {
        [JsonProperty("current")]
        public WeatherApiCurrent Current { get; set; }
    }

    public class WeatherApiCurrent
    {
        [JsonProperty("temp_c")]
        public float TempC { get; set; }

        [JsonProperty("condition")]
        public WeatherApiCondition Condition { get; set; }
    }

    public class WeatherApiCondition
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}