using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class DogBreedInfoRequest : IRequest
    {
        private readonly string _breedId;
        private const string ApiUrl = "https://dogapi.dog/api/v2/breeds/";
        private DogBreedInfo _result;

        public DogBreedInfoRequest(string breedId)
        {
            _breedId = breedId;
        }

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            Debug.Log($"📡 Запрашиваем информацию о породе {_breedId}...");

            using var request = UnityWebRequest.Get(ApiUrl + _breedId);
            await request.SendWebRequest().WithCancellation(token);

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"✅ Успешный ответ от API, начинаем обработку...");

                try
                {
                    // ✅ Парсим JSON через `Newtonsoft.Json`
                    var parsedData = JsonConvert.DeserializeObject<DogBreedApiResponse>(request.downloadHandler.text);

                    if (parsedData?.Data == null)
                    {
                        Debug.LogError($"❌ Ошибка: JSON не содержит данных!");
                        _result = null;
                        return;
                    }

                    // ✅ Преобразуем API-ответ в `DogBreedInfo`
                    _result = new DogBreedInfo
                    {
                        id = parsedData.Data.Id,
                        name = parsedData.Data.Attributes.Name,
                        description = parsedData.Data.Attributes.Description
                    };

                    Debug.Log($"📌 Найдена порода: {_result.name}");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"❌ Ошибка парсинга данных: {e.Message}");
                    _result = null;
                }
            }
            else
            {
                Debug.LogError($"❌ Ошибка загрузки данных: {request.error}");
                _result = null;
            }
        }

        public UniTask<DogBreedInfo> GetBreedInfoDataAsync(CancellationToken token)
        {
            return UniTask.FromResult(_result);
        }
    }

    [System.Serializable]
    public class DogBreedInfoList
    {
        public List<DogBreedInfo> breedsInfo;
    }
    [System.Serializable]
    public class DogBreedInfoAttributes
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
    
    public class DogBreedApiResponse
    {
        [JsonProperty("data")]
        public DogBreedData Data { get; set; }
    }

    [System.Serializable]
    public class DogBreedInfo
    {
        public string id;
        public string name;
        public string description;
    }
}