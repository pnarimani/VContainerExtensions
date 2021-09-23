
using System;
using UnityEngine;
using VContainer.Unity;
using System.Collections.Generic;

namespace VContainer
{
    public abstract class PrefabFactory< TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public virtual TOut Create(Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
                (LifetimeScope scope, TOut output) = Spawn(position, rotation, parent, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn);
                return output;
        }

        public virtual TOut Create(Transform parent, bool keepWorldPosition)
        {
                (LifetimeScope scope, TOut output) = Spawn(parent, keepWorldPosition, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn);
                return output;
        }

        private void CallPoolSpawnMethod(LifetimeScope spawnedScope, TOut output, Action despawn)
        {
            if (PoolSettings == null) return;

            // If the spawned prefab has a scope, we want to find every instance of object which implements IPoolable and call OnPoolSpawned on that. 
            if (spawnedScope != null)
            {
                // It is possible that the output was retrieved by GetComponent and was not registered in the container.
                // We need to make sure that IPoolable functions are called on the output 100% of the time.
                bool outputIsCalled = false;

                // This will resolve a list of all the objects which have been registered and have IPoolable implemented. 
                var poolables = spawnedScope.Container.Resolve<IReadOnlyCollection<IPoolable>>();
                foreach (var p in poolables)
                {
                    try
                    {
                        p.OnPoolSpawn(despawn);
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

            if (output is IPoolable outputPoolable)
            {
                try
                {
                    outputPoolable.OnPoolSpawn(despawn);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

    }
    public abstract class PrefabFactory<TParam1, TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public virtual TOut Create(TParam1 param1, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(position, rotation, parent, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        public virtual TOut Create(TParam1 param1, Transform parent, bool keepWorldPosition)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(parent, keepWorldPosition, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        private void CallPoolSpawnMethod(LifetimeScope spawnedScope, TOut output, Action despawn, TParam1 param1)
        {
            if (PoolSettings == null) return;

            // If the spawned prefab has a scope, we want to find every instance of object which implements IPoolable and call OnPoolSpawned on that. 
            if (spawnedScope != null)
            {
                // It is possible that the output was retrieved by GetComponent and was not registered in the container.
                // We need to make sure that IPoolable functions are called on the output 100% of the time.
                bool outputIsCalled = false;

                // This will resolve a list of all the objects which have been registered and have IPoolable implemented. 
                var poolables = spawnedScope.Container.Resolve<IReadOnlyCollection<IPoolable<TParam1>>>();
                foreach (var p in poolables)
                {
                    try
                    {
                        p.OnPoolSpawn(despawn, param1);
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

            if (output is IPoolable<TParam1> outputPoolable)
            {
                try
                {
                    outputPoolable.OnPoolSpawn(despawn, param1);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }


        private IDisposable RegisterParameters(TParam1 param1)
        {
            // We don't want to include extra parameters when object is pooled because these parameters will be passed using IPoolable functions.
            if (PoolSettings == null)
            {
                Installer.Instance.Param1 = param1;
                return LifetimeScope.Enqueue(Installer.Instance);
            }

            return null;
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
                builder.RegisterInstance(Param1).AsImplementedInterfaces().AsSelf();
            }
        }
    }
    public abstract class PrefabFactory<TParam1, TParam2, TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public virtual TOut Create(TParam1 param1, TParam2 param2, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(position, rotation, parent, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        public virtual TOut Create(TParam1 param1, TParam2 param2, Transform parent, bool keepWorldPosition)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(parent, keepWorldPosition, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        private void CallPoolSpawnMethod(LifetimeScope spawnedScope, TOut output, Action despawn, TParam1 param1, TParam2 param2)
        {
            if (PoolSettings == null) return;

            // If the spawned prefab has a scope, we want to find every instance of object which implements IPoolable and call OnPoolSpawned on that. 
            if (spawnedScope != null)
            {
                // It is possible that the output was retrieved by GetComponent and was not registered in the container.
                // We need to make sure that IPoolable functions are called on the output 100% of the time.
                bool outputIsCalled = false;

                // This will resolve a list of all the objects which have been registered and have IPoolable implemented. 
                var poolables = spawnedScope.Container.Resolve<IReadOnlyCollection<IPoolable<TParam1, TParam2>>>();
                foreach (var p in poolables)
                {
                    try
                    {
                        p.OnPoolSpawn(despawn, param1, param2);
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

            if (output is IPoolable<TParam1, TParam2> outputPoolable)
            {
                try
                {
                    outputPoolable.OnPoolSpawn(despawn, param1, param2);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }


        private IDisposable RegisterParameters(TParam1 param1, TParam2 param2)
        {
            // We don't want to include extra parameters when object is pooled because these parameters will be passed using IPoolable functions.
            if (PoolSettings == null)
            {
                Installer.Instance.Param1 = param1;
                Installer.Instance.Param2 = param2;
                return LifetimeScope.Enqueue(Installer.Instance);
            }

            return null;
        }

        private class Installer : IInstaller
        {
            public static readonly Installer Instance = new Installer();
            public TParam1 Param1;
            public TParam2 Param2;

            private Installer()
            {
            }

            public void Install(IContainerBuilder builder)
            {
                builder.RegisterInstance(Param1).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param2).AsImplementedInterfaces().AsSelf();
            }
        }
    }
    public abstract class PrefabFactory<TParam1, TParam2, TParam3, TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(position, rotation, parent, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, Transform parent, bool keepWorldPosition)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(parent, keepWorldPosition, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        private void CallPoolSpawnMethod(LifetimeScope spawnedScope, TOut output, Action despawn, TParam1 param1, TParam2 param2, TParam3 param3)
        {
            if (PoolSettings == null) return;

            // If the spawned prefab has a scope, we want to find every instance of object which implements IPoolable and call OnPoolSpawned on that. 
            if (spawnedScope != null)
            {
                // It is possible that the output was retrieved by GetComponent and was not registered in the container.
                // We need to make sure that IPoolable functions are called on the output 100% of the time.
                bool outputIsCalled = false;

                // This will resolve a list of all the objects which have been registered and have IPoolable implemented. 
                var poolables = spawnedScope.Container.Resolve<IReadOnlyCollection<IPoolable<TParam1, TParam2, TParam3>>>();
                foreach (var p in poolables)
                {
                    try
                    {
                        p.OnPoolSpawn(despawn, param1, param2, param3);
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

            if (output is IPoolable<TParam1, TParam2, TParam3> outputPoolable)
            {
                try
                {
                    outputPoolable.OnPoolSpawn(despawn, param1, param2, param3);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }


        private IDisposable RegisterParameters(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            // We don't want to include extra parameters when object is pooled because these parameters will be passed using IPoolable functions.
            if (PoolSettings == null)
            {
                Installer.Instance.Param1 = param1;
                Installer.Instance.Param2 = param2;
                Installer.Instance.Param3 = param3;
                return LifetimeScope.Enqueue(Installer.Instance);
            }

            return null;
        }

        private class Installer : IInstaller
        {
            public static readonly Installer Instance = new Installer();
            public TParam1 Param1;
            public TParam2 Param2;
            public TParam3 Param3;

            private Installer()
            {
            }

            public void Install(IContainerBuilder builder)
            {
                builder.RegisterInstance(Param1).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param2).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param3).AsImplementedInterfaces().AsSelf();
            }
        }
    }
    public abstract class PrefabFactory<TParam1, TParam2, TParam3, TParam4, TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(position, rotation, parent, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, Transform parent, bool keepWorldPosition)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(parent, keepWorldPosition, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        private void CallPoolSpawnMethod(LifetimeScope spawnedScope, TOut output, Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            if (PoolSettings == null) return;

            // If the spawned prefab has a scope, we want to find every instance of object which implements IPoolable and call OnPoolSpawned on that. 
            if (spawnedScope != null)
            {
                // It is possible that the output was retrieved by GetComponent and was not registered in the container.
                // We need to make sure that IPoolable functions are called on the output 100% of the time.
                bool outputIsCalled = false;

                // This will resolve a list of all the objects which have been registered and have IPoolable implemented. 
                var poolables = spawnedScope.Container.Resolve<IReadOnlyCollection<IPoolable<TParam1, TParam2, TParam3, TParam4>>>();
                foreach (var p in poolables)
                {
                    try
                    {
                        p.OnPoolSpawn(despawn, param1, param2, param3, param4);
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

            if (output is IPoolable<TParam1, TParam2, TParam3, TParam4> outputPoolable)
            {
                try
                {
                    outputPoolable.OnPoolSpawn(despawn, param1, param2, param3, param4);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }


        private IDisposable RegisterParameters(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            // We don't want to include extra parameters when object is pooled because these parameters will be passed using IPoolable functions.
            if (PoolSettings == null)
            {
                Installer.Instance.Param1 = param1;
                Installer.Instance.Param2 = param2;
                Installer.Instance.Param3 = param3;
                Installer.Instance.Param4 = param4;
                return LifetimeScope.Enqueue(Installer.Instance);
            }

            return null;
        }

        private class Installer : IInstaller
        {
            public static readonly Installer Instance = new Installer();
            public TParam1 Param1;
            public TParam2 Param2;
            public TParam3 Param3;
            public TParam4 Param4;

            private Installer()
            {
            }

            public void Install(IContainerBuilder builder)
            {
                builder.RegisterInstance(Param1).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param2).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param3).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param4).AsImplementedInterfaces().AsSelf();
            }
        }
    }
    public abstract class PrefabFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4, param5);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(position, rotation, parent, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4, param5);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, Transform parent, bool keepWorldPosition)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4, param5);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(parent, keepWorldPosition, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4, param5);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        private void CallPoolSpawnMethod(LifetimeScope spawnedScope, TOut output, Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            if (PoolSettings == null) return;

            // If the spawned prefab has a scope, we want to find every instance of object which implements IPoolable and call OnPoolSpawned on that. 
            if (spawnedScope != null)
            {
                // It is possible that the output was retrieved by GetComponent and was not registered in the container.
                // We need to make sure that IPoolable functions are called on the output 100% of the time.
                bool outputIsCalled = false;

                // This will resolve a list of all the objects which have been registered and have IPoolable implemented. 
                var poolables = spawnedScope.Container.Resolve<IReadOnlyCollection<IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5>>>();
                foreach (var p in poolables)
                {
                    try
                    {
                        p.OnPoolSpawn(despawn, param1, param2, param3, param4, param5);
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

            if (output is IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5> outputPoolable)
            {
                try
                {
                    outputPoolable.OnPoolSpawn(despawn, param1, param2, param3, param4, param5);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }


        private IDisposable RegisterParameters(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
        {
            // We don't want to include extra parameters when object is pooled because these parameters will be passed using IPoolable functions.
            if (PoolSettings == null)
            {
                Installer.Instance.Param1 = param1;
                Installer.Instance.Param2 = param2;
                Installer.Instance.Param3 = param3;
                Installer.Instance.Param4 = param4;
                Installer.Instance.Param5 = param5;
                return LifetimeScope.Enqueue(Installer.Instance);
            }

            return null;
        }

        private class Installer : IInstaller
        {
            public static readonly Installer Instance = new Installer();
            public TParam1 Param1;
            public TParam2 Param2;
            public TParam3 Param3;
            public TParam4 Param4;
            public TParam5 Param5;

            private Installer()
            {
            }

            public void Install(IContainerBuilder builder)
            {
                builder.RegisterInstance(Param1).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param2).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param3).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param4).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param5).AsImplementedInterfaces().AsSelf();
            }
        }
    }
    public abstract class PrefabFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4, param5, param6);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(position, rotation, parent, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4, param5, param6);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, Transform parent, bool keepWorldPosition)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4, param5, param6);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(parent, keepWorldPosition, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4, param5, param6);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        private void CallPoolSpawnMethod(LifetimeScope spawnedScope, TOut output, Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            if (PoolSettings == null) return;

            // If the spawned prefab has a scope, we want to find every instance of object which implements IPoolable and call OnPoolSpawned on that. 
            if (spawnedScope != null)
            {
                // It is possible that the output was retrieved by GetComponent and was not registered in the container.
                // We need to make sure that IPoolable functions are called on the output 100% of the time.
                bool outputIsCalled = false;

                // This will resolve a list of all the objects which have been registered and have IPoolable implemented. 
                var poolables = spawnedScope.Container.Resolve<IReadOnlyCollection<IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>>>();
                foreach (var p in poolables)
                {
                    try
                    {
                        p.OnPoolSpawn(despawn, param1, param2, param3, param4, param5, param6);
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

            if (output is IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> outputPoolable)
            {
                try
                {
                    outputPoolable.OnPoolSpawn(despawn, param1, param2, param3, param4, param5, param6);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }


        private IDisposable RegisterParameters(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
        {
            // We don't want to include extra parameters when object is pooled because these parameters will be passed using IPoolable functions.
            if (PoolSettings == null)
            {
                Installer.Instance.Param1 = param1;
                Installer.Instance.Param2 = param2;
                Installer.Instance.Param3 = param3;
                Installer.Instance.Param4 = param4;
                Installer.Instance.Param5 = param5;
                Installer.Instance.Param6 = param6;
                return LifetimeScope.Enqueue(Installer.Instance);
            }

            return null;
        }

        private class Installer : IInstaller
        {
            public static readonly Installer Instance = new Installer();
            public TParam1 Param1;
            public TParam2 Param2;
            public TParam3 Param3;
            public TParam4 Param4;
            public TParam5 Param5;
            public TParam6 Param6;

            private Installer()
            {
            }

            public void Install(IContainerBuilder builder)
            {
                builder.RegisterInstance(Param1).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param2).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param3).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param4).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param5).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param6).AsImplementedInterfaces().AsSelf();
            }
        }
    }
    public abstract class PrefabFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4, param5, param6, param7);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(position, rotation, parent, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4, param5, param6, param7);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, Transform parent, bool keepWorldPosition)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4, param5, param6, param7);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(parent, keepWorldPosition, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4, param5, param6, param7);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        private void CallPoolSpawnMethod(LifetimeScope spawnedScope, TOut output, Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7)
        {
            if (PoolSettings == null) return;

            // If the spawned prefab has a scope, we want to find every instance of object which implements IPoolable and call OnPoolSpawned on that. 
            if (spawnedScope != null)
            {
                // It is possible that the output was retrieved by GetComponent and was not registered in the container.
                // We need to make sure that IPoolable functions are called on the output 100% of the time.
                bool outputIsCalled = false;

                // This will resolve a list of all the objects which have been registered and have IPoolable implemented. 
                var poolables = spawnedScope.Container.Resolve<IReadOnlyCollection<IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>>>();
                foreach (var p in poolables)
                {
                    try
                    {
                        p.OnPoolSpawn(despawn, param1, param2, param3, param4, param5, param6, param7);
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

            if (output is IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> outputPoolable)
            {
                try
                {
                    outputPoolable.OnPoolSpawn(despawn, param1, param2, param3, param4, param5, param6, param7);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }


        private IDisposable RegisterParameters(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7)
        {
            // We don't want to include extra parameters when object is pooled because these parameters will be passed using IPoolable functions.
            if (PoolSettings == null)
            {
                Installer.Instance.Param1 = param1;
                Installer.Instance.Param2 = param2;
                Installer.Instance.Param3 = param3;
                Installer.Instance.Param4 = param4;
                Installer.Instance.Param5 = param5;
                Installer.Instance.Param6 = param6;
                Installer.Instance.Param7 = param7;
                return LifetimeScope.Enqueue(Installer.Instance);
            }

            return null;
        }

        private class Installer : IInstaller
        {
            public static readonly Installer Instance = new Installer();
            public TParam1 Param1;
            public TParam2 Param2;
            public TParam3 Param3;
            public TParam4 Param4;
            public TParam5 Param5;
            public TParam6 Param6;
            public TParam7 Param7;

            private Installer()
            {
            }

            public void Install(IContainerBuilder builder)
            {
                builder.RegisterInstance(Param1).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param2).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param3).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param4).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param5).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param6).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param7).AsImplementedInterfaces().AsSelf();
            }
        }
    }
    public abstract class PrefabFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4, param5, param6, param7, param8);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(position, rotation, parent, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4, param5, param6, param7, param8);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, Transform parent, bool keepWorldPosition)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4, param5, param6, param7, param8);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(parent, keepWorldPosition, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4, param5, param6, param7, param8);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        private void CallPoolSpawnMethod(LifetimeScope spawnedScope, TOut output, Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8)
        {
            if (PoolSettings == null) return;

            // If the spawned prefab has a scope, we want to find every instance of object which implements IPoolable and call OnPoolSpawned on that. 
            if (spawnedScope != null)
            {
                // It is possible that the output was retrieved by GetComponent and was not registered in the container.
                // We need to make sure that IPoolable functions are called on the output 100% of the time.
                bool outputIsCalled = false;

                // This will resolve a list of all the objects which have been registered and have IPoolable implemented. 
                var poolables = spawnedScope.Container.Resolve<IReadOnlyCollection<IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8>>>();
                foreach (var p in poolables)
                {
                    try
                    {
                        p.OnPoolSpawn(despawn, param1, param2, param3, param4, param5, param6, param7, param8);
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

            if (output is IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8> outputPoolable)
            {
                try
                {
                    outputPoolable.OnPoolSpawn(despawn, param1, param2, param3, param4, param5, param6, param7, param8);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }


        private IDisposable RegisterParameters(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8)
        {
            // We don't want to include extra parameters when object is pooled because these parameters will be passed using IPoolable functions.
            if (PoolSettings == null)
            {
                Installer.Instance.Param1 = param1;
                Installer.Instance.Param2 = param2;
                Installer.Instance.Param3 = param3;
                Installer.Instance.Param4 = param4;
                Installer.Instance.Param5 = param5;
                Installer.Instance.Param6 = param6;
                Installer.Instance.Param7 = param7;
                Installer.Instance.Param8 = param8;
                return LifetimeScope.Enqueue(Installer.Instance);
            }

            return null;
        }

        private class Installer : IInstaller
        {
            public static readonly Installer Instance = new Installer();
            public TParam1 Param1;
            public TParam2 Param2;
            public TParam3 Param3;
            public TParam4 Param4;
            public TParam5 Param5;
            public TParam6 Param6;
            public TParam7 Param7;
            public TParam8 Param8;

            private Installer()
            {
            }

            public void Install(IContainerBuilder builder)
            {
                builder.RegisterInstance(Param1).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param2).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param3).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param4).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param5).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param6).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param7).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param8).AsImplementedInterfaces().AsSelf();
            }
        }
    }
    public abstract class PrefabFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4, param5, param6, param7, param8, param9);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(position, rotation, parent, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4, param5, param6, param7, param8, param9);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, Transform parent, bool keepWorldPosition)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4, param5, param6, param7, param8, param9);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(parent, keepWorldPosition, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4, param5, param6, param7, param8, param9);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        private void CallPoolSpawnMethod(LifetimeScope spawnedScope, TOut output, Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9)
        {
            if (PoolSettings == null) return;

            // If the spawned prefab has a scope, we want to find every instance of object which implements IPoolable and call OnPoolSpawned on that. 
            if (spawnedScope != null)
            {
                // It is possible that the output was retrieved by GetComponent and was not registered in the container.
                // We need to make sure that IPoolable functions are called on the output 100% of the time.
                bool outputIsCalled = false;

                // This will resolve a list of all the objects which have been registered and have IPoolable implemented. 
                var poolables = spawnedScope.Container.Resolve<IReadOnlyCollection<IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9>>>();
                foreach (var p in poolables)
                {
                    try
                    {
                        p.OnPoolSpawn(despawn, param1, param2, param3, param4, param5, param6, param7, param8, param9);
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

            if (output is IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9> outputPoolable)
            {
                try
                {
                    outputPoolable.OnPoolSpawn(despawn, param1, param2, param3, param4, param5, param6, param7, param8, param9);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }


        private IDisposable RegisterParameters(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9)
        {
            // We don't want to include extra parameters when object is pooled because these parameters will be passed using IPoolable functions.
            if (PoolSettings == null)
            {
                Installer.Instance.Param1 = param1;
                Installer.Instance.Param2 = param2;
                Installer.Instance.Param3 = param3;
                Installer.Instance.Param4 = param4;
                Installer.Instance.Param5 = param5;
                Installer.Instance.Param6 = param6;
                Installer.Instance.Param7 = param7;
                Installer.Instance.Param8 = param8;
                Installer.Instance.Param9 = param9;
                return LifetimeScope.Enqueue(Installer.Instance);
            }

            return null;
        }

        private class Installer : IInstaller
        {
            public static readonly Installer Instance = new Installer();
            public TParam1 Param1;
            public TParam2 Param2;
            public TParam3 Param3;
            public TParam4 Param4;
            public TParam5 Param5;
            public TParam6 Param6;
            public TParam7 Param7;
            public TParam8 Param8;
            public TParam9 Param9;

            private Installer()
            {
            }

            public void Install(IContainerBuilder builder)
            {
                builder.RegisterInstance(Param1).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param2).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param3).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param4).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param5).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param6).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param7).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param8).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param9).AsImplementedInterfaces().AsSelf();
            }
        }
    }
    public abstract class PrefabFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, TParam10 param10, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(position, rotation, parent, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        public virtual TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, TParam10 param10, Transform parent, bool keepWorldPosition)
        {
            IDisposable extraInstallerHandle = RegisterParameters(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
            try
            {
                (LifetimeScope scope, TOut output) = Spawn(parent, keepWorldPosition, out var despawn);
                CallPoolSpawnMethod(scope, output, despawn, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
                return output;
            }
            finally
            {
                extraInstallerHandle?.Dispose();
            }
        }

        private void CallPoolSpawnMethod(LifetimeScope spawnedScope, TOut output, Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, TParam10 param10)
        {
            if (PoolSettings == null) return;

            // If the spawned prefab has a scope, we want to find every instance of object which implements IPoolable and call OnPoolSpawned on that. 
            if (spawnedScope != null)
            {
                // It is possible that the output was retrieved by GetComponent and was not registered in the container.
                // We need to make sure that IPoolable functions are called on the output 100% of the time.
                bool outputIsCalled = false;

                // This will resolve a list of all the objects which have been registered and have IPoolable implemented. 
                var poolables = spawnedScope.Container.Resolve<IReadOnlyCollection<IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10>>>();
                foreach (var p in poolables)
                {
                    try
                    {
                        p.OnPoolSpawn(despawn, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
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

            if (output is IPoolable<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> outputPoolable)
            {
                try
                {
                    outputPoolable.OnPoolSpawn(despawn, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }


        private IDisposable RegisterParameters(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, TParam10 param10)
        {
            // We don't want to include extra parameters when object is pooled because these parameters will be passed using IPoolable functions.
            if (PoolSettings == null)
            {
                Installer.Instance.Param1 = param1;
                Installer.Instance.Param2 = param2;
                Installer.Instance.Param3 = param3;
                Installer.Instance.Param4 = param4;
                Installer.Instance.Param5 = param5;
                Installer.Instance.Param6 = param6;
                Installer.Instance.Param7 = param7;
                Installer.Instance.Param8 = param8;
                Installer.Instance.Param9 = param9;
                Installer.Instance.Param10 = param10;
                return LifetimeScope.Enqueue(Installer.Instance);
            }

            return null;
        }

        private class Installer : IInstaller
        {
            public static readonly Installer Instance = new Installer();
            public TParam1 Param1;
            public TParam2 Param2;
            public TParam3 Param3;
            public TParam4 Param4;
            public TParam5 Param5;
            public TParam6 Param6;
            public TParam7 Param7;
            public TParam8 Param8;
            public TParam9 Param9;
            public TParam10 Param10;

            private Installer()
            {
            }

            public void Install(IContainerBuilder builder)
            {
                builder.RegisterInstance(Param1).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param2).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param3).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param4).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param5).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param6).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param7).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param8).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param9).AsImplementedInterfaces().AsSelf();
                builder.RegisterInstance(Param10).AsImplementedInterfaces().AsSelf();
            }
        }
    }
}