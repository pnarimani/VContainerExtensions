using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace VContainer
{
    public abstract class PrefabFactoryBase
    {
        protected static readonly Dictionary<object, Stack<object>> PrefabToPool =
            new Dictionary<object, Stack<object>>();
    }

    public abstract class PrefabFactoryBase<TOut> : PrefabFactoryBase, IDisposable where TOut : class
    {
        /// <summary>
        /// LifetimeScope which is used to instantiate the prefab.
        /// If the prefab we are spawning has LifetimeScope and the factory has a CurrentScope, the prefab will be the child of CurrentScope. 
        /// CurrentScope can be null. In that case, the prefab will be instantiated without a parent LifetimeScope.
        /// </summary>
        [CanBeNull] protected LifetimeScope CurrentScope;

        /// <summary>
        /// The cached component of LifetimeScope on the given prefab. This value can be null if the given prefab doesn't have a LifetimeScope component attached to it.
        /// </summary>
        [CanBeNull] protected LifetimeScope ContextOnPrefab;

        /// <summary>
        /// The prefab which was given through injection. The prefab can be null if null value was provided in <see cref="Init"/> function.
        /// </summary>
        [CanBeNull] protected MonoBehaviour Prefab;

        /// <summary>
        /// If not-null, factory tries to pool the objects.
        /// </summary>
        [CanBeNull] protected PoolSettings PoolSettings;

        protected Transform PoolParentTransform;

        [Inject]
        public PrefabFactoryBase()
        {
        }

        [Inject]
        public void Init(LifetimeScope current, MonoBehaviour prefab, PoolSettings poolSettings)
        {
            PoolSettings = poolSettings;
            CurrentScope = current;
            Prefab = prefab;

            if (Prefab is LifetimeScope context)
                ContextOnPrefab = context;
            else if (Prefab != null && (context = Prefab.GetComponent<LifetimeScope>()) != null)
                ContextOnPrefab = context;
            else
                ContextOnPrefab = null;

            // Adjust PoolParentTransform according to the poolSettings
            if (poolSettings == null && PoolParentTransform != null)
            {
                Object.Destroy(PoolParentTransform.gameObject);
            }
            else if (poolSettings != null)
            {
                if (PoolParentTransform != null && PoolParentTransform.gameObject.name != poolSettings.ParentName)
                {
                    Object.Destroy(PoolParentTransform.gameObject);
                }

                if (PoolParentTransform == null && !string.IsNullOrEmpty(poolSettings.ParentName))
                {
                    PoolParentTransform = new GameObject(poolSettings.ParentName).transform;
                    Object.DontDestroyOnLoad(PoolParentTransform.gameObject);
                }
            }

            if (poolSettings != null)
            {
                for (int i = 0; i < poolSettings.PrewarmCount; i++)
                {
                    SpawnInternal(default, default, PoolParentTransform, false, out Action despawn);
                    despawn();
                }
            }
        }

        protected (LifetimeScope, TOut) Spawn(
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            out Action despawn)
        {
            return SpawnInternal(position, rotation, parent, null, out despawn);
        }

        protected (LifetimeScope, TOut) Spawn(
            Transform parent,
            bool keepWorldPosition,
            out Action despawn)
        {
            return SpawnInternal(default, default, parent, keepWorldPosition, out despawn);
        }

        private (LifetimeScope, TOut) SpawnInternal(
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            bool? keepWorldPosition,
            out Action despawn)
        {
            Stack<object> pool = null;

            if (ContextOnPrefab != null)
            {
                LifetimeScope clone = null;

                if (PoolSettings != null)
                {
                    if (PrefabToPool.TryGetValue(ContextOnPrefab, out pool))
                    {
                        if (pool.Count > 0)
                        {
                            clone = (LifetimeScope) pool.Pop();
                        }
                    }
                    else
                    {
                        pool = new Stack<object>();
                        PrefabToPool.Add(ContextOnPrefab, pool);
                    }
                }

                if (clone == null)
                {
                    clone = CurrentScope != null
                        ? CurrentScope.CreateChildFromPrefab(ContextOnPrefab)
                        : Object.Instantiate(ContextOnPrefab);
                    clone.Build();
                }

                Transform t = clone.transform;
                if (keepWorldPosition != null)
                {
                    t.SetParent(parent, keepWorldPosition.Value);
                }
                else
                {
                    t.SetParent(parent);
                    t.SetPositionAndRotation(position, rotation);
                }

                t.gameObject.SetActive(true);

                TOut output = GetResultFromSpawnedContext(clone);
                despawn = pool != null ? CreateDespawnAction(pool, clone, output) : null;
                return (clone, output);
            }

            if (Prefab != null)
            {
                if (PoolSettings != null)
                {
                    if (PrefabToPool.TryGetValue(Prefab, out pool))
                    {
                        if (pool.Count > 0)
                        {
                            var clone = (MonoBehaviour) pool.Pop();
                            Transform t = clone.transform;
                            if (keepWorldPosition != null)
                            {
                                t.SetParent(parent, keepWorldPosition.Value);
                            }
                            else
                            {
                                t.SetParent(parent);
                                t.SetPositionAndRotation(position, rotation);
                            }

                            clone.gameObject.SetActive(true);
                            despawn = CreateDespawnAction(pool, clone as TOut);
                            return (null, clone as TOut);
                        }
                    }
                    else
                    {
                        pool = new Stack<object>();
                        PrefabToPool.Add(Prefab, pool);
                    }
                }

                GameObject prefabGO = Prefab.gameObject;

                TOut output;

                // If current scope is null, we instantiate the given prefab using unity. 
                // This spawn mode doesn't support injections.
                // But if the current scope is not null, the entire gameObject will be injected.
                // Note that the behaviour of this part is dependent on the VContainer implementation.
                if (CurrentScope == null)
                {
                    output = (keepWorldPosition != null
                            ? Object.Instantiate(prefabGO, parent, keepWorldPosition.Value)
                            : Object.Instantiate(prefabGO, position, rotation, parent))
                        .GetComponent<TOut>();
                }
                else
                {
                    IObjectResolver c = CurrentScope.Container;
                    output = (keepWorldPosition != null
                            ? c.Instantiate(prefabGO, parent, keepWorldPosition.Value)
                            : c.Instantiate(prefabGO, position, rotation, parent)
                        ).GetComponent<TOut>();
                }

                despawn = CreateDespawnAction(pool, output);

                return (null, output);
            }

            throw new InvalidOperationException("Factory is not initialized");
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

        private void CallPoolDespawnMethod(LifetimeScope spawnedScope, TOut output)
        {
            if (PoolSettings == null) return;

            // If the spawned prefab has a scope, we want to find every instance of object which implements IPoolable and call OnPoolDespawn on that. 
            if (spawnedScope != null)
            {
                // It is possible that the output was retrieved by GetComponent and was not registered in the container.
                // We need to make sure that IPoolable functions are called on the output 100% of the time.
                bool outputIsCalled = false;

                // This will resolve a list of all the objects which have been registered and have IPoolable implemented. 
                var poolables = spawnedScope.Container.Resolve<IReadOnlyCollection<IPoolableBase>>();
                foreach (var p in poolables)
                {
                    try
                    {
                        p.OnPoolDespawn();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }

                    if (p.Equals(output))
                    {
                        outputIsCalled = true;
                    }
                }

                if (outputIsCalled)
                    return;
            }

            if (output is IPoolableBase outputPoolable)
            {
                try
                {
                    outputPoolable.OnPoolDespawn();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        private Action CreateDespawnAction(Stack<object> pool, LifetimeScope spawnedScope, TOut output)
        {
            if (pool == null)
                return null;

            return () =>
            {
                if (spawnedScope == null)
                    return;

                CallPoolDespawnMethod(spawnedScope, output);

                // PoolSettings will be null if the factory is disposed.
                if (PoolSettings == null || pool.Count >= PoolSettings.MaxCount)
                {
                    Object.Destroy(spawnedScope.gameObject);
                    return;
                }

                spawnedScope.gameObject.SetActive(false);
                spawnedScope.transform.SetParent(PoolParentTransform);
                pool.Push(spawnedScope);
            };
        }

        private Action CreateDespawnAction(Stack<object> pool, TOut output)
        {
            if (pool == null)
                return null;

            return () =>
            {
                if (output == null)
                    return;

                var comp = output as Component;
                if (comp == null)
                {
                    Debug.LogError("Output is not a component. This is not supported.");
                    return;
                }

                CallPoolDespawnMethod(null, output);

                // PoolSettings will be null if the factory is disposed.
                if (PoolSettings == null || pool.Count >= PoolSettings.MaxCount)
                {
                    Object.Destroy(comp.gameObject);
                    return;
                }

                comp.gameObject.SetActive(false);
                comp.transform.SetParent(PoolParentTransform);
                pool.Push(output);
            };
        }

        void IDisposable.Dispose()
        {
            if (PoolParentTransform != null)
                Object.Destroy(PoolParentTransform.gameObject);
            PoolSettings = null;
        }
    }
}