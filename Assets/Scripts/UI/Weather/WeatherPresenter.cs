using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace
{
    public class WeatherPresenter
    {
        private readonly WeatherModel _model;
        private readonly WeatherView _view;
        private CancellationTokenSource _cts;

        public WeatherPresenter(WeatherModel model, WeatherView view)
        {
            _model = model;
            _view = view;
            _cts = new CancellationTokenSource();
            StartUpdatingWeather().Forget();
        }

        private async UniTaskVoid StartUpdatingWeather()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                if (_view.gameObject.activeInHierarchy)
                {
                    var request = new WeatherRequest();
                    _model.EnqueueWeatherRequest(request);

                    var weatherData = await request.GetWeatherDataAsync(_cts.Token);

                    if (_view.gameObject.activeInHierarchy)
                    {
                        _view.UpdateWeather(weatherData);
                    }
                    else
                    {
                        _model.CancelWeatherRequest(request);
                    }
                }

                await UniTask.Delay(5000, cancellationToken: _cts.Token);
            }
        }
    }
}