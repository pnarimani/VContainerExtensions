using System;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace VContainer
{
    public class PrefabFactoryBase<TOut> where TOut : class
    {
        [Inject]
        public PrefabFactoryBase()
        {
        }

        [CanBeNull] protected LifetimeScope CurrentScope { get; private set; }

        protected MonoBehaviour Prefab { get; private set; }

        [Inject]
        public void Init([CanBeNull] LifetimeScope current, [NotNull] MonoBehaviour prefab)
        {
            CurrentScope = current;
            Prefab = prefab;
        }

        protected TOut SpawnContext(LifetimeScope context, Transform parent, bool keepWorldPosition)
        {
            LifetimeScope clone = CurrentScope != null
                ? CurrentScope.CreateChildFromPrefab(context)
                : Object.Instantiate(context);
            clone.transform.SetParent(parent, keepWorldPosition);
            clone.Build();
            return GetResultFromSpawnedContext(clone);
        }

        protected TOut SpawnContext(LifetimeScope context, Vector3 position, Quaternion rotation, Transform parent)
        {
            LifetimeScope clone = CurrentScope != null
                ? CurrentScope.CreateChildFromPrefab(context)
                : Object.Instantiate(context);
            Transform t = clone.transform;
            t.SetParent(parent);
            t.position = position;
            t.rotation = rotation;
            clone.Build();
            return GetResultFromSpawnedContext(clone);
        }

        protected TOut Spawn(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (CurrentScope == null)
                return Object.Instantiate(Prefab, position, rotation, parent) as TOut;
            if (Prefab is Component comp)
                return CurrentScope.Container.Instantiate(comp.gameObject, position, rotation, parent)
                    .GetComponent<TOut>();
            return CurrentScope.Container.Instantiate(Prefab, position, rotation, parent) as TOut;
        }

        protected TOut Spawn(Transform parent, bool keepWorldPosition)
        {
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
                // An exception will be thrown if the output is not registered in the LifetimeScope in the container.
                // If the output is component, we can also try to retrieve the result using GetComponent
                if (typeof(Component).IsAssignableFrom(typeof(TOut)))
                    return spawnedContext.GetComponent(typeof(TOut)) as TOut;
                // If the result is not a component, there's nothing we can do. So we rethrow the exception.
                throw;
            }
        }
    }

    public class PrefabFactory<TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public TOut Create(Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            if (Prefab is LifetimeScope context)
                return SpawnContext(context, position, rotation, parent);

            if ((context = Prefab.GetComponent<LifetimeScope>()) != null)
                return SpawnContext(context, position, rotation, parent);

            return Spawn(position, rotation, parent);
        }

        public TOut Create(Transform parent, bool keepWorldPosition)
        {
            if (Prefab is LifetimeScope context)
                return SpawnContext(context, parent, keepWorldPosition);

            if ((context = Prefab.GetComponent<LifetimeScope>()) != null)
                return SpawnContext(context, parent, keepWorldPosition);

            return Spawn(parent, keepWorldPosition);
        }
    }

    public class PrefabFactory<TParam1, TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public TOut Create(
            TParam1 param1,
            Vector3 position = default,
            Quaternion rotation = default,
            Transform parent = default)
        {
            Installer.Instance.Param1 = param1;
            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                if (Prefab is LifetimeScope context)
                    return SpawnContext(context, position, rotation, parent);

                if ((context = Prefab.GetComponent<LifetimeScope>()) != null)
                    return SpawnContext(context, position, rotation, parent);

                return Spawn(position, rotation, parent);
            }
        }

        public TOut Create(TParam1 param1, Transform parent, bool keepWorldPosition)
        {
            Installer.Instance.Param1 = param1;
            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                if (Prefab is LifetimeScope context)
                    return SpawnContext(context, parent, keepWorldPosition);

                if ((context = Prefab.GetComponent<LifetimeScope>()) != null)
                    return SpawnContext(context, parent, keepWorldPosition);

                return Spawn(parent, keepWorldPosition);
            }
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