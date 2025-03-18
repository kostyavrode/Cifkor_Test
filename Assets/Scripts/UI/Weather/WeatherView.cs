using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class WeatherView : MonoBehaviour
    {
        [SerializeField] private TMP_Text weatherText;

        public void UpdateWeatherText(string text)
        {
            weatherText.text = text;
        }
    }
}