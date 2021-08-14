using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace VContainer
{
    public class ViewPresenterContext<TView, TPresenter> : LifetimeScope where TView : MonoBehaviour
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterViewPresenter<TView, TPresenter>(builder);
        }

        protected void RegisterViewPresenter<TAdditionalView, TAdditionalPresenter>(IContainerBuilder builder)
            where TAdditionalView : MonoBehaviour
        {
            builder.RegisterEntryPoint<TAdditionalPresenter>().AsSelf();
            builder.RegisterComponent(GetComponent<TAdditionalView>()).AsImplementedInterfaces();
        }
    }
}