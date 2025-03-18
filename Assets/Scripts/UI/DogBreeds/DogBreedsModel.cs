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
        private DogBreedRequest _currentRequest;
        private DogBreedInfoRequest _currentBreedInfoRequest;

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
    
            var breeds = request.GetBreedsData();
    
            _currentRequest = null;
            return breeds;
        }
        
        public async UniTask<DogBreedInfo> GetBreedInfoAsync(string breedId, CancellationToken token)
        {
            var request = new DogBreedInfoRequest(breedId);

            var breedInfo = await request.GetBreedInfoDataAsync(token);

            if (breedInfo == null)
            {
                Debug.LogError("Не удалось получить информацию о породе:"+breedId);
            }

            _currentBreedInfoRequest = null;
            return breedInfo;
        }
        
        public void CancelBreedRequest()
        {
            if (_currentRequest != null)
            _requestQueueManager.RemoveRequest(_currentRequest);
        }

        public void CancelBreedInfoRequest()
        {
            if (_currentBreedInfoRequest != null)
            _requestQueueManager.RemoveRequest(_currentBreedInfoRequest);
        }
    }
}