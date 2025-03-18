using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class WeatherRequest : IRequest
    {
        private const string ApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast"; 
        private WeatherData _result;
        public UniTaskCompletionSource<bool> CompletionSource { get; } = new UniTaskCompletionSource<bool>();

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            using var request = UnityWebRequest.Get(ApiUrl);
            var operation = request.SendWebRequest();

            try
            {
                await operation.WithCancellation(token);

                if (request.result == UnityWebRequest.Result.Success)
                {
                    _result = ParseWeatherData(request.downloadHandler.text);
                    CompletionSource.TrySetResult(true);
                }
                else
                {
                    _result = null;
                    CompletionSource.TrySetException(new System.Exception(request.error));
                }
            }
            catch (OperationCanceledException)
            {
                CompletionSource.TrySetCanceled();
            }
            catch (System.Exception e)
            {
                _result = null;
                CompletionSource.TrySetException(e);
            }
        }

        public WeatherData GetWeatherData()
        {
            return _result;
        }

        private WeatherData ParseWeatherData(string json)
        {
            var data = JObject.Parse(json);
            var todayPeriod = data["properties"]["periods"][0];

            return new WeatherData(
                todayPeriod["temperature"].Value<int>(),
                todayPeriod["temperatureUnit"].Value<string>(),
                todayPeriod["icon"].Value<string>()
            );
        }

        public class WeatherData
        {
            public int Temperature { get; }
            public string TemperatureUnit { get; }
            public string IconUrl { get; }

            public WeatherData(int temperature, string temperatureUnit, string iconUrl)
            {
                Temperature = temperature;
                TemperatureUnit = temperatureUnit;
                IconUrl = iconUrl;
            }
        }
    }
}
