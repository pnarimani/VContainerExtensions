using VContainer;
using VContainer.Unity;

namespace VContainerExtensions.Runtime
{
    public static class PrefabFactoryExt
    {
        public static void RegisterPrefabFactory<TFactory>(this IContainerBuilder builder, LifetimeScope prefab) where TFactory : ContextPrefabFactoryBase
        {
            builder.Register<TFactory>(Lifetime.Scoped)
                .WithParameter("prefab", prefab);
        }

        public static void RegisterPrefabFactory<TFactory, TOut>(this IContainerBuilder builder, TOut prefab) where TFactory : PrefabFactoryBase
        {
            builder.Register<TFactory>(Lifetime.Scoped)
                .WithParameter("prefab", prefab);
        }
    }
}