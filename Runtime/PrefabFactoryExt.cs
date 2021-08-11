using UnityEngine;
using VContainer;

namespace VContainerExtensions.Runtime
{
    public static class PrefabFactoryExt
    {
        public static void RegisterPrefabFactory<TOut, TFactory>(this IContainerBuilder builder, MonoBehaviour prefab)
            where TFactory : PrefabFactoryBase<TOut> where TOut : class
        {
            builder.Register<TFactory>(Lifetime.Scoped)
                .WithParameter("prefab", prefab);
        }
    }
}