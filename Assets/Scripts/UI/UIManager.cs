using DefaultNamespace.Services;
using DefaultNamespace.UI.DogBreeds;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject dogBreedsScreen;
        [SerializeField] private DogBreedsButton dogButtonPrefab;
        [SerializeField] private Transform buttonContainer;

        private DogBreedsPresenter _dogBreedsPresenter;

        private void Start()
        {
        }
    }
}