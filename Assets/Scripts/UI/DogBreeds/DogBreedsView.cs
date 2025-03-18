using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI.DogBreeds
{
    public class DogBreedsView : MonoBehaviour
    {
        [SerializeField] private Transform _breedsContainer;
        [SerializeField] private GameObject _popup;
        [SerializeField] private TMP_Text _headerPopupText;
        [SerializeField] private TMP_Text _popupText;
        
        public event Action<string, DogBreedsButton> OnBreedClicked;
        public event Action OnViewActivated;
        
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
            _headerPopupText.text = header;
            _popupText.text = description;
            _popup.SetActive(true);
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