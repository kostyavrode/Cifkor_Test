using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace.Services
{
    public class SpriteService
    {
        private readonly RequestQueueManager _requestQueueManager;

        public SpriteService(RequestQueueManager requestQueueManager)
        {
            _requestQueueManager = requestQueueManager;
        }

        public async UniTask<Sprite> LoadSpriteAsync(string url, CancellationToken token)
        {
            var request = new SpriteRequest(url);
            _requestQueueManager.EnqueueRequest(request);

            try
            {
                await request.CompletionSource.Task.AttachExternalCancellation(token);
                return request.GetSprite();
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
    }
}