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
            using var request = UnityWebRequest.Get(ApiUrl + _breedId);
            await request.SendWebRequest().WithCancellation(token);

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var parsedData = JsonConvert.DeserializeObject<DogBreedApiResponse>(request.downloadHandler.text);

                    if (parsedData?.Data == null)
                    {
                        _result = null;
                        return;
                    }
                    
                    _result = new DogBreedInfo
                    {
                        id = parsedData.Data.Id,
                        name = parsedData.Data.Attributes.Name,
                        description = parsedData.Data.Attributes.Description
                    };
                    
                }
                catch (System.Exception e)
                {
                    _result = null;
                }
            }
            else
            {
                _result = null;
            }
        }

        public async UniTask<DogBreedInfo> GetBreedInfoDataAsync(CancellationToken token)
        {
            await ExecuteAsync(token);

            return _result;
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