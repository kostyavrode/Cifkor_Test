using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    public class SpriteRequest : IRequest
    {
        private readonly string _url;
        private Sprite _result;
        private CancellationToken _token;

        public UniTaskCompletionSource<bool> CompletionSource { get; } = new UniTaskCompletionSource<bool>();

        public SpriteRequest(string url)
        {
            _url = url;
        }

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            _token = token;
            using var request = UnityWebRequestTexture.GetTexture(_url);
            await request.SendWebRequest().WithCancellation(_token);

            if (request.result == UnityWebRequest.Result.Success)
            {
                var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                _result = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                CompletionSource.TrySetResult(true);
            }
            else
            {
                CompletionSource.TrySetException(new System.Exception(request.error));
            }
        }

        public Sprite GetSprite() => _result;
    }
}