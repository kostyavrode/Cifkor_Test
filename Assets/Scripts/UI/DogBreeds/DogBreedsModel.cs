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
        private DogBreedsService _dogBreedsService;
        private RequestQueueManager _requestQueueManager;

        [Inject]
        public void Construct(DogBreedsService dogBreedsService,RequestQueueManager requestQueueManager)
        {
            _dogBreedsService = dogBreedsService;
            _requestQueueManager = requestQueueManager;
        }
        
        public UniTask<List<DogBreed>> GetBreedsAsync(CancellationToken cancelToken)
        {
            return _dogBreedsService.GetBreedsAsync(cancelToken);
        }
        
        public async UniTask<DogBreedInfo> GetBreedInfoAsync(string breedId, CancellationToken token)
        {
            Debug.Log($"📡 Запрос информации о породе {breedId}...");
    
            var request = new DogBreedInfoRequest(breedId);
            //_requestQueueManager.EnqueueRequest(request);

            var breedInfo = await request.GetBreedInfoDataAsync(token);

            if (breedInfo == null)
            {
                Debug.LogError($"❌ Не удалось получить информацию о породе {breedId}");
            }
            else
            {
                Debug.Log($"✅ Получена информация: {breedInfo.name}");
            }

            return breedInfo;
        }
    }
}