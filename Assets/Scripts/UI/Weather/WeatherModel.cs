using Cysharp.Threading.Tasks;
using Zenject;

namespace DefaultNamespace
{
    public class WeatherModel
    {
        private RequestQueueManager _requestQueueManager;
        private WeatherRequest _currentRequest;

        [Inject]
        public void Construct(RequestQueueManager requestQueueManager)
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