using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
public class WeatherUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text weatherText; // Привяжи UI-текст в инспекторе

    private const string WeatherApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
    private CancellationTokenSource _cts;

    private void OnEnable()
    {
        _cts = new CancellationTokenSource();
        StartWeatherUpdates(_cts.Token).Forget(); // Запускаем обновление погоды
    }

    private void OnDisable()
    {
        _cts?.Cancel(); // Останавливаем запросы при выключении объекта
    }

    /// <summary>
    /// Запрашивает погоду каждые 5 секунд.
    /// </summary>
    private async UniTaskVoid StartWeatherUpdates(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await GetWeatherAsync(token);
            await UniTask.Delay(5000, cancellationToken: token); // Ждем 5 секунд перед следующим запросом
        }
    }

    /// <summary>
    /// Делает запрос к API погоды и обновляет UI.
    /// </summary>
    private async UniTask GetWeatherAsync(CancellationToken token)
    {
        Debug.Log("GetWeatherAsync");
        using var request = UnityWebRequest.Get(WeatherApiUrl);
        await request.SendWebRequest().WithCancellation(token);

        if (request.result == UnityWebRequest.Result.Success)
        {
            var weatherInfo = ParseWeatherResponse(request.downloadHandler.text);
            weatherText.text = weatherInfo;
        }
        else
        {
            weatherText.text = "Ошибка загрузки погоды!";
            Debug.LogError($"Weather API Error: {request.error}");
        }
    }

    /// <summary>
    /// Парсит JSON и извлекает температуру.
    /// </summary>
    private string ParseWeatherResponse(string json)
    {
        var weatherData = JsonUtility.FromJson<WeatherResponse>(json);
        if (weatherData?.properties?.periods != null && weatherData.properties.periods.Length > 0)
        {
            var todayWeather = weatherData.properties.periods[0]; // Берем прогноз на сегодня
            return $"{todayWeather.name} - {todayWeather.temperature}°F";
        }

        return "Нет данных о погоде.";
    }
}
}