using DefaultNamespace.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class WeatherView : MonoBehaviour
    {
        [SerializeField] private TMP_Text temperatureText;
        [SerializeField] private Image weatherIcon;

        public void UpdateWeather(WeatherRequest.WeatherData data, Sprite sprite)
        {
            temperatureText.text = $"Сегодня {data.Temperature}{data.TemperatureUnit}";
            weatherIcon.sprite = sprite;
        }
    }
}