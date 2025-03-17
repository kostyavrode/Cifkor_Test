﻿using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class DogBreedRequest : IRequest
    {
        private const string DogApiUrl = "https://dogapi.dog/api/v2/breeds";
        private List<DogBreed> _breeds;

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            using var request = UnityWebRequest.Get(DogApiUrl);
            await request.SendWebRequest().WithCancellation(token);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"📡 Получены данные: {request.downloadHandler.text}");

                try
                {
                    // ✅ Используем `Newtonsoft.Json` для парсинга API
                    var parsedData = JsonConvert.DeserializeObject<DogApiResponse>(request.downloadHandler.text);
                    if (parsedData?.Data == null)
                    {
                        Debug.LogError("❌ Ошибка парсинга JSON: список пуст!");
                        _breeds = new List<DogBreed>();
                        return;
                    }

                    // ✅ Преобразуем в нужную структуру `DogBreedList`
                    _breeds = new List<DogBreed>();
                    foreach (var breed in parsedData.Data)
                    {
                        _breeds.Add(new DogBreed
                        {
                            id = breed.Id,
                            name = breed.Attributes.Name
                        });
                    }

                    Debug.Log($"✅ Загружено {_breeds.Count} пород!");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"❌ Ошибка парсинга данных: {e.Message}");
                    _breeds = new List<DogBreed>();
                }
            }
            else
            {
                Debug.LogError($"❌ Ошибка загрузки пород: {request.error}");
                _breeds = new List<DogBreed>();
            }
        }
        
        public UniTask<List<DogBreed>> GetBreedsDataAsync()
        {
            return UniTask.FromResult(_breeds);
        }
    }
    
    public class DogBreedList
    {
        public List<DogBreed> breeds;
    }

    public class DogBreed
    {
        public string id;
        public string name;
    }
    
    public class DogApiResponse
    {
        [JsonProperty("data")]
        public List<DogBreedData> Data { get; set; }
    }

    public class DogBreedData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attributes")]
        public DogBreedAttributes Attributes { get; set; }
    }

    public class DogBreedAttributes
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}