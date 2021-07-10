using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Oboto
{
    public abstract class ScriptableObjectInstaller : ScriptableObject, IInstaller
    {
        public abstract void Install(IContainerBuilder builder);
    }
}