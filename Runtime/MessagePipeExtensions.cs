#if MESSAGE_PIPE
using System;
using MessagePipe;
using VContainer.Unity;

namespace VContainer
{
    public static class MessagePipeExtensions
    {
        public static MessagePipeOptions RegisterSingleMessagePipe(this IContainerBuilder builder,
            Action<MessagePipeOptions> configure = null, bool setAsGlobalProvider = true)
        {
            var scope = (LifetimeScope) builder.ApplicationOrigin;
            if (scope.Parent != null)
            {
                scope.Parent.Build();
                return scope.Parent.Container.Resolve<MessagePipeOptions>();
            }

            // If another installer on the same LifetimeScope has installed MessagePipe, we cannot register message pipe.
            // Instead, we need to get the existing registration and use the already registered MessagePipeOptions.
            if (builder.Exists<MessagePipeOptions>())
            {
                // SpawnInstance doesn't require a resolver because MessagePipeOptions is registered as an instance.
                // Instance registrations are resolver-independent.
                return (MessagePipeOptions) builder.GetRegistration<MessagePipeOptions>().Build().SpawnInstance(null);
            }

            if (setAsGlobalProvider)
            {
                // If this is the first time that we are registering MessagePipe, it's probably a good time to register this resolver as GlobalProvider
                builder.RegisterBuildCallback(resolver =>
                {
                    GlobalMessagePipe.SetProvider(resolver.AsServiceProvider());
                });
            }

            return configure != null
                ? builder.RegisterMessagePipe(configure)
                : builder.RegisterMessagePipe();
        }
    }
}
#endif