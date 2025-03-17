using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class WeatherRequest : IRequest
    {
        private const string WeatherApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            using var request = UnityWebRequest.Get(WeatherApiUrl);
            await request.SendWebRequest().WithCancellation(token);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Weather Data: {request.downloadHandler.text}");
            }
            else
            {
                Debug.LogError($"Weather API Error: {request.error}");
            }
        }
    }
}