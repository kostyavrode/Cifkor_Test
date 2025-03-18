using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace
{
    public class WeatherPresenter
    {
        private readonly WeatherModel _model;
        private readonly WeatherView _view;
        private readonly CancellationTokenSource _cts = new();

        public WeatherPresenter(WeatherModel model, WeatherView view)
        {
            _model = model;
            _view = view;
            StartUpdatingWeather();
        }

        public void StartUpdatingWeather()
        {
            UpdateWeatherLoop(_cts.Token).Forget();
        }

        public void StopUpdatingWeather()
        {
            _cts.Cancel();
        }

        private async UniTaskVoid UpdateWeatherLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var weather = await _model.GetWeatherAsync(token);
                _view.UpdateWeatherText($"🌤 {weather.Temperature}°C, {weather.Condition}");

                await UniTask.Delay(5000, cancellationToken: token); // ⏳ Ждём 5 секунд
            }
        }
    }
}