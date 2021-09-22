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

        [CanBeNull] protected LifetimeScope CurrentScope;
        [CanBeNull] protected LifetimeScope ContextOnPrefab;
        [CanBeNull] protected MonoBehaviour Prefab;

        [Inject]
        public void Init([CanBeNull] LifetimeScope current, [CanBeNull] MonoBehaviour prefab)
        {
            CurrentScope = current;
            Prefab = prefab;

            if (Prefab is LifetimeScope context)
                ContextOnPrefab = context;
            else if (Prefab != null && (context = Prefab.GetComponent<LifetimeScope>()) != null)
                ContextOnPrefab = context;
            else
                ContextOnPrefab = null;
        }

        protected TOut Spawn(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (ContextOnPrefab != null)
            {
                LifetimeScope clone = CurrentScope != null
                    ? CurrentScope.CreateChildFromPrefab(ContextOnPrefab)
                    : Object.Instantiate(ContextOnPrefab);

                Transform t = clone.transform;
                t.SetParent(parent);
                t.position = position;
                t.rotation = rotation;
                clone.Build();
                return GetResultFromSpawnedContext(clone);
            }

            if (CurrentScope == null)
                return Object.Instantiate(Prefab, position, rotation, parent) as TOut;
            if (Prefab is Component comp)
                return CurrentScope.Container.Instantiate(comp.gameObject, position, rotation, parent)
                    .GetComponent<TOut>();
            return CurrentScope.Container.Instantiate(Prefab, position, rotation, parent) as TOut;
        }

        protected TOut Spawn(Transform parent, bool keepWorldPosition)
        {
            if (ContextOnPrefab != null)
            {
                LifetimeScope clone = CurrentScope != null
                    ? CurrentScope.CreateChildFromPrefab(ContextOnPrefab)
                    : Object.Instantiate(ContextOnPrefab);
                clone.transform.SetParent(parent, keepWorldPosition);
                clone.Build();
                return GetResultFromSpawnedContext(clone);
            }

            if (CurrentScope == null)
                return Object.Instantiate(Prefab, parent, keepWorldPosition) as TOut;

            if (Prefab is Component comp)
                return CurrentScope.Container.Instantiate(comp.gameObject, parent, keepWorldPosition)
                    .GetComponent<TOut>();

            return CurrentScope.Container.Instantiate(Prefab, parent, keepWorldPosition) as TOut;
        }

        private static TOut GetResultFromSpawnedContext(LifetimeScope spawnedContext)
        {
            try
            {
                return spawnedContext.Container.Resolve<TOut>();
            }
            catch (VContainerException)
            {
                // An exception will be thrown if the output is not registered in the LifetimeScope in the spawned container.
                // If the output is component, we can also try to retrieve the result using GetComponent
                if (typeof(Component).IsAssignableFrom(typeof(TOut)))
                {
                    Component component = spawnedContext.GetComponent(typeof(TOut));
                    
                    // If we could not find the component on the GameObject, we need to throw the exception to let the user know there's a problem.
                    if (component == null)
                        throw;
                    
                    // We cannot use c-style cast because compiler doesn't know that component is type of TOut.
                    return component as TOut;
                }

                // If the result is not a component, there's nothing we can do. So we rethrow the exception.
                throw;
            }
        }
    }
}