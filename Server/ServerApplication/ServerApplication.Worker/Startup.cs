using Akka.Actor;
using Akka.Configuration;
using Akka.DI.Core;
using Castle.Windsor;
using ServerApplication.Worker.Guardian;
using System;
using System.IO;

namespace ServerApplication.Worker
{
    public class Startup
    {
        private static ActorSystem _actorSystem;
        private static string baseProjeto = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\TextToRead\\BASEPROJETO.txt";

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
                        port = 6000
                        hostname = 0.0.0.0
                        public-hostname = localhost
                    }
                }
            }
            ");

            _actorSystem = ActorSystem.Create("serverApplication", config);

            var resolver = new DependencyResolver(container, _actorSystem);

            var serverApplicationGuardian = _actorSystem.ActorOf(_actorSystem.DI().Props<ServerApplicationGuardian>(), "server-application-guardian");

            var lines = File.ReadAllLines(baseProjeto);

            serverApplicationGuardian.Tell(lines);
        }
    }
}
