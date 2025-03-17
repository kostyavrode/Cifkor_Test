using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UI.DogBreeds;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.UI.DogBreeds
{
    public class DogBreedsPresenter : IInitializable, IDisposable
    {
        private DogBreedsView _view;
        private DogBreedsModel _model;
        private CancellationTokenSource _cts;
        private DogButtonFactory _factory;
        private readonly List<DogBreedsButton> _activeButtons = new();

        [Inject]
        public void Construct(DogBreedsView view, DogBreedsModel model, DogButtonFactory factory)
        {
            _view = view;
            _model = model;
            _factory = factory;
        }
        
        public void Initialize()
        {
            _view.OnBreedClicked += HandleBreedClicked;
            _view.OnViewActivated += OnViewActivated;
            LoadBreedsAsync().Forget();
        }

        public void Dispose()
        {
            _view.OnBreedClicked -= HandleBreedClicked;
        }

        private void OnViewActivated()
        {
            LoadBreedsAsync().Forget();
        }
        
        public async UniTaskVoid LoadBreedsAsync()
        {
            Debug.Log("📡 Начинаем загрузку пород...");

            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var breeds = await _model.GetBreedsAsync(_cts.Token);
            Debug.Log(breeds);

            Debug.Log($"✅ Загружено пород: {breeds.Count}");

            _view.ClearButtons();
            _activeButtons.Clear();

            for (int i = 0; i < 10; i++)
            {
                var button = _factory.Create();
                button.Initialize(breeds[i].id, breeds[i].name);
                _view.AddButton(button);
                _activeButtons.Add(button);
            }
        }
        
        private async void HandleBreedClicked(string breedId, DogBreedsButton button)
        {
            Debug.Log($"🟢 Нажата кнопка породы {breedId}, загружаем информацию...");
            button.SetLoading(true);
            var breedInfo = await _model.GetBreedInfoAsync(breedId, _cts.Token);
            button.SetLoading(false);

            if (breedInfo != null)
                _view.ShowPopup(breedInfo.name, breedInfo.description);
        }
    }
    
}