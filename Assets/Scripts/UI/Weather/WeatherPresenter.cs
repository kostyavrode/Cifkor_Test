using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Services;

namespace DefaultNamespace
{
    public class WeatherPresenter
    {
        private readonly WeatherModel _model;
        private readonly WeatherView _view;
        private readonly SpriteService _spriteService;
        private CancellationTokenSource _cts;

        public WeatherPresenter(WeatherModel model, WeatherView view, SpriteService spriteService)
        {
            _model = model;
            _view = view;
            _cts = new CancellationTokenSource();
            _spriteService = spriteService;
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