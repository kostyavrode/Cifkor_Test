using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Services;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.UI.DogBreeds
{
    public class DogBreedsModel
    {
        private RequestQueueManager _requestQueueManager;

        [Inject]
        public void Construct(RequestQueueManager requestQueueManager)
        {
            _requestQueueManager = requestQueueManager;
        }
        
        public async UniTask<List<DogBreed>> GetBreedsAsync(CancellationToken cancelToken)
        {
            var request = new DogBreedRequest(); 
            _requestQueueManager.EnqueueRequest(request);
            
            await request.CompletionSource.Task;
    
            var breeds = request.GetBreedsDataAsync();
    
            return breeds ?? new List<DogBreed>();
        }
        
        public async UniTask<DogBreedInfo> GetBreedInfoAsync(string breedId, CancellationToken token)
        {
            var request = new DogBreedInfoRequest(breedId);

            var breedInfo = await request.GetBreedInfoDataAsync(token);

            if (breedInfo == null)
            {
                Debug.LogError("Не удалось получить информацию о породе:"+breedId);
            }

            return breedInfo;
        }
    }
}