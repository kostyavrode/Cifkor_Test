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

        public async UniTask<WeatherData> GetWeatherAsync(CancellationToken token)
        {
            var request = new WeatherRequest();
            _requestQueueManager.EnqueueRequest(request);

            await request.CompletionSource.Task; // Ждём выполнения запроса
            return await request.GetWeatherDataAsync();
        }
    }
}