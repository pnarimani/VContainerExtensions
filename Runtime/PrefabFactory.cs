
using UnityEngine;
using VContainer.Unity;

namespace VContainer
{
    public abstract class PrefabFactory< TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public TOut Create(Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {

            {
                return Spawn(position, rotation, parent);
            }
        }

        public TOut Create(Transform parent, bool keepWorldPosition)
        {

            {
                return Spawn(parent, keepWorldPosition);
            }
        }

    }
    public abstract class PrefabFactory<TParam1, TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public TOut Create(TParam1 param1, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            Installer.Instance.Param1 = param1;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(position, rotation, parent);
            }
        }

        public TOut Create(TParam1 param1, Transform parent, bool keepWorldPosition)
        {
            Installer.Instance.Param1 = param1;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
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
                builder.RegisterInstance(Param1).AsImplementedInterfaces().AsSelf();
            }
        }
    }
    public abstract class PrefabFactory<TParam1, TParam2, TOut> : PrefabFactoryBase<TOut> where TOut : class
    {
        public TOut Create(TParam1 param1, TParam2 param2, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(position, rotation, parent);
            }
        }

        public TOut Create(TParam1 param1, TParam2 param2, Transform parent, bool keepWorldPosition)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(parent, keepWorldPosition);
            }
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
        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;
            Installer.Instance.Param3 = param3;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(position, rotation, parent);
            }
        }

        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, Transform parent, bool keepWorldPosition)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;
            Installer.Instance.Param3 = param3;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(parent, keepWorldPosition);
            }
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
        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;
            Installer.Instance.Param3 = param3;
            Installer.Instance.Param4 = param4;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(position, rotation, parent);
            }
        }

        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, Transform parent, bool keepWorldPosition)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;
            Installer.Instance.Param3 = param3;
            Installer.Instance.Param4 = param4;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(parent, keepWorldPosition);
            }
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
        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;
            Installer.Instance.Param3 = param3;
            Installer.Instance.Param4 = param4;
            Installer.Instance.Param5 = param5;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(position, rotation, parent);
            }
        }

        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, Transform parent, bool keepWorldPosition)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;
            Installer.Instance.Param3 = param3;
            Installer.Instance.Param4 = param4;
            Installer.Instance.Param5 = param5;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(parent, keepWorldPosition);
            }
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
        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;
            Installer.Instance.Param3 = param3;
            Installer.Instance.Param4 = param4;
            Installer.Instance.Param5 = param5;
            Installer.Instance.Param6 = param6;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(position, rotation, parent);
            }
        }

        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, Transform parent, bool keepWorldPosition)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;
            Installer.Instance.Param3 = param3;
            Installer.Instance.Param4 = param4;
            Installer.Instance.Param5 = param5;
            Installer.Instance.Param6 = param6;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(parent, keepWorldPosition);
            }
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
        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;
            Installer.Instance.Param3 = param3;
            Installer.Instance.Param4 = param4;
            Installer.Instance.Param5 = param5;
            Installer.Instance.Param6 = param6;
            Installer.Instance.Param7 = param7;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(position, rotation, parent);
            }
        }

        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, Transform parent, bool keepWorldPosition)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;
            Installer.Instance.Param3 = param3;
            Installer.Instance.Param4 = param4;
            Installer.Instance.Param5 = param5;
            Installer.Instance.Param6 = param6;
            Installer.Instance.Param7 = param7;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(parent, keepWorldPosition);
            }
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
        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;
            Installer.Instance.Param3 = param3;
            Installer.Instance.Param4 = param4;
            Installer.Instance.Param5 = param5;
            Installer.Instance.Param6 = param6;
            Installer.Instance.Param7 = param7;
            Installer.Instance.Param8 = param8;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(position, rotation, parent);
            }
        }

        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, Transform parent, bool keepWorldPosition)
        {
            Installer.Instance.Param1 = param1;
            Installer.Instance.Param2 = param2;
            Installer.Instance.Param3 = param3;
            Installer.Instance.Param4 = param4;
            Installer.Instance.Param5 = param5;
            Installer.Instance.Param6 = param6;
            Installer.Instance.Param7 = param7;
            Installer.Instance.Param8 = param8;

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(parent, keepWorldPosition);
            }
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
        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
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

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(position, rotation, parent);
            }
        }

        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, Transform parent, bool keepWorldPosition)
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

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(parent, keepWorldPosition);
            }
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
        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, TParam10 param10, Vector3 position = default, Quaternion rotation = default, Transform parent = default)
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

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(position, rotation, parent);
            }
        }

        public TOut Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, TParam10 param10, Transform parent, bool keepWorldPosition)
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

            using (LifetimeScope.Enqueue(Installer.Instance))
            {
                return Spawn(parent, keepWorldPosition);
            }
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