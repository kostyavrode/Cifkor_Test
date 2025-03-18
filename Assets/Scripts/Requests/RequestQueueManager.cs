using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace
{
public class RequestQueueManager
{
    private readonly Queue<IRequest> _requestQueue = new();
    private CancellationTokenSource _cts;
    private bool _isProcessing;
    
    public void EnqueueRequest(IRequest request)
    {
        _requestQueue.Enqueue(request);
        ProcessQueue().Forget();
        Debug.Log("Добавлен процесс");
    }
    
    public bool RemoveRequest(IRequest request)
    {
        if (!_requestQueue.Contains(request)) return false;

        var newQueue = new Queue<IRequest>();

        while (_requestQueue.Count > 0)
        {
            var dequeuedRequest = _requestQueue.Dequeue();
            if (dequeuedRequest != request) 
                newQueue.Enqueue(dequeuedRequest);
        }

        _requestQueue.Clear();
        foreach (var req in newQueue) 
            _requestQueue.Enqueue(req);

        return true;
    }
    
    public void CancelAllRequests()
    {
        _cts?.Cancel();
        _requestQueue.Clear();
        _isProcessing = false;
    }
    
    private async UniTaskVoid ProcessQueue()
    {
        if (_isProcessing) return;
        _isProcessing = true;

        while (_requestQueue.Count > 0)
        {
            var request = _requestQueue.Dequeue();
            _cts = new CancellationTokenSource();

            try
            {
                await request.ExecuteAsync(_cts.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Запрос отменен");
            }
        }
        _isProcessing = false;
    }
}
}