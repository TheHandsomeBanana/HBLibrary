using HBLibrary.Interface.DI;
using Unity;
using Unity.Lifetime;

namespace HBLibrary.DI {
    public static class UnityBase {
        public static IUnityContainer MainContainer { get; } = new UnityContainer();
        public static UnityContainerRegistry Registry { get; } = new UnityContainerRegistry(MainContainer);

        public static void Boot(params IUnitySetup[] setups) {
            foreach (IUnitySetup setup in setups)
                setup.Build(MainContainer);
        }

        public static void Boot(IUnityContainer container, params IUnitySetup[] setups) {
            foreach (IUnitySetup setup in setups)
                setup.Build(container);
        }

        /// <summary>
        /// Gets a child container registered in the parent container
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IUnityContainer GetChildContainer(string name) {
            return MainContainer.Resolve<IUnityContainer>(name);
        }

        /// <summary>
        /// Creates a child container and registers it in the parent UnityContainer
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IUnityContainer CreateChildContainer(string name) {
            IUnityContainer childContainer = MainContainer.CreateChildContainer();
            MainContainer.RegisterInstance(name, childContainer, new ContainerControlledLifetimeManager());
            return childContainer;
        }
    }
}
