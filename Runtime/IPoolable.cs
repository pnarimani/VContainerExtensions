
using System;

namespace VContainer
{
    public interface IPoolableBase
    {
        void OnPoolDespawn();
    }
    public interface IPoolable : IPoolableBase
    {
        void OnPoolSpawn(Action despawn);
    }
    public interface IPoolable<in TParam1> : IPoolableBase
    {
        void OnPoolSpawn(Action despawn, TParam1 param1);
    }
    public interface IPoolable<in TParam1, in TParam2> : IPoolableBase
    {
        void OnPoolSpawn(Action despawn, TParam1 param1, TParam2 param2);
    }
    public interface IPoolable<in TParam1, in TParam2, in TParam3> : IPoolableBase
    {
        void OnPoolSpawn(Action despawn, TParam1 param1, TParam2 param2, TParam3 param3);
    }
    public interface IPoolable<in TParam1, in TParam2, in TParam3, in TParam4> : IPoolableBase
    {
        void OnPoolSpawn(Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }
    public interface IPoolable<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5> : IPoolableBase
    {
        void OnPoolSpawn(Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5);
    }
    public interface IPoolable<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6> : IPoolableBase
    {
        void OnPoolSpawn(Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6);
    }
    public interface IPoolable<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7> : IPoolableBase
    {
        void OnPoolSpawn(Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7);
    }
    public interface IPoolable<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, in TParam8> : IPoolableBase
    {
        void OnPoolSpawn(Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8);
    }
    public interface IPoolable<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, in TParam8, in TParam9> : IPoolableBase
    {
        void OnPoolSpawn(Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9);
    }
    public interface IPoolable<in TParam1, in TParam2, in TParam3, in TParam4, in TParam5, in TParam6, in TParam7, in TParam8, in TParam9, in TParam10> : IPoolableBase
    {
        void OnPoolSpawn(Action despawn, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9, TParam10 param10);
    }
}