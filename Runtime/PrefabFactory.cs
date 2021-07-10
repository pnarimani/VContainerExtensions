using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace VContainerExtensions.Runtime
{
    public class PrefabFactoryBase
    {
    }

    public class PrefabFactory<TOut> : PrefabFactoryBase where TOut : MonoBehaviour
    {
        private LifetimeScope _current;
        private TOut _prefab;

        [Inject]
        public void Init(LifetimeScope current, TOut prefab)
        {
            _current = current;
            _prefab = prefab;
        }

        public TOut Create(Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            return _current.Container.Instantiate(_prefab, position, rotation, parent);
        }
        
        public TOut Create(Transform parent, bool keepWorldPosition)
        {
            return _current.Container.Instantiate(_prefab, parent, keepWorldPosition);
        }
    }

    public class PrefabFactory<TParam1, TOut> : PrefabFactoryBase where TOut : MonoBehaviour
    {
        private LifetimeScope _current;
        private TOut _prefab;

        [Inject]
        public void Init(LifetimeScope current, TOut prefab)
        {
            _current = current;
            _prefab = prefab;
        }

        public TOut Create(TParam1 param1, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            Installer.Instance.Param1 = param1;
            using (LifetimeScope.Enqueue(Installer.Instance))
                return _current.Container.Instantiate(_prefab, position, rotation, parent);
        }
        
        public TOut Create(TParam1 param1, Transform parent, bool keepWorldPosition)
        {
            Installer.Instance.Param1 = param1;
            using (LifetimeScope.Enqueue(Installer.Instance))
                return _current.Container.Instantiate(_prefab, parent, keepWorldPosition);
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