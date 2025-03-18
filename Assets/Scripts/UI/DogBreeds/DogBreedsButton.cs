using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace.UI.DogBreeds
{
    public class DogBreedsButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _breedText;
        [SerializeField] private GameObject loadingIndicator;
        private string _breedId;

        public event Action<string, DogBreedsButton> OnClick;

        public void Initialize(string breedId, string breedName)
        {
            _breedId = breedId;
            _breedText.text = breedName;
            loadingIndicator.SetActive(false);
            _button.onClick.AddListener(() => OnClick(_breedId, this));
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void SetLoading(bool isLoading)
        {
            loadingIndicator.SetActive(isLoading);
            //_breedText.text = isLoading ? "Загрузка..." : _breedText.text;
            _button.interactable = !isLoading;
        }

        public void OnButtonClick()
        {
            OnClick?.Invoke(_breedId, this);
        }
    }
}