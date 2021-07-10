using VContainer;
using VContainer.Unity;

namespace VContainerExtensions.Runtime
{
    /// <summary>
    /// Used as a non-generic marker for ContextPrefabFactory
    /// </summary>
    public class ContextPrefabFactoryBase
    {
    }
    
    // TODO: Add more generic variants using t4 templates

    public class ContextPrefabFactory<TOut> : ContextPrefabFactoryBase
    {
        private LifetimeScope _current;
        private LifetimeScope _prefab;

        [Inject]
        public void Init(LifetimeScope current, LifetimeScope prefab)
        {
            _current = current;
            _prefab = prefab;
        }

        public TOut Create()
        {
            LifetimeScope clone = _current.CreateChildFromPrefab(_prefab);
            clone.Build();
            return clone.Container.Resolve<TOut>();
        }
    }

    public class ContextPrefabFactory<TParam1, TOut> : ContextPrefabFactoryBase
    {
        private LifetimeScope _current;
        private LifetimeScope _prefab;

        [Inject]
        public void Init(LifetimeScope current, LifetimeScope prefab)
        {
            _current = current;
            _prefab = prefab;
        }

        public TOut Create(TParam1 param1)
        {
            Installer.Instance.Param1 = param1;
            LifetimeScope clone = _current.CreateChildFromPrefab(_prefab, Installer.Instance);
            clone.Build();
            return clone.Container.Resolve<TOut>();
        }

        private class Installer : IInstaller
        {
            public static readonly Installer Instance = new Installer();
            public TParam1 Param1;

            private Installer()
            {
            }

            public void Install(IContainerBuilder builder)
            {
                builder.RegisterInstance(Param1);
            }
        }
    }

}