using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace
{
public class RequestQueueManager
{
    private readonly Queue<IRequest> _requestQueue = new(); // Очередь запросов
    private CancellationTokenSource _cts;
    private bool _isProcessing;

    /// <summary>
    /// Добавляет запрос в очередь.
    /// </summary>
    public void EnqueueRequest(IRequest request)
    {
        _requestQueue.Enqueue(request);
        ProcessQueue().Forget();
        Debug.Log("Добавлен процесс");
    }

    /// <summary>
    /// Удаляет конкретный запрос из очереди (если он еще не выполнялся).
    /// </summary>
    public bool RemoveRequest(IRequest request)
    {
        if (!_requestQueue.Contains(request)) return false; // Запроса нет - ничего не делаем

        var newQueue = new Queue<IRequest>(); // Создаем новую очередь без удаляемого элемента

        while (_requestQueue.Count > 0)
        {
            var dequeuedRequest = _requestQueue.Dequeue();
            if (dequeuedRequest != request) 
                newQueue.Enqueue(dequeuedRequest); // Добавляем только НЕ удаляемый запрос
        }

        _requestQueue.Clear();  // Очистка старой очереди
        foreach (var req in newQueue) 
            _requestQueue.Enqueue(req); // Перезапись очереди без удаленного запроса

        return true;
    }

    /// <summary>
    /// Отменяет текущий запрос и очищает очередь.
    /// </summary>
    public void CancelAllRequests()
    {
        _cts?.Cancel();
        _requestQueue.Clear();
        _isProcessing = false;
    }

    /// <summary>
    /// Запускает выполнение запросов по очереди.
    /// </summary>
    private async UniTaskVoid ProcessQueue()
    {
        if (_isProcessing) return;
        _isProcessing = true;

        while (_requestQueue.Count > 0)
        {
            var request = _requestQueue.Dequeue(); // Берем первый элемент
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