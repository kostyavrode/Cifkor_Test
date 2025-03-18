using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace.Services
{
    public class SpriteService
    {
        public async UniTask<Sprite> LoadSpriteAsync(string url)
        {
            using var request = UnityWebRequestTexture.GetTexture(url);
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }

            Debug.LogError($"Ошибка загрузки изображения: {request.error}");
            return null;
        }
    }
}