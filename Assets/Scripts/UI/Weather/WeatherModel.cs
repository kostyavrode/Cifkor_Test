using System.Threading;
using Cysharp.Threading.Tasks;

namespace DefaultNamespace
{
    public class WeatherModel
    {
        private readonly RequestQueueManager _requestQueueManager;
        private WeatherRequest _currentRequest;

        public WeatherModel(RequestQueueManager requestQueueManager)
        {
            _requestQueueManager = requestQueueManager;
        }

        public async UniTask<WeatherRequest.WeatherData> GetWeatherAsync()
        {
            var request = new WeatherRequest();
            _requestQueueManager.EnqueueRequest(request);
            
            await request.CompletionSource.Task;
            
            var weatherData = request.GetWeatherData();

            _currentRequest = null;
            return weatherData;
        }

        public void CancelWeatherRequest()
        {
            if (_currentRequest != null)
            _requestQueueManager.RemoveRequest(_currentRequest);
        }
    }
}