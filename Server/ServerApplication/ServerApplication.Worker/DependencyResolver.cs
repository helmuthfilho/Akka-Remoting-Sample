using Akka.Actor;
using Akka.DI.Core;
using Castle.Windsor;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace ServerApplication.Worker
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly IWindsorContainer _container;
        private readonly ActorSystem _system;
        private readonly ConcurrentDictionary<string, Type> _typeCache;

        public DependencyResolver(IWindsorContainer container, ActorSystem system)
        {
            _container = container;
            _system = system;
            _typeCache = new ConcurrentDictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
            _system.AddDependencyResolver(this);
        }

        public Props Create<TActor>() where TActor : ActorBase
        {
            return Create(typeof(TActor));
        }

        public Props Create(Type actorType)
        {
            return _system.GetExtension<DIExt>().Props(actorType);
        }

        public Func<ActorBase> CreateActorFactory(Type actorType)
        {
            return () => (ActorBase)_container.Resolve(actorType);
        }

        public Type GetType(string actorName)
        {
            _typeCache.TryAdd(actorName,
                actorName.GetTypeValue() ??
                _container.Kernel
                    .GetAssignableHandlers(typeof(object))
                    .Where(handler =>
                        handler.ComponentModel.Name.Equals(actorName, StringComparison.InvariantCultureIgnoreCase))
                    .Select(handler => handler.ComponentModel.Implementation)
                    .FirstOrDefault());

            return _typeCache[actorName];
        }

        public void Release(ActorBase actor)
        {
            _container.Release(actor);
        }
    }
}