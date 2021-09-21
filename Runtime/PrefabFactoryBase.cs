using JetBrains.Annotations;
using UnityEngine;
using VContainer.Unity;

namespace VContainer
{
    public abstract class PrefabFactoryBase<TOut> where TOut : class
    {
        [Inject]
        public PrefabFactoryBase()
        {
        }

        [CanBeNull] private LifetimeScope _currentScope;
        [CanBeNull] private LifetimeScope _contextOnPrefab;
        private MonoBehaviour _prefab;

        [Inject]
        public void Init([CanBeNull] LifetimeScope current, [NotNull] MonoBehaviour prefab)
        {
            _currentScope = current;
            _prefab = prefab;

            if (_prefab is LifetimeScope context)
                _contextOnPrefab = context;
            else if ((context = _prefab.GetComponent<LifetimeScope>()) != null)
                _contextOnPrefab = context;
        }

        protected TOut Spawn(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (_contextOnPrefab != null)
            {
                LifetimeScope clone = _currentScope != null
                    ? _currentScope.CreateChildFromPrefab(_contextOnPrefab)
                    : Object.Instantiate(_contextOnPrefab);

                Transform t = clone.transform;
                t.SetParent(parent);
                t.position = position;
                t.rotation = rotation;
                clone.Build();
                return GetResultFromSpawnedContext(clone);
            }

            if (_currentScope == null)
                return Object.Instantiate(_prefab, position, rotation, parent) as TOut;
            if (_prefab is Component comp)
                return _currentScope.Container.Instantiate(comp.gameObject, position, rotation, parent)
                    .GetComponent<TOut>();
            return _currentScope.Container.Instantiate(_prefab, position, rotation, parent) as TOut;
        }

        protected TOut Spawn(Transform parent, bool keepWorldPosition)
        {
            if (_contextOnPrefab != null)
            {
                LifetimeScope clone = _currentScope != null
                    ? _currentScope.CreateChildFromPrefab(_contextOnPrefab)
                    : Object.Instantiate(_contextOnPrefab);
                clone.transform.SetParent(parent, keepWorldPosition);
                clone.Build();
                return GetResultFromSpawnedContext(clone);
            }

            if (_currentScope == null)
                return Object.Instantiate(_prefab, parent, keepWorldPosition) as TOut;

            if (_prefab is Component comp)
                return _currentScope.Container.Instantiate(comp.gameObject, parent, keepWorldPosition)
                    .GetComponent<TOut>();

            return _currentScope.Container.Instantiate(_prefab, parent, keepWorldPosition) as TOut;
        }

        private static TOut GetResultFromSpawnedContext(LifetimeScope spawnedContext)
        {
            try
            {
                return spawnedContext.Container.Resolve<TOut>();
            }
            catch (VContainerException)
            {
                // An exception will be thrown if the output is not registered in the LifetimeScope in the container.
                // If the output is component, we can also try to retrieve the result using GetComponent
                if (typeof(Component).IsAssignableFrom(typeof(TOut)))
                    return spawnedContext.GetComponent(typeof(TOut)) as TOut;
                // If the result is not a component, there's nothing we can do. So we rethrow the exception.
                throw;
            }
        }
    }
}