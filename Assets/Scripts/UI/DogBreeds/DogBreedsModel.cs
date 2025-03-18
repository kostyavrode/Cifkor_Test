using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
            _currentRequest = request;

            _requestQueueManager.EnqueueRequest(request);

            try
            {
                await request.CompletionSource.Task.AttachExternalCancellation(cancelToken);
        
                var breeds = request.GetBreedsData();

                _currentRequest = null;
                return breeds ?? new List<DogBreed>();
            }
            catch (OperationCanceledException)
            {
                return new List<DogBreed>();
            }
        }
        
        public async UniTask<DogBreedInfo> GetBreedInfoAsync(string breedId, CancellationToken token)
        {
            var request = new DogBreedInfoRequest(breedId);
            _currentBreedInfoRequest = request;

            _requestQueueManager.EnqueueRequest(request);

            try
            {
                var breedInfo = await request.GetBreedInfoDataAsync(token);

                _currentBreedInfoRequest = null;
                return breedInfo;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
        
        public void CancelBreedRequest()
        {
            if (_currentRequest != null)
            {
                _requestQueueManager.RemoveRequest(_currentRequest);
                _currentRequest = null;
            }
        }

        public void CancelBreedInfoRequest()
        {
            if (_currentBreedInfoRequest != null)
            {
                _requestQueueManager.RemoveRequest(_currentBreedInfoRequest);
                _currentBreedInfoRequest = null;
            }
        }
    }
}