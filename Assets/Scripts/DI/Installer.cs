using DefaultNamespace.Services;
using DefaultNamespace.UI.DogBreeds;
using UI.DogBreeds;
using UI.Popup;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DefaultNamespace.DI
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private WeatherView _weatherView;
        [SerializeField] private PopupView _popupView;
        [SerializeField] private GameObject _dogBreedsScreen;
        [SerializeField] private DogBreedsButton _dogButtonPrefab;
        [SerializeField] private Transform _buttonContainer;

        public override void InstallBindings()
        {
            Container.Bind<RequestQueueManager>().AsSingle();
            
            Container.Bind<DogBreedsModel>().AsSingle();
            
            Container.Bind<DogBreedsView>().FromComponentOn(_dogBreedsScreen).AsSingle();
            
            Container.Bind<PopupView>().FromInstance(_popupView).AsSingle();
            
            Container.Bind<WeatherModel>().AsSingle();
            Container.Bind<WeatherPresenter>().AsSingle().WithArguments(_weatherView).NonLazy();
            
            Container.BindFactory<DogBreedsButton, DogButtonFactory>()
                .FromComponentInNewPrefab(_dogButtonPrefab)
                .UnderTransform(_buttonContainer);
            
            Container.BindInterfacesAndSelfTo<DogBreedsPresenter>().AsSingle().NonLazy();
            
            Container.Bind<SpriteService>().AsSingle();
        }
    }
}