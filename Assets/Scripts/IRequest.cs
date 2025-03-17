using System.Threading;
using Cysharp.Threading.Tasks;

namespace DefaultNamespace
{
    public interface IRequest
    {
        UniTask ExecuteAsync(CancellationToken token);
    }
}