using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Services;
using Zenject;

namespace DefaultNamespace
{
    public class WeatherPresenter : IDisposable
    {
        private WeatherModel _model;
        private WeatherView _view;
        private SpriteService _spriteService;
        private CancellationTokenSource _cts;

        [Inject]
        public void Construct(WeatherModel model, WeatherView view, SpriteService spriteService)
        {
            _model = model;
            _view = view;
            _spriteService = spriteService;
            _cts = new CancellationTokenSource();
            _view.OnViewActivated += StartWeather;
        }
        
        public void Dispose()
        {
            _view.OnViewActivated -= StartWeather;
        }

        private void StartWeather()
        {
            StartUpdatingWeather().Forget();
        }

        private async UniTaskVoid StartUpdatingWeather()
        {
            _cts = new CancellationTokenSource();

            while (_view.gameObject.activeInHierarchy)
            {
                var weatherData = await _model.GetWeatherAsync();
                if (weatherData != null)
                {
                    var sprite = await _spriteService.LoadSpriteAsync(weatherData.IconUrl,_cts.Token);
                    _view.UpdateWeather(weatherData, sprite);
                }

                await UniTask.Delay(5000, cancellationToken: _cts.Token);
            }

            _cts.Cancel();
        }
    }
}