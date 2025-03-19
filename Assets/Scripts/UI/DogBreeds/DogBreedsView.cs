using System;
using TMPro;
using UI.Popup;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.UI.DogBreeds
{
    public class DogBreedsView : MonoBehaviour
    {
        [SerializeField] private Transform _breedsContainer;
        private PopupView _popup;
        
        public event Action<string, DogBreedsButton> OnBreedClicked;
        public event Action OnViewActivated;

        [Inject]
        public void Construct(PopupView popupView)
        {
            _popup = popupView;
        }
        
        private void OnEnable()
        {
            OnViewActivated?.Invoke();
        }

        public void AddButton(DogBreedsButton button)
        {
            button.transform.SetParent(_breedsContainer,false);
            button.gameObject.SetActive(true);
            button.OnClick += (breedId, btn) => OnBreedClicked?.Invoke(breedId, btn);
        }

        public void ShowPopup(string header,string description)
        {
            _popup.Show(header, description);
        }

        public void ClearButtons()
        {
            foreach (Transform child in _breedsContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}