using DefaultNamespace.Services;
using DefaultNamespace.UI.DogBreeds;
using UI.DogBreeds;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.DI
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private WeatherView _weatherView;
        [SerializeField] private GameObject dogBreedsScreen;
        [SerializeField] private DogBreedsButton dogButtonPrefab;
        [SerializeField] private Transform buttonContainer;

        public override void InstallBindings()
        {
            Container.Bind<RequestQueueManager>().AsSingle();
            
            Container.Bind<DogBreedsModel>().AsSingle();
            
            Container.Bind<DogBreedsView>().FromComponentOn(dogBreedsScreen).AsSingle();
            
            Container.Bind<WeatherModel>().AsSingle();
            Container.Bind<WeatherPresenter>().AsSingle().WithArguments(_weatherView).NonLazy();
            
            Container.BindFactory<DogBreedsButton, DogButtonFactory>()
                .FromComponentInNewPrefab(dogButtonPrefab)
                .UnderTransform(buttonContainer);
            
            Container.BindInterfacesAndSelfTo<DogBreedsPresenter>().AsSingle().NonLazy();
        }
    }
}