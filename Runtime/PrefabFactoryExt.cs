using UnityEngine;

namespace VContainer
{
    public static class PrefabFactoryExt
    {
        public static RegistrationBuilder RegisterPrefabFactory<TOut, TFactory>(this IContainerBuilder builder, MonoBehaviour prefab)
            where TFactory : PrefabFactoryBase<TOut> where TOut : class
        {
            return builder.Register<TFactory>(Lifetime.Scoped).WithParameter("prefab", prefab);;
        }
    }
}