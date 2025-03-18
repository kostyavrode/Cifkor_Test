using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace.Services
{
    public class SpriteService
    {
        public async void LoadSprite(string url, Action<Sprite> onLoaded)
        {
            using var request = UnityWebRequestTexture.GetTexture(url);
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                onLoaded?.Invoke(sprite);
            }
            else
            {
                Debug.LogError($"❌ Ошибка загрузки изображения: {request.error}");
            }
        }
    }
}