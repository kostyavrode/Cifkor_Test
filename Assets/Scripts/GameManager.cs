using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        private RequestQueueManager _requestQueueManager;

        private void Start()
        {
            _requestQueueManager = new RequestQueueManager();
        
            // Запрос погоды
            _requestQueueManager.EnqueueRequest(new WeatherRequest());
        

            //_requestQueueManager.EnqueueRequest(new DogBreedRequest());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _requestQueueManager.EnqueueRequest(new WeatherRequest());
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                _requestQueueManager.EnqueueRequest(new DogBreedRequest());
            }

            if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.W))
            {
                _requestQueueManager.RemoveRequest(new WeatherRequest());
            }
            if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.D))
            {
                _requestQueueManager.RemoveRequest(new DogBreedRequest());
            }
        }

        private void OnDestroy()
        {
            _requestQueueManager.CancelAllRequests();
        }
    }
}