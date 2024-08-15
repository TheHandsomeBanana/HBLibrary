using Unity;

namespace HBLibrary.Common.DI.Unity {
    public interface IUnitySetup {
        void Build(IUnityContainer container);
    }
}
