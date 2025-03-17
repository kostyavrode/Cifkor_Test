using DefaultNamespace.Services;
using DefaultNamespace.UI.DogBreeds;
using UI.DogBreeds;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.DI
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private GameObject dogBreedsScreen;
        [SerializeField] private DogBreedsButton dogButtonPrefab;
        [SerializeField] private Transform buttonContainer;

        public override void InstallBindings()
        {
            // Менеджер очереди запросов
            Container.Bind<RequestQueueManager>().AsSingle();

            // Сервис данных о породах
            Container.Bind<DogBreedsService>().AsSingle();
        
            // Модель данных о породах
            Container.Bind<DogBreedsModel>().AsSingle();

            // Представление (UI) - получаем компонент с объекта сцены
            Container.Bind<DogBreedsView>().FromComponentOn(dogBreedsScreen).AsSingle();

            // Фабрика кнопок с Zenject
            Container.BindFactory<DogBreedsButton, DogButtonFactory>()
                .FromComponentInNewPrefab(dogButtonPrefab)
                .UnderTransform(buttonContainer);

            // Презентер - создается автоматически при старте сцены
            Container.BindInterfacesAndSelfTo<DogBreedsPresenter>().AsSingle().NonLazy();
        }
    }
}