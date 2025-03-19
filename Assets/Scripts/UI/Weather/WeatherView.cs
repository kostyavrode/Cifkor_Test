using System;
using DefaultNamespace.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class WeatherView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _temperatureText;
        [SerializeField] private Image _weatherIcon;
        
        public event Action OnViewActivated;

        private void OnEnable()
        {
            OnViewActivated?.Invoke();
        }

        public void UpdateWeather(WeatherRequest.WeatherData data, Sprite sprite)
        {
            _temperatureText.text = "Сегодня "+data.Temperature+data.TemperatureUnit;
            _weatherIcon.sprite = sprite;
        }
    }
}