using HBLibrary.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace HBLibrary.DI;
public class UnityContainerRegistry : IDisposable {
    private readonly Dictionary<Guid, IUnityContainer> containerRegistry = [];
    public IUnityContainer BaseContainer { get; }
    public UnityContainerRegistry(IUnityContainer baseContainer) {
        BaseContainer = baseContainer;
    }

    public IUnityContainer this[Guid guid] {
        get { return containerRegistry[guid]; }
        set { containerRegistry[guid] = value; }
    }

    public IUnityContainer this[string name] {
        get { return containerRegistry[name.ToGuid()]; }
        set { containerRegistry[name.ToGuid()] = value; }
    }

    public IUnityContainer Get(Guid guid) {
        return this[guid];
    }

    public IUnityContainer Get(string name) {
        return this[name];
    }

    public bool TryGet(Guid guid, [NotNullWhen(true)] out IUnityContainer? container) {
        return containerRegistry.TryGetValue(guid, out container);
    }

    public bool TryGet(string name, [NotNullWhen(true)] out IUnityContainer? container) {
        return TryGet(name.ToGuid(), out container);
    }

    public IUnityContainer RegisterChildContainer(Guid containerId) {
        IUnityContainer childContainer = BaseContainer.CreateChildContainer();
        containerRegistry.Add(containerId, childContainer);
        return childContainer;
    }

    public IUnityContainer RegisterChildContainer(string name) {
        return RegisterChildContainer(name.ToGuid());
    }

    public bool DisposeChildContainer(Guid containerId) {
        IUnityContainer container = containerRegistry[containerId];
        container.Dispose();

        return containerRegistry.Remove(containerId);
    }

    public bool DisposeChildContainer(string name) {
        return DisposeChildContainer(name.ToGuid());
    }

    public void Dispose() {
        foreach (IUnityContainer container in containerRegistry.Values) {
            container.Dispose();
        }

        BaseContainer.Dispose();
    }
}
