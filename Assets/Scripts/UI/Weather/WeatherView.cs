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
        private readonly SpriteService _spriteService = new SpriteService();

        public void UpdateWeather(WeatherRequest.WeatherData data)
        {
            temperatureText.text = "Сегодня "+data.Temperature+data.TemperatureUnit;
            _spriteService.LoadSprite(data.IconUrl, sprite => weatherIcon.sprite = sprite);
        }
    }
}