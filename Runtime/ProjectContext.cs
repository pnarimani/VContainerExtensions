using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace VContainer
{
    public class ProjectContext : LifetimeScope
    {
        [SerializeField] private ScriptableObjectInstaller[] _installers;

        protected override void Configure(IContainerBuilder builder)
        {
            foreach (ScriptableObjectInstaller installer in _installers)
            {
                installer.Install(builder);
            }
        }
    }
}
