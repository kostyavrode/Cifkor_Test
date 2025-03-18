using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI.DogBreeds
{
    public class DogBreedsView : MonoBehaviour
    {
        [SerializeField] private Transform _breedsContainer;
        [SerializeField] private GameObject popup;
        [SerializeField] private TMP_Text headerPopupText;
        [SerializeField] private TMP_Text popupText;
        
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
            headerPopupText.text = header;
            popupText.text = description;
            popup.SetActive(true);
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