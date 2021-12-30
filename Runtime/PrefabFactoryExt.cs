using UnityEngine;

namespace VContainer
{
    public static class PrefabFactoryExt
    {
        public static RegistrationBuilder RegisterPrefabFactory<TOut, TFactory>(this IContainerBuilder builder, MonoBehaviour prefab, PoolSettings poolSettings = null)
            where TFactory : PrefabFactoryBase<TOut> where TOut : class
        {
            return builder.Register<TFactory>(Lifetime.Singleton)
                .WithParameter("prefab", prefab)
                .WithParameter(poolSettings);
        }
    }
}