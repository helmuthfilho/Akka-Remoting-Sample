using Akka.Actor;
using Akka.Configuration;
using Akka.DI.Core;
using Akka.Routing;
using Castle.Windsor;
using Client1Application.Worker.Guardian;
using System;

namespace Client1Application.Worker
{
    public class Startup
    {
        private static ActorSystem _actorSystem;

        public async void Stop()
        {
            await _actorSystem.Terminate();
        }

        public void Run()
        {
            using (var container = Bootstrapper.Initialize())
            {
                Start(container);
            }
        }

        public void Start(IWindsorContainer container)
        {
            var config = ConfigurationFactory.ParseString(@"
            akka{
                actor
                {
                    provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                }
                remote
                {
                    helios.tcp
                    {
                        port = 7000
                        hostname = 0.0.0.0
                        public-hostname = localhost
                    }
                }
            }
            ");

            _actorSystem = ActorSystem.Create("clientApplication1", config);

            var resolver = new DependencyResolver(container, _actorSystem);

            _actorSystem.ActorOf(_actorSystem.DI().Props<Client1ApplicationGuardian>().WithRouter(new RoundRobinPool(50, new DefaultResizer(1,100, 100))), "client1-application-guardian");
        }
    }
}
