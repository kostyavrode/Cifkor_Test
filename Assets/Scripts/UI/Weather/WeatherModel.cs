using System.Threading;
using Cysharp.Threading.Tasks;

namespace DefaultNamespace
{
    public class WeatherModel
    {
        private readonly RequestQueueManager _requestQueueManager;

        public WeatherModel(RequestQueueManager requestQueueManager)
        {
            _requestQueueManager = requestQueueManager;
        }

        public void EnqueueWeatherRequest(WeatherRequest request)
        {
            _requestQueueManager.EnqueueRequest(request);
        }

        public void CancelWeatherRequest(WeatherRequest request)
        {
            _requestQueueManager.RemoveRequest(request);
        }
    }
}