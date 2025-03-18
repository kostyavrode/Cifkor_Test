using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class DogBreedRequest : IRequest
    {
        private const string DogApiUrl = "https://dogapi.dog/api/v2/breeds";
        private List<DogBreed> _breeds;
        
        public UniTaskCompletionSource<bool> CompletionSource { get; } = new UniTaskCompletionSource<bool>();

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            using var request = UnityWebRequest.Get(DogApiUrl);
            var operation = request.SendWebRequest();

            try
            {
                await operation.WithCancellation(token);

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var parsedData = JsonConvert.DeserializeObject<DogApiResponse>(request.downloadHandler.text);
                    if (parsedData?.Data == null)
                    {
                        _breeds = new List<DogBreed>();
                        CompletionSource.TrySetResult(false);
                        return;
                    }

                    _breeds = new List<DogBreed>();
                    foreach (var breed in parsedData.Data)
                    {
                        _breeds.Add(new DogBreed
                        {
                            id = breed.Id,
                            name = breed.Attributes.Name
                        });
                    }

                    CompletionSource.TrySetResult(true);
                }
                else
                {
                    _breeds = new List<DogBreed>();
                    CompletionSource.TrySetException(new System.Exception(request.error));
                }
            }
            catch (OperationCanceledException)
            {
                CompletionSource.TrySetCanceled();
            }
            catch (System.Exception e)
            {
                _breeds = new List<DogBreed>();
                CompletionSource.TrySetException(e);
            }
        }

        public List<DogBreed> GetBreedsData()
        {
            return _breeds;
        }
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
