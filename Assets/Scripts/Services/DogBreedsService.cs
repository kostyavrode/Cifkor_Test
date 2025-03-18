using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace.Services
{
    public class DogBreedsService
    {
        private readonly RequestQueueManager _requestQueueManager;

        public DogBreedsService(RequestQueueManager requestQueueManager)
        {
            _requestQueueManager = requestQueueManager;
        }

        public async UniTask<List<DogBreed>> GetBreedsAsync(CancellationToken token)
        {
            var request = new DogBreedRequest(); 
            _requestQueueManager.EnqueueRequest(request);
            
            await request.CompletionSource.Task;
            // ✅ Дожидаемся выполнения запроса перед получением данных
            //await request.ExecuteAsync(token); 
    
            var breeds = request.GetBreedsDataAsync();
    
            Debug.Log($"📡 Получено {breeds?.Count} пород");
    
            return breeds ?? new List<DogBreed>(); // Возвращаем пустой список, если вдруг null
        }

        public UniTask<DogBreedInfo> GetBreedInfoAsync(string breedId, CancellationToken token)
        {
            var request = new DogBreedInfoRequest(breedId);
            _requestQueueManager.EnqueueRequest(request);
            return request.GetBreedInfoDataAsync(token);
        }
    }
}